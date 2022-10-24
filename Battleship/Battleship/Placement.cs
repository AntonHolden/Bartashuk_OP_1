using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Battleship.Init;
using static Battleship.Data;
using System.Windows.Media;

namespace Battleship
{
    public static class Placement
    {
        public static Dictionary<Player, Button[,]> buttons = Init.buttons;

        public static int selectedShipSize = 0;

        public static bool IsFull(int size) => shipsPlaced[size] == 5 - selectedShipSize;

        public static bool AllFull()
        {
            foreach (var ship in shipsPlaced)
            {
                if (!IsFull(ship.Key)) return false;
            }
            return true;
        }

        public static List<Tuple<int, int>> prevPlacementCoords = new List<Tuple<int, int>>();

        public static void PlaceModeClicker(object sender, EventArgs e, int row, int column)
        {
            Button? pressedButton = sender as Button;

            if (pressedButton == null) throw new Exception("You clicked on a non-existent button");

            if (prevPlacementCoords.Contains(new Tuple<int, int>(row, column)))
            {
                prevPlacementCoords.Remove(new Tuple<int, int>(row, column));
            }
            else if (field[Player.Player][row, column] != null)
            {
                RemoveShip(row, column);
                UpdatePlacementsNotes();
                UpdatePlacementButtons();
            }
            else
            {
                pressedButton.Background = Brushes.Blue;
                prevPlacementCoords.Add(new Tuple<int, int>(row, column));
                PaintCells();
            }
            if (prevPlacementCoords.Count == selectedShipSize)
            {
                AddShip(prevPlacementCoords);
                UpdatePlacementsNotes();
                UpdatePlacementButtons();

                prevPlacementCoords.Clear();

                prevPlacementButton = null;
                DisableEmptyCells(Player.Player);
                selectedShipSize = 0;
                //if (AllFull) // ClearNote(); DisableButtons(); StartGame();
            }

        }

        public static void UpdatePlacementButtons()
        {
            foreach (var placementButton in placementButtonsToSize)
            {
                if (IsFull(placementButton.Value)) placementButton.Key.IsEnabled = false;
                else placementButton.Key.IsEnabled = true;
            }
        }

        public static void AddShip(List<Tuple<int, int>> coords)
        {
            foreach (var coord in coords) field[Player.Player][coord.Item1, coord.Item2] = new Ship(selectedShipSize, prevPlacementCoords, Player.Player);

            PaintShip(coords);
            shipsPlaced[selectedShipSize]++;
        }

        public static void RemoveShip(int row, int column)
        {

            var coords = field[Player.Player][row, column].shipCoords;
            shipsPlaced[field[Player.Player][row, column].shipSize]--;

            foreach (var coord in coords) field[Player.Player][coord.Item1, coord.Item2] = null;

            UnPaintShip(coords);
        }
        public static void PaintShip(List<Tuple<int, int>> coords)
        {
            foreach (var coord in coords)
            {
                Border? border = (Border?)GetGridBorder(grids[Player.Player], coord.Item1, coord.Item2);

                if (border == null) throw new Exception("Can't paint ship!");

                border.Background = Brushes.Blue;
            }
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
            UpdateNote();
            UpdatePlacementsNotes();
        }

        public static void UpdateNote() => mainWindow.Note.Text = $"Расставьте корабли!";

        public static void ClearNote() => mainWindow.Note.Text = string.Empty;

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
                    if ((field[Player.Player][row, column] == null) && (IsPlacementGood(row, column))) AllowToPlace(row, column);
                    else if (field[Player.Player][row, column] != null) buttons[Player.Player][row, column].IsEnabled = true;
                    else buttons[Player.Player][row, column].IsEnabled = false;
                }
            }
        }

        public static void AllowToPlace(int row, int column)
        {
            buttons[Player.Player][row, column].IsEnabled = true;
            buttons[Player.Player][row, column].Background = (Brush)(new BrushConverter().ConvertFrom("#FF6A5ACD"));
        }
        public static bool IsPlacementGood(int row, int column)
        {
            if ((IsHorizontalPlacementGood(row, column)) || (IsVerticalPlacementGood(row, column))) return true;

            return false;
        }

        public static bool IsVerticalPlacementGood(int row, int column)
        {
            for (int upDiff = -selectedShipSize + 1; upDiff <= 0; upDiff++)
            {
                for (int newRow = row + upDiff; newRow < row + upDiff + selectedShipSize; newRow++)
                {
                    if (!IsPlaceGood(row, newRow)) break;
                    if (newRow == row + upDiff + selectedShipSize - 1) return true;
                }
            }
            return false;
        }

        public static bool IsHorizontalPlacementGood(int row, int column)
        {
            for (int leftDiff = -selectedShipSize + 1; leftDiff <= 0; leftDiff++)
            {
                for (int newColumn = column + leftDiff; newColumn < column + leftDiff + selectedShipSize; newColumn++)
                {
                    if (!IsPlaceGood(row, newColumn)) break;
                    if (newColumn == column + leftDiff + selectedShipSize - 1) return true;
                }
            }
            return false;
        }

        public static bool IsPlaceGood(int row, int column)
        {
            return ((InRange(row)) && (InRange(column)) && (field[Player.Player][row, column] == null) && (ShipIsNotAround(row, column)));
        }
        public static bool ShipIsNotAround(int row, int column)
        {
            foreach (int rowDiff in new List<int> { -1, 0, 1 })
            {
                foreach (int columnDiff in new List<int> { -1, 0, 1 })
                {
                    if ((rowDiff == 0) && (columnDiff == 0)) continue;

                    int newRow = row + rowDiff;
                    int newColumn = column + columnDiff;

                    if ((InRange(newRow)) && (InRange(newColumn)) && (field[Player.Player][newRow, newColumn] != null)) return false;
                }
            }
            return true;
        }

        public static bool InRange(int x) => ((x >= 1) && (x <= fieldSize));

        public static void PaintNextPlacements()
        {

        }

        public static void PaintNextNextPlacements()
        {

        }

        public static void PaintVertical()
        {
            //PaintUp();
        }

        public static void PaintHorizontal()
        {

        }

        public static void UpdateShipsLeft()
        {

        }

        public static void UpdatePlacementsNotes()
        {
            foreach (var shipSize in shipsPlaced) sizeToPlacementNotes[shipSize.Key].Text = $"Осталось: {5 - shipSize.Key - shipSize.Value}";
        }
    }
}
