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

namespace Battleship
{
    public static class Data
    {
        public enum Player
        {
            Player = 1,
            Opponent
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

        public const int fieldSize = 10;

        public static Button[,] playerButtons = new Button[fieldSize + 1, fieldSize + 1];
        public static Button[,] opponentButtons = new Button[fieldSize + 1, fieldSize + 1];

        public static Button? prevButton = null;
        public static Tuple<int?, int?> prevCoord = new Tuple<int?, int?>(null, null);

        public static void ClickOnOpponentCell(object sender, EventArgs e, int row, int column)
        {
            
        }

        public static void PlaceModeClicker(object sender, EventArgs e, int row, int column)
        {

        }
    }
}