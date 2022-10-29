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
using static Battleship.Data;
using static Battleship.Init;
using static Battleship.BotPlacement;
using static Battleship.Placement;
using System.Data.Common;
using System.Threading;

namespace Battleship
{
    public static class Game
    {
        public static Dictionary<Player, Button[,]> buttons = Placement.buttons;
        public static List<Tuple<int, int>> possibleCoords = new List<Tuple<int, int>>();
        public static List<Tuple<int, int>> hittedCoords = new List<Tuple<int, int>>();

        public static List<Tuple<int, int>> enabledButtonsCoords = new List<Tuple<int, int>>();

        public static void StartGame()
        {
            mainWindow.StartButton.Visibility = Visibility.Hidden;
            foreach (var placementButton in placementButtonsToSize) placementButton.Key.Visibility = Visibility.Hidden;
            foreach (var placementNote in sizeToPlacementNotes) placementNote.Value.Visibility = Visibility.Hidden;
            mainWindow.PlayerShipsLeftNote.Visibility = Visibility.Visible;
            mainWindow.OpponentShipsLeftNote.Visibility = Visibility.Visible;
            mainWindow.State.FontSize = stateFontSize;

            FillCoords(possibleCoords);
            FillCoords(enabledButtonsCoords);

            AllPlayerButtonsAreEnabled(Player.Player, false);
            AllPlayerButtonsAreEnabled(Player.Opponent, true);
            UpdateShipsLeftNotes();
        }

        public static void FillCoords(List<Tuple<int, int>> coords)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++) coords.Add(new Tuple<int, int>(i, j));
            }
        }

        public static void UpdateShipsLeftNotes()
        {
            string text = "У вас осталось:";
            foreach (var shipSize in shipsPlaced[Player.Player]) text += $"\n{shipSize.Key}-палубных кораблей: {shipSize.Value}";
            mainWindow.PlayerShipsLeftNote.Text = text;

            text = "У противника осталось:";
            foreach (var shipSize in shipsPlaced[Player.Opponent]) text += $"\n{shipSize.Key}-палубных кораблей: {shipSize.Value}";
            mainWindow.OpponentShipsLeftNote.Text = text;
        }

        public static bool IsPlayerWon(Player player)
        {
            foreach (var ship in shipsPlaced[3 - player])
            {
                if (ship.Value != 0) return false;
            }
            return true;
        }

        public static void EnableOpponentButtons()
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++)
                {
                    if (enabledButtonsCoords.Contains(new Tuple<int, int>(i, j))) buttons[Player.Opponent][i, j].IsEnabled = true;
                }
            }
        }

        async public static void BotMove()
        {
            AllPlayerButtonsAreEnabled(Player.Opponent, false);
            await Task.Delay(1);
            Thread.Sleep(999);
            EnableOpponentButtons();

            Tuple<int, int> coord;
            if (hittedCoords.Count > 0) coord = LookForPlayerShip();
            else coord = possibleCoords[randomizer.Next(possibleCoords.Count)];

            if (coord == null) throw new Exception("Something wrong with looking for player's ship by bot!");

            possibleCoords.Remove(coord);

            if (field[Player.Player][coord.Item1, coord.Item2] == null)
            {
                MakeMissImage(Player.Player, coord.Item1, coord.Item2);
                mainWindow.State.Foreground = Brushes.Blue;
                mainWindow.State.Text = $"Ход противника: {columnToLetter[coord.Item2]}{coord.Item1}\nПромах!";
            }
            else
            {
                MakeHitImage(Player.Player, coord.Item1, coord.Item2);
                field[Player.Player][coord.Item1, coord.Item2].Hit(coord.Item1, coord.Item2);
                if (field[Player.Player][coord.Item1, coord.Item2].isDefeated) hittedCoords.Clear();
                else hittedCoords.Add(coord);
            }

            if (IsPlayerWon(Player.Opponent)) End(false);
        }

        public static Tuple<int, int> LookForPlayerShip()
        {
            if (hittedCoords.Count > 1) return LookForPlayerShipNext();

            var coords = new List<Tuple<int, int>>();
            int row = hittedCoords[0].Item1, column = hittedCoords[0].Item2;

            if (IsCanHit(row, column - 1)) coords.Add(new Tuple<int, int>(row, column - 1));
            if (IsCanHit(row, column + 1)) coords.Add(new Tuple<int, int>(row, column + 1));
            if (IsCanHit(row - 1, column)) coords.Add(new Tuple<int, int>(row - 1, column));
            if (IsCanHit(row + 1, column)) coords.Add(new Tuple<int, int>(row + 1, column));

            return coords[randomizer.Next(coords.Count)];
        }

        public static Tuple<int, int> LookForPlayerShipNext()
        {
            var coords = new List<Tuple<int, int>>();

            if (hittedCoords[0].Item1 == hittedCoords[1].Item1)       //horizontal
            {
                int rightCoord = 1, leftCoord = fieldSize, row = hittedCoords[0].Item1;
                foreach (var coord in hittedCoords)
                {
                    rightCoord = Math.Max(rightCoord, coord.Item2);
                    leftCoord = Math.Min(leftCoord, coord.Item2);
                }

                if (IsCanHit(row, leftCoord - 1)) coords.Add(new Tuple<int, int>(row, leftCoord - 1));
                if (IsCanHit(row, rightCoord + 1)) coords.Add(new Tuple<int, int>(row, rightCoord + 1));
            }
            else                                                                    //vertical
            {
                int downCoord = 1, upCoord = fieldSize, column = hittedCoords[1].Item2;
                foreach (var coord in hittedCoords)
                {
                    downCoord = Math.Max(downCoord, coord.Item1);
                    upCoord = Math.Min(upCoord, coord.Item1);
                }

                if (IsCanHit(upCoord - 1, column)) coords.Add(new Tuple<int, int>(upCoord - 1, column));
                if (IsCanHit(downCoord + 1, column)) coords.Add(new Tuple<int, int>(downCoord + 1, column));
            }

            return coords[randomizer.Next(coords.Count)];
        }

        public static bool IsCanHit(int row, int column) =>
            ((possibleCoords.Contains(new Tuple<int, int>(row, column))) &&
            (AreNotAroundShipsDefeated(row, column)));

        public static bool AreNotAroundShipsDefeated(int row, int column)
        {
            foreach (int rowDiff in new List<int> { -1, 0, 1 })
            {
                foreach (int columnDiff in new List<int> { -1, 0, 1 })
                {
                    if ((rowDiff == 0) && (columnDiff == 0)) continue;

                    int newRow = row + rowDiff;
                    int newColumn = column + columnDiff;

                    if ((InRange(newRow)) &&
                        (InRange(newColumn)) &&
                        (field[Player.Player][newRow, newColumn] != null) &&
                        (field[Player.Player][newRow, newColumn].isDefeated)) return false;
                }
            }
            return true;
        }

        public static void End(bool isPlayerWon)
        {
            if (isPlayerWon)
            {
                mainWindow.State.Foreground = Brushes.Green;
                mainWindow.State.FontSize = stateFontSize * 1.2;
                mainWindow.State.Text = "Вы победили!";
            }
            else
            {
                mainWindow.State.Foreground = Brushes.Red;
                mainWindow.State.FontSize = stateFontSize * 1.2;
                mainWindow.State.Text = "Вы проиграли!";
            }
            AllPlayerButtonsAreEnabled(Player.Opponent, false);
        }
    }
}
