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
        public static MainWindow mainWindow = Init.mainWindow;
        public static List<int> shipSizes = new List<int>() { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        public static Dictionary<Player, Button[,]> buttons = Init.buttons;
        public static int currentShipInd = 0;
        public static Dictionary<Player, Ship[,]> field = Data.field;

        //static Button? prevButton = null;
        static List<Tuple<int, int>> prevCoord = new List<Tuple<int, int>>();



        public static void PlaceModeClicker(object sender, EventArgs e, int row, int column)
        {
            Button? pressedButton = sender as Button;

            if (pressedButton == null) throw new Exception("You clicked on a non-existent button");

            if (prevCoord.Contains(new Tuple<int, int>(row, column)))
            {
                prevCoord.Remove(new Tuple<int, int>(row, column));
            }
            else
            {
                pressedButton.Background = Brushes.Blue;
                prevCoord.Add(new Tuple<int, int>(row, column));
            }
            if (prevCoord.Count == shipSizes[currentShipInd])
            {
                foreach (var coord in prevCoord)
                {
                    field[Player.Player][coord.Item1, coord.Item2] = new Ship(shipSizes[currentShipInd], prevCoord, Player.Player);
                }

                prevCoord.Clear();
                currentShipInd++;
                //if (currentShipInd==shipSizes.Count) //StartGame();
            }
            PaintCells();
        }

        public static void StartPlacement()
        {
            PaintCells();
            UpdateNote();
        }

        public static void UpdateNote()
        {
            mainWindow.Note.Text = $"Поставьте {shipSizes[currentShipInd]}-палубный корабль";
        }

        public static void ClearNote()
        {
            mainWindow.Note.Text = string.Empty;
        }

        public static void PaintCells()
        {
            if (prevCoord.Count == 0) PaintPlacements();
            else if (prevCoord.Count == 1) PaintNextPlacements();
            else PaintNextNextPlacements();
        }
        public static void PaintPlacements()
        {
            for (int row = 1; row <= fieldSize; row++)
            {
                for (int column = 1; column <= fieldSize; column++)
                {
                    if ((field[Player.Player][row, column] == null) && (IsPlacementGood(row, column))) EnableButton(row, column);
                    else buttons[Player.Player][row, column].IsEnabled = false;
                }
            }
        }

        public static void EnableButton(int row,int column)
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
            for (int upDiff = -shipSizes[currentShipInd] + 1; upDiff <= 0; upDiff++)
            {
                for (int newRow = row + upDiff; newRow < row + upDiff + shipSizes[currentShipInd]; newRow++)
                {
                    if (!IsPlaceGood(row, newRow)) break;
                    if (newRow == row + upDiff + shipSizes[currentShipInd] - 1) return true;
                }
            }
            return false;
        }

        public static bool IsHorizontalPlacementGood(int row, int column)
        {
            for (int leftDiff = -shipSizes[currentShipInd] + 1; leftDiff <= 0; leftDiff++)
            {
                for (int newColumn = column + leftDiff; newColumn < column + leftDiff + shipSizes[currentShipInd]; newColumn++)
                {
                    if (!IsPlaceGood(row, newColumn)) break;
                    if (newColumn == column + leftDiff + shipSizes[currentShipInd] - 1) return true;
                }
            }
            return false;
        }

        public static bool IsPlaceGood(int row, int column)
        {
            return ((InRange(row)) && (InRange(column)) && (playerField[row, column] == null) && (ShipIsNotAround(row, column)));
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

                    if ((InRange(newRow)) && (InRange(newColumn)) && (playerField[newRow, newColumn] != null)) return false;
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
            PaintUp();
        }

        public static void PaintHorizontal()
        {

        }
    }
}
