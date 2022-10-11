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
        static UIElement? GetGridButton(Grid grid, int row, int column)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement child = grid.Children[i];
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column && child is Button)
                    return child;
            }
            return null;
        }

        static Grid CellsGrid = Init.CellsGrid!;
        static Player currentPlayer = Init.currentPlayer;
        static Checker[,] board = Init.board;

        static int whiteCheckersLeft = 12;
        static int blackCheckersLeft = 12;

        
        
        
        
        public static void ChangePlayer()
        {
            currentPlayer = 3 - currentPlayer;
        }



        public static void DoSmth()
        {
            int row = 3;
            int column = 3;
            Button? smth = (Button?)GetGridButton(CellsGrid, row, column);

            if (smth == null) throw new Exception($"There is no button in grid in {row} row in {column} column!");

            smth.IsEnabled = true;
        }

        
    }
}
