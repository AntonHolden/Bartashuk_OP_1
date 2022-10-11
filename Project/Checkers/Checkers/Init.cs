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

namespace Checkers
{
    public static class Init
    {
        public static Grid? CellsGrid;

        public static Checker[,] board = Data.board;
        static Player currentPlayer = Player.White;

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

        public static void MakeImage(ref Button button, Player player)
        {
            Image image = new Image();
            image.Source = (player == Player.White) ? new BitmapImage(new Uri("Resources/whiteChecker.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/blackChecker.png", UriKind.Relative));
            image.Width = 80;

            StackPanel stackPanel = new StackPanel();

            stackPanel.Children.Add(image);
            button.Content = stackPanel;
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
        }
        public static void InitCell(int row, int column)
        {
            Button button = new Button();
            button.Style = (Style)button.FindResource("CellStyle");

            if (((row > 2) && (row < boardSize - 3)) || (!IsCellBlack(row, column))) button.IsEnabled = false;

            else if ((IsCellBlack(row, column)) && (row >= boardSize - 3))
            {
                MakeImage(ref button, currentPlayer);
                board[row, column] = new Checker(currentPlayer, row, column);
            }
            else
            {
                MakeImage(ref button, 3 - currentPlayer);
                board[row, column] = new Checker(3 - currentPlayer, row, column);
                button.IsEnabled = false;
            }

            addButtonToGrid(ref button, row, column);

        }


    }
}
