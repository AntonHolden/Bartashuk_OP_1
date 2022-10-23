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

        public const int boardSize = 10;

        public static Button[,] buttons = new Button[boardSize+1, boardSize+1];

        public static Button? prevButton = null;
        public static Tuple<int?, int?> prevCoord = new Tuple<int?, int?>(null, null);

        public static void ClickOnCell(object sender, EventArgs e, int row, int column)
        {
            
        }
    }
}