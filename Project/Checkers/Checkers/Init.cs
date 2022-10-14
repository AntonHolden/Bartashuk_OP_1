using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Checkers.Data;
using static Checkers.Game;
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
        public static Grid cellsGrid;
        public static Checker[,] board = Data.board;
        public static Player currentPlayer = Player.White;
        public static Button[,] buttons = Data.buttons;


        public static void InitAll(ref Grid cellsGridMain)
        {
            if (cellsGridMain == null) throw new Exception("ellsGridMain == null");

            cellsGrid = cellsGridMain;
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

            cellsGrid.Children.Add(button);

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
                MakeImage(ref button, currentPlayer, false);
                board[row, column] = new Checker(currentPlayer, row, column);
                if (currentPlayer != Player.White) button.IsEnabled = false;
            }
            else
            {
                MakeImage(ref button, 3 - currentPlayer, false);
                board[row, column] = new Checker(3 - currentPlayer, row, column);
                if (currentPlayer == Player.White) button.IsEnabled = false;
            }

            addButtonToGrid(ref button, row, column);

        }

        //public static void ClearButtons()
        //{
        //    for (int i=0;i<boardSize; i++)
        //    {
        //        for (int j=0;j<boardSize; j++)
        //        {
        //            buttons[i, j].IsEnabled = false;
        //            buttons[i, j].BorderBrush = Brushes.Transparent;
        //            buttons[i, j].Background = Brushes.Transparent;
        //        }
        //    }
        //}
        public static void Reset()
        {
            cellsGrid.Children.OfType<Button>().ToList().ForEach(b => cellsGrid.Children.Remove(b));

            board = Data.board;
            currentPlayer = Player.White;
            buttons = Data.buttons;
            InitCells();


            Game.currentPlayer = currentPlayer;
            Game.board = board;
            Game.buttons = buttons;
            isMoving = false;
            isContinue = false;
            isEating = false;

            canEat = new Dictionary<Player, bool>()
            {
                { Player.White, false },
                { Player.Black, false }
            };

            canMove = new Dictionary<Player, bool>()
            {
                { Player.White, false },
                { Player.Black, false }
            };

            checkersLeft = new Dictionary<Player, int>()
            {
                { Player.White, 12 },
                { Player.Black, 12 }
            };
        }
    }
}
