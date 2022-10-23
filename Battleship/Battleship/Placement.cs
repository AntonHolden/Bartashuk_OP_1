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
        public static Button[,] playerButtons = Init.playerButtons;
        public static Button[,] opponentButtons = Init.opponentButtons;
        public static int currentShipInd = 0;

        public static Ship[,] playerField = Data.playerField;
        public static Ship[,] opponentField = Data.opponentField;

        public static Button? prevButton = null;
        public static Tuple<int?, int?> prevCoord = new Tuple<int?, int?>(null, null);

        public static void PlaceModeClicker(object sender, EventArgs e, int row, int column)
        {

        }

        public static void StartPlacement()
        {
            PaintPlacements();
            MakeLabel(shipSizes[currentShipInd]);
        }

        public static void MakeLabel(int size)
        {
            mainWindow.Note.Text = $"Поставьте {size}-палубный корабль";
        }

        public static void RemoveLabel()
        {
            mainWindow.Note.Text = string.Empty;
        }

        public static void PaintPlacements()
        {
            for (int row = 1; row <= fieldSize; row++)
            {
                for (int column = 1; column <= fieldSize; column++)
                {
                    if ((playerField[row, column] == null) && (IsPlacementGood(row, column))) playerButtons[row, column].IsEnabled = true;
                    else playerButtons[row, column].IsEnabled = false;
                }
            }
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
    }
}
