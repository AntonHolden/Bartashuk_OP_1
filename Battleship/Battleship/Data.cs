using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Battleship.Placement;
using static Battleship.BotPlacement;
using static Battleship.Init;
using static Battleship.Game;
using System.Data.Common;
using System.Threading;

namespace Battleship
{
    public static class Data
    {
        public enum Player
        {
            Player = 1,
            Opponent
        }

        public class Ship
        {
            public int shipSize;
            public List<Tuple<int, int>> shipCoords;
            public bool isHitted = false;
            public bool isDefeated = false;
            private Player player;
            public Ship(int shipSize, List<Tuple<int, int>> shipCoords, Player player)
            {
                this.shipSize = shipSize;
                this.shipCoords = shipCoords;
                this.player = player;
            }

            public void Hit(int row, int column)
            {
                isHitted = true;
                if (IsDefeat())
                {
                    foreach (var coord in shipCoords)
                    {
                        MakeDefeatImage(player, coord.Item1, coord.Item2);
                        field[player][coord.Item1, coord.Item2].isDefeated = true;
                    }

                    if (player == Player.Opponent)
                    {
                        mainWindow.State.Foreground = Brushes.Green;
                        mainWindow.State.Text = $"Ваш ход: {columnToLetter[column]}{row}\nВы потопили корабль!";
                    }
                    else
                    {
                        mainWindow.State.Foreground = Brushes.Red;
                        mainWindow.State.Text = $"Ход противника: {columnToLetter[column]}{row}\nВаш корабль потопили!";
                    }
                    shipsPlaced[player][shipSize]--;
                    UpdateShipsLeftNotes();
                }
                else
                {
                    if (player == Player.Opponent)
                    {
                        mainWindow.State.Foreground = Brushes.Green;
                        mainWindow.State.Text = $"Ваш ход: {columnToLetter[column]}{row}\nПопадание!";
                    }
                    else
                    {
                        mainWindow.State.Foreground = Brushes.Red;
                        mainWindow.State.Text = $"Ход противника: {columnToLetter[column]}{row}\nПопадание!";
                    }
                }
            }
            private bool IsDefeat()
            {
                foreach (var coord in shipCoords)
                {
                    if (!field[player][coord.Item1, coord.Item2].isHitted) return false;
                }
                return true;
            }
        }

