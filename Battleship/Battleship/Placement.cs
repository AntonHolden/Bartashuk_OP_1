using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Battleship.Init;
using static Battleship.Data;
using System.Windows.Media;
using System.Data.Common;
using System.Reflection;

namespace Battleship
{
    public static class Placement
    {
        public static Dictionary<Player, Button[,]> buttons = Init.buttons;

        public static int selectedShipSize = -1;

        public static List<Tuple<int, int>> prevPlacementCoords = new List<Tuple<int, int>>();

        public static void PlaceModeClicker(object sender, EventArgs e, int row, int column)
        {
            Button? pressedButton = sender as Button;

            if (pressedButton == null) throw new Exception("You clicked on a non-existent button");

            if (prevPlacementCoords.Contains(new Tuple<int, int>(row, column)))
            {
                UnPaintBorder(Player.Player, row, column);
                prevPlacementCoords.Remove(new Tuple<int, int>(row, column));
                PaintCells();
            }
            else if (field[Player.Player][row, column] != null)
            {
                RemoveShip(row, column);
                UpdateNotes();
                UpdateButtons();
                DisableEmptyCells(Player.Player);
            }
            else
            {
                Border? border = (Border?)GetGridBorder(grids[Player.Player], row, column);

                if (border == null) throw new Exception($"Can't PaintBorder in {row} row in {column} column in PlaceModeClicker!");

                border.Background = Brushes.Blue;
                prevPlacementCoords.Add(new Tuple<int, int>(row, column));
                PaintCells();
            }
            if (prevPlacementCoords.Count == selectedShipSize)
            {
                AddShip(prevPlacementCoords);
                UpdateNotes();
                UpdateButtons();

                prevPlacementCoords.Clear();

                prevPlacementButton = null;
                DisableEmptyCells(Player.Player);
                selectedShipSize = -1;
            }
        }

        public static void UpdateButtons()
        {
            UpdatePlacementButtons();

            if (!AllFull(Player.Player)) mainWindow.StartButton.IsEnabled = false;
            else mainWindow.StartButton.IsEnabled = true;
        }

        public static void UpdateNotes()
        {
            UpdatePlacementsNotes();

            if (!AllFull(Player.Player)) mainWindow.State.Text = "Расставьте корабли!";
            else mainWindow.State.Text = string.Empty;
        }

        public static void UpdatePlacementButtons()
        {
            foreach (var placementButton in placementButtonsToSize)
            {
                if (IsFull(Player.Player, placementButton.Value)) placementButton.Key.IsEnabled = false;
                else placementButton.Key.IsEnabled = true;
            }
        }

        public static void AddShip(List<Tuple<int, int>> coords)
        {
            foreach (var coord in coords)
                field[Player.Player][coord.Item1, coord.Item2] = new Ship(selectedShipSize, prevPlacementCoords.Select(i => new Tuple<int, int>(i.Item1, i.Item2)).ToList(), Player.Player);

            shipsPlaced[Player.Player][selectedShipSize]++;
        }

        public static void RemoveShip(int row, int column)
        {

            var coords = field[Player.Player][row, column].shipCoords;
            shipsPlaced[Player.Player][field[Player.Player][row, column].shipSize]--;

            foreach (var coord in coords) field[Player.Player][coord.Item1, coord.Item2] = null;

            UnPaintShip(coords);
        }

        public static void UnPaintShip(List<Tuple<int, int>> coords)
        {
            foreach (var coord in coords)
            {
                Border? border = (Border?)GetGridBorder(grids[Player.Player], coord.Item1, coord.Item2);

                if (border == null) throw new Exception("Can't unpaint ship!");

                border.Background = Brushes.Transparent;
            }
        }

        public static void StartPlacement()
        {
            UpdateNotes();
            UpdateButtons();
        }

        public static void UnPaintBorder(Player player, int row, int column)
        {
            Border? border = (Border?)GetGridBorder(grids[player], row, column);

            if (border == null) throw new Exception($"Can't UnPaintBorder in {row} row in {column} column!");

            border.Background = Brushes.Transparent;
            border.Child = null;
        }

        public static void PaintBorder(int row, int column)
        {
            Border? border = (Border?)GetGridBorder(grids[Player.Player], row, column);

            if (border == null) throw new Exception($"Can't PaintBorder in {row} row in {column} column!");

            border.Background = (Brush)(new BrushConverter().ConvertFrom("#FF6A5ACD"));
        }
        public static void AllowToPlace(int row, int column)
        {
            buttons[Player.Player][row, column].IsEnabled = true;
            PaintBorder(row, column);
        }
        public static bool IsPlacementGood(int size, int row, int column) => ((IsHorizontalPlacementGood(size, row, column)) || (IsVerticalPlacementGood(size, row, column)));

        public static bool IsVerticalPlacementGood(int size, int row, int column)
        {
            for (int upDiff = -size + 1; upDiff <= 0; upDiff++)
            {
                for (int newRow = row + upDiff; newRow < row + upDiff + size; newRow++)
                {
                    if (!IsPlaceGood(Player.Player, newRow, column)) break;
                    if (newRow == row + upDiff + size - 1) return true;
                }
            }
            return false;
        }

