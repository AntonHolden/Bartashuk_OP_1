using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static Checkers.Data;
using static Checkers.Init;

namespace Checkers
{
    public static class Game
    {
        static UIElement GetGridElement(Grid g, int r, int c)
        {
            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c && e is Button)
                    return e;
            }
            return null;
        }

        static Grid CellsGrid = Init.CellsGrid!;

        static Checker[,] board = Init.board;

        static int whiteCheckersLeft = 12;
        static int blackCheckersLeft = 12;

        public static void DoSmth()
        {
            var smth = GetGridElement(CellsGrid, 3, 3);
            smth.IsEnabled = true;
        }

        
    }
}