        public static UIElement? GetGridBorder(Grid grid, int row, int column)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement child = grid.Children[i];
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column && child is Border)
                    return child;
            }
            return null;
        }

        public static double stateFontSize = (SystemParameters.PrimaryScreenWidth * 0.8) / 36;
        public static double cellSize = SystemParameters.PrimaryScreenWidth * 0.8 * 0.47 / 11;

        public static Dictionary<int, string> columnToLetter = new Dictionary<int, string>();

        public static void InitColumnToLetter()
        {
            char letter = 'A';
            for (int i = 1; i <= fieldSize; i++)
            {
                columnToLetter[i] = letter.ToString();
                letter++;
            }
        }

        public static void MakeMissImage(Player player, int row, int column)
        {
            Border? border = (Border?)GetGridBorder(grids[player], row, column);
            if (border == null) throw new Exception($"Can't get border in MakeMissImage!");

            Image image = new Image();
            image.Source = new BitmapImage(new Uri("Resources/miss.png", UriKind.Relative));
            image.Width = cellSize * 0.39;
            image.Height = cellSize * 0.39;

            border.Child = image;
        }
        public static void MakeHitImage(Player player, int row, int column)
        {
            Border? border = (Border?)GetGridBorder(grids[player], row, column);
            if (border == null) throw new Exception($"Can't get border in MakeHitImage!");

            Image image = new Image();
            image.Source = new BitmapImage(new Uri("Resources/hit.png", UriKind.Relative));
            image.Width = cellSize * 0.54;
            image.Height = cellSize * 0.54;

            border.Child = image;
        }
        public static void MakeDefeatImage(Player player, int row, int column)
        {
            Border? border = (Border?)GetGridBorder(grids[player], row, column);
            if (border == null) throw new Exception($"Can't get border in MakeDefeatImage!");

            Image image = new Image();
            image.Source = new BitmapImage(new Uri("Resources/defeat.png", UriKind.Relative));
            image.Width = cellSize * 0.85;
            image.Height = cellSize * 0.85;

            border.Child = image;
        }

        public const int fieldSize = 10;

        public static Dictionary<Player, Button[,]> buttons = new Dictionary<Player, Button[,]>()
        {
            {Player.Player, new Button[fieldSize + 1, fieldSize + 1]},
            {Player.Opponent,new Button[fieldSize + 1, fieldSize + 1] }
        };

        public static Dictionary<Player, Ship[,]> field = new Dictionary<Player, Ship[,]>()
        {
            {Player.Player, new Ship[fieldSize + 1, fieldSize + 1]},
            {Player.Opponent,new Ship[fieldSize + 1, fieldSize + 1] }
        };

        public static void ClickOnOpponentCell(object sender, EventArgs e, int row, int column)
        {
            if (field[Player.Opponent][row, column] == null)
            {
                MakeMissImage(Player.Opponent, row, column);
                mainWindow.State.Foreground = Brushes.Blue;
                mainWindow.State.Text = $"Ваш ход: {columnToLetter[column]}{row}\nПромах!";
            }
            else
            {
                MakeHitImage(Player.Opponent, row, column);
                field[Player.Opponent][row, column].Hit(row, column);
            }

            buttons[Player.Opponent][row, column].IsEnabled = false;
            enabledButtonsCoords.Remove(new Tuple<int, int>(row, column));

            if (IsPlayerWon(Player.Player)) End(true);
            else BotMove();
        }

        public static Dictionary<Player, Dictionary<int, int>> shipsPlaced = new Dictionary<Player, Dictionary<int, int>>     //value.key - size, value.value - count
        {
            {Player.Player, GetZeroShipsPlacedCount()},

            {Player.Opponent, GetZeroShipsPlacedCount()}
        };

        public static void DisableEmptyCells(Player player)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++)
                {
                    if (field[player][i, j] != null) buttons[player][i, j].IsEnabled = true;
                    else UnAllowToPlace(player, i, j);
                }
            }
        }

        public static Button? prevPlacementButton = null;
        public static void PlacementButtonsClicker(object sender, EventArgs e)
        {
            Button? pressedButton = sender as Button;
            if (pressedButton == null) throw new Exception("You clicked on a non-existent placementButton");

            prevPlacementCoords.Clear();

            if (pressedButton == prevPlacementButton)
            {
                DisableEmptyCells(Player.Player);
                prevPlacementButton = null;
                selectedShipSize = -1;
            }
            else
            {
                prevPlacementButton = pressedButton;
                selectedShipSize = placementButtonsToSize[pressedButton];
                PaintCells();
            }

            UpdateNotes();
        }

        public static bool IsFull(Player player, int size) => shipsPlaced[player][size] == 5 - size;

        public static bool AllFull(Player player)
        {
            foreach (var ship in shipsPlaced[player])
            {
                if (!IsFull(player, ship.Key)) return false;
            }
            return true;
        }
        public static void AllPlayerButtonsAreEnabled(Player player, bool isEnabled)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++) buttons[player][i, j].IsEnabled = isEnabled;
            }
        }

        public static void StartButtonClicker(object sender, EventArgs e) => StartGame();

        public static void RestartButtonClicker(object sender, EventArgs e) => Restart();

        public static Dictionary<int, int> GetZeroShipsPlacedCount()
        {
            return new Dictionary<int, int>()
                {
                    { 1,0 },
                    { 2,0 },
                    { 3,0 },
                    { 4,0 }
                };
        }

        public static void Restart()
        {
            prevPlacementCoords.Clear();
            selectedShipSize = -1;
            prevPlacementButton = null;

            shipsPlaced = new Dictionary<Player, Dictionary<int, int>>
            {
                {Player.Player, GetZeroShipsPlacedCount()},
                {Player.Opponent, GetZeroShipsPlacedCount()}
            };

            field = new Dictionary<Player, Ship[,]>()
            {
                {Player.Player, new Ship[fieldSize + 1, fieldSize + 1]},
                {Player.Opponent,new Ship[fieldSize + 1, fieldSize + 1] }
            };

            DisableEmptyCells(Player.Player);
            DisableEmptyCells(Player.Opponent);
            mainWindow.StartButton.Visibility = Visibility.Visible;
            foreach (var placementButton in placementButtonsToSize) placementButton.Key.Visibility = Visibility.Visible;
            foreach (var placementNote in sizeToPlacementNotes) placementNote.Value.Visibility = Visibility.Visible;
            mainWindow.PlayerShipsLeftNote.Visibility = Visibility.Hidden;
            mainWindow.OpponentShipsLeftNote.Visibility = Visibility.Hidden;
            mainWindow.State.FontSize = stateFontSize;
            mainWindow.State.Foreground = Brushes.DarkRed;

            possibleCoords.Clear();
            hittedCoords.Clear();
            enabledButtonsCoords.Clear();

            StartBotPlacement();
            StartPlacement();
        }
    }
}