        public static bool IsHorizontalPlacementGood(int size, int row, int column)
        {
            for (int leftDiff = -size + 1; leftDiff <= 0; leftDiff++)
            {
                for (int newColumn = column + leftDiff; newColumn < column + leftDiff + size; newColumn++)
                {
                    if (!IsPlaceGood(Player.Player, row, newColumn)) break;
                    if (newColumn == column + leftDiff + size - 1) return true;
                }
            }
            return false;
        }

        public static bool IsPlaceGood(Player player, int row, int column) =>
            ((InRange(row)) &&
            (InRange(column)) &&
            (field[player][row, column] == null) &&
            (ShipIsNotAround(player, row, column)));

        public static bool ShipIsNotAround(Player player, int row, int column)
        {
            foreach (int rowDiff in new List<int> { -1, 0, 1 })
            {
                foreach (int columnDiff in new List<int> { -1, 0, 1 })
                {
                    if ((rowDiff == 0) && (columnDiff == 0)) continue;

                    int newRow = row + rowDiff;
                    int newColumn = column + columnDiff;

                    if ((InRange(newRow)) && (InRange(newColumn)) && (field[player][newRow, newColumn] != null)) return false;
                }
            }
            return true;
        }

        public static bool InRange(int x) => ((x >= 1) && (x <= fieldSize));


        public static void UnAllowToPlace(Player player, int row, int column)
        {
            buttons[player][row, column].IsEnabled = false;
            UnPaintBorder(player, row, column);
        }
        public static void PaintCells()
        {
            if (prevPlacementCoords.Count == 0) PaintPlacements();
            else if (prevPlacementCoords.Count == 1) PaintNextPlacements();
            else PaintNextNextPlacements();
        }

        public static void PaintPlacements()
        {
            for (int row = 1; row <= fieldSize; row++)
            {
                for (int column = 1; column <= fieldSize; column++)
                {
                    if ((field[Player.Player][row, column] == null) && (IsPlacementGood(selectedShipSize, row, column))) AllowToPlace(row, column);
                    else if (field[Player.Player][row, column] != null) buttons[Player.Player][row, column].IsEnabled = false;
                    else UnAllowToPlace(Player.Player, row, column);
                }
            }
        }
        public static void PaintNextPlacements()
        {
            DisableExtraButtons(false, false, 0, 0);

            int row = prevPlacementCoords[0].Item1, column = prevPlacementCoords[0].Item2;
            if (IsHorizontalPlacementGood(selectedShipSize, row, column))
            {
                if (IsPlaceGood(Player.Player, row, column - 1)) AllowToPlace(row, column - 1);
                if (IsPlaceGood(Player.Player, row, column + 1)) AllowToPlace(row, column + 1);
            }
            if (IsVerticalPlacementGood(selectedShipSize, row, column))
            {
                if (IsPlaceGood(Player.Player, row - 1, column)) AllowToPlace(row - 1, column);
                if (IsPlaceGood(Player.Player, row + 1, column)) AllowToPlace(row + 1, column);
            }
        }

        public static void PaintNextNextPlacements()
        {
            if (prevPlacementCoords[0].Item1 == prevPlacementCoords[1].Item1)       //horizontal
            {
                int rightCoord = 1, leftCoord = fieldSize, row = prevPlacementCoords[0].Item1;
                foreach (var coord in prevPlacementCoords)
                {
                    rightCoord = Math.Max(rightCoord, coord.Item2);
                    leftCoord = Math.Min(leftCoord, coord.Item2);
                }

                DisableExtraButtons(true, true, leftCoord, rightCoord);

                if (IsPlaceGood(Player.Player, row, leftCoord - 1)) AllowToPlace(row, leftCoord - 1);
                if (IsPlaceGood(Player.Player, row, rightCoord + 1)) AllowToPlace(row, rightCoord + 1);
            }
            else                                                                    //vertical
            {
                int downCoord = 1, upCoord = fieldSize, column = prevPlacementCoords[1].Item2;
                foreach (var coord in prevPlacementCoords)
                {
                    downCoord = Math.Max(downCoord, coord.Item1);
                    upCoord = Math.Min(upCoord, coord.Item1);
                }

                DisableExtraButtons(true, false, upCoord, downCoord);

                if (IsPlaceGood(Player.Player, upCoord - 1, column)) AllowToPlace(upCoord - 1, column);
                if (IsPlaceGood(Player.Player, downCoord + 1, column)) AllowToPlace(downCoord + 1, column);
            }
        }

        public static void DisableExtraButtons(bool advancedCheck, bool isHorizontal, int firstBorder, int secondBorder)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++)
                {
                    if (!prevPlacementCoords.Contains(new Tuple<int, int>(i, j)))
                    {
                        if (field[Player.Player][i, j] != null) buttons[Player.Player][i, j].IsEnabled = false;
                        else UnAllowToPlace(Player.Player, i, j);
                    }
                    else if (advancedCheck)
                    {
                        if (isHorizontal)
                        {
                            if ((j != firstBorder) && (j != secondBorder)) buttons[Player.Player][i, j].IsEnabled = false;
                        }
                        else
                        {
                            if ((i != firstBorder) && (i != secondBorder)) buttons[Player.Player][i, j].IsEnabled = false;
                        }
                    }
                }
            }
        }

        public static void UpdatePlacementsNotes()
        {
            foreach (var shipSize in shipsPlaced[Player.Player]) sizeToPlacementNotes[shipSize.Key].Text = $"Осталось: {5 - shipSize.Key - shipSize.Value}";
        }
    }
}
