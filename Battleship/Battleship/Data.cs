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
                    foreach (var coord in shipCoords) MakeDefeatImage(player, coord.Item1, coord.Item2);

                    if (player == Player.Opponent)
                    {
                        mainWindow.State.Foreground = Brushes.Green;
                        mainWindow.State.Text = $"Ход:{columnToLetter[column]}{row}\nВы потопили корабль противника!";
                    }
                    else
                    {
                        mainWindow.State.Foreground = Brushes.Red;
                        mainWindow.State.Text = $"Ход:{columnToLetter[column]}{row}\nПротивник потопил ваш корабль!";
                    }
                    shipsPlaced[player][shipSize]--;
                    UpdateShipsLeftNotes();
                }
                else
                {
                    if (player == Player.Opponent)
                    {
                        mainWindow.State.Foreground = Brushes.Green;
                        mainWindow.State.Text = $"Ход:{columnToLetter[column]}{row}\nВы попали по кораблю противника!";
                    }
                    else
                    {
                        mainWindow.State.Foreground = Brushes.Red;
                        mainWindow.State.Text = $"Ход:{columnToLetter[column]}{row}\nПротивник попал по вашему кораблю!";
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
            image.Width = 20;
            image.Height = 20;

            border.Child = image;
        }
        public static void MakeHitImage(Player player, int row, int column)
        {
            Border? border = (Border?)GetGridBorder(grids[player], row, column);
            if (border == null) throw new Exception($"Can't get border in MakeHitImage!");

            Image image = new Image();
            image.Source = new BitmapImage(new Uri("Resources/hit.png", UriKind.Relative));
            image.Width = 30;
            image.Height = 30;

            border.Child = image;
        }
        public static void MakeDefeatImage(Player player, int row, int column)
        {
            Border? border = (Border?)GetGridBorder(grids[player], row, column);
            if (border == null) throw new Exception($"Can't get border in MakeDefeatImage!");

            Image image = new Image();
            image.Source = new BitmapImage(new Uri("Resources/defeat.png", UriKind.Relative));
            image.Width = 55;
            image.Height = 55;

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
                mainWindow.State.Text = $"Ход:{columnToLetter[column]}{row}\nВы промахнулись";
            }
            else
            {
                MakeHitImage(Player.Opponent, row, column);
                field[Player.Opponent][row, column].Hit(row,column);
            }

            buttons[Player.Opponent][row, column].IsEnabled = false;

            if (IsPlayerWon(Player.Player)) End(true);
            else BotMove();
        }

        public static Dictionary<Player, Dictionary<int, int>> shipsPlaced = new Dictionary<Player, Dictionary<int, int>>     //value.key - size, value.value - count
        {
            {Player.Player, new Dictionary<int, int>()
            {
                { 1,0 },
                { 2,0 },
                { 3,0 },
                { 4,0 }
            }
            },

            {Player.Opponent, new Dictionary<int, int>()
            {
                { 1,0 },
                { 2,0 },
                { 3,0 },
                { 4,0 }
            }
            }
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
            prevPlacementCoords.Clear();
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
        public static void EnableAllButtons(Player player)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++) buttons[player][i, j].IsEnabled = true;
            }
        }

        public static void DisableAllButtons(Player player)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++) buttons[player][i, j].IsEnabled = false;
            }
        }
        public static void StartButtonClicker(object sender, EventArgs e)
        {
            StartGame();
        }

        public static void RestartButtonClicker(object sender, EventArgs e)
        {
            Restart();
        }

        public static void Restart()
        {
            prevPlacementCoords.Clear();
            selectedShipSize = -1;
            prevPlacementButton = null;

            shipsPlaced = new Dictionary<Player, Dictionary<int, int>>
            {
                {Player.Player, new Dictionary<int, int>()
                {
                    { 1,0 },
                    { 2,0 },
                    { 3,0 },
                    { 4,0 }
                }
                },

                {Player.Opponent, new Dictionary<int, int>()
                {
                    { 1,0 },
                    { 2,0 },
                    { 3,0 },
                    { 4,0 }
                }
                }
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
            mainWindow.State.FontSize = 40;
            mainWindow.State.Foreground = Brushes.DarkRed;

            possibleCoords.Clear();

            StartBotPlacement();
            StartPlacement();
        }
    }
}