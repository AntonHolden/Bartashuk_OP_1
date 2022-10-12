using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Checkers.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Windows.Media;


namespace Checkers
{
    public static class Init
    {
        public static Grid? CellsGrid;

        public static Checker[,] board = Data.board;
        public static Player currentPlayer = Player.White;
        public static Button[,] buttons = Data.buttons;

            
        public static void InitAll(ref Grid CellsGridMain)
        {
            CellsGrid = CellsGridMain;
            InitCells();
        }

        public static void InitCells()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++) InitCell(i, j);
            }
        }

        public static bool IsCellBlack(int row, int column)
        {
            return ((row + column) % 2 != 0);
        }

        public static void addButtonToGrid(ref Button button, int row, int column)
        {
            Grid.SetColumn(button, column);
            Grid.SetRow(button, row);

            if (CellsGrid == null) throw new Exception("CellsGrid==null");

            CellsGrid.Children.Add(button);

            buttons[row, column] = button;
        }
        public static void InitCell(int row, int column)
        {
            Button button = new Button();
            button.Style = (Style)button.FindResource("CellStyle");
            button.Click += new RoutedEventHandler((sender, e) => ClickOnChecker(sender, e, row, column));

            if (((row > 2) && (row < boardSize - 3)) || (!IsCellBlack(row, column))) button.IsEnabled = false;

            else if ((IsCellBlack(row, column)) && (row >= boardSize - 3))
            {
                MakeImage(ref button, currentPlayer,false);
                board[row, column] = new Checker(currentPlayer, row, column);
                if (currentPlayer != Player.White) button.IsEnabled = false;
            }
            else
            {
                MakeImage(ref button, 3 - currentPlayer,false);
                board[row, column] = new Checker(3 - currentPlayer, row, column);
                if (currentPlayer == Player.White) button.IsEnabled = false;
            }

            addButtonToGrid(ref button, row, column);

        }
    }
}
