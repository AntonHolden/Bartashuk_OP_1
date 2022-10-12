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
using static Checkers.Game;

namespace Checkers
{
    public static class Data
    {
        public enum Player
        {
            White = 1,
            Black
        }

        public class Checker
        {
            public Player Player { get; init; }
            public bool IsQueen { get; private set; }
            public int row, column;
            public List<Tuple<int, int>> possibleMoves = new List<Tuple<int, int>>();
            public List<Tuple<int, int>> possibleEats = new List<Tuple<int, int>>();


            public Checker(Player player, int row, int column)
            {
                Player = player;
                IsQueen = false;
                this.row = row;
                this.column = column;

            }

            public void MakeAQueen() => IsQueen = true;

            public void UpdatePossibleMoves()
            {
                if (IsQueen)
                {
                    UpdatePossibleMovesForQueen();
                    return;
                }
                possibleEats.Clear();
                possibleMoves.Clear();
                foreach (int possibleRowDiff in new List<int> { -1, 1 })
                {
                    foreach (int possibleColumnDiff in new List<int> { -1, 1 })
                    {
                        if ((row + possibleRowDiff <= boardSize - 1) && (row + possibleRowDiff >= 0) && (column + possibleColumnDiff <= boardSize - 1) && (column + possibleColumnDiff >= 0))
                        {
                            if (board[row + possibleRowDiff, column + possibleColumnDiff] == null)
                            {
                                if ((Player == Init.currentPlayer) && (possibleRowDiff == -1)) possibleMoves.Add(new Tuple<int, int>(row + possibleRowDiff, column + possibleColumnDiff));
                                else if ((Player != Init.currentPlayer) && (possibleRowDiff == 1)) possibleMoves.Add(new Tuple<int, int>(row + possibleRowDiff, column + possibleColumnDiff));
                            }
                            else if ((board[row + possibleRowDiff, column + possibleColumnDiff].Player == 3 - Player) && (row + 2 * possibleRowDiff <= boardSize - 1) && (row + 2 * possibleRowDiff >= 0) && (column + 2 * possibleColumnDiff <= boardSize - 1) && (column + 2 * possibleColumnDiff >= 0) && (board[row + 2 * possibleRowDiff, column + 2 * possibleColumnDiff] == null)) possibleEats.Add(new Tuple<int, int>(row + 2 * possibleRowDiff, column + 2 * possibleColumnDiff));
                        }
                    }
                }
                if (possibleEats.Any()) canEat[Player] = true;
            }

            public void UpdatePossibleMovesForQueen()
            {
                possibleEats.Clear();
                possibleMoves.Clear();
                foreach (int possibleRowDiff in new List<int> { -1, 1 })
                {
                    foreach (int possibleColumnDiff in new List<int> { -1, 1 })
                    {
                        if ((row + possibleRowDiff <= boardSize - 1) && (row + possibleRowDiff >= 0) && (column + possibleColumnDiff <= boardSize - 1) && (column + possibleColumnDiff >= 0))
                        {
                            if (board[row + possibleRowDiff, column + possibleColumnDiff] == null) possibleMoves.Add(new Tuple<int, int>(row + possibleRowDiff, column + possibleColumnDiff));
                            else if ((board[row + possibleRowDiff, column + possibleColumnDiff].Player == 3 - Player) && (row + 2 * possibleRowDiff <= boardSize - 1) && (row + 2 * possibleRowDiff >= 0) && (column + 2 * possibleColumnDiff <= boardSize - 1) && (column + 2 * possibleColumnDiff >= 0) && (board[row + 2 * possibleRowDiff, column + 2 * possibleColumnDiff] == null)) possibleEats.Add(new Tuple<int, int>(row + 2 * possibleRowDiff, column + 2 * possibleColumnDiff));
                        }
                    }
                }
                if (possibleEats.Any()) canEat[Player] = true;
            }

        }

        public const int boardSize = 8;

        public static Checker[,] board = new Checker[boardSize, boardSize];

        public static Button[,] buttons = new Button[boardSize, boardSize];

        public static Button? prevButton = null;
        public static Tuple<int?, int?> prevCoord = new Tuple<int?, int?>(null, null);

        public static void PaintPossibleMoves(int row, int column)
        {
            foreach (var possibleMove in board[row, column].possibleEats.Any() ? board[row, column].possibleEats : board[row, column].possibleMoves)
            {
                buttons[possibleMove.Item1, possibleMove.Item2].IsEnabled = true;
                buttons[possibleMove.Item1, possibleMove.Item2].BorderBrush = Brushes.Red;
            }
        }

        public static void UnPaintPossibleMoves(int row, int column)
        {
            foreach (var possibleMove in board[row, column].possibleEats.Any() ? board[row, column].possibleEats : board[row, column].possibleMoves)
            {
                buttons[possibleMove.Item1, possibleMove.Item2].IsEnabled = false;
                buttons[possibleMove.Item1, possibleMove.Item2].BorderBrush = Brushes.Transparent;
            }
        }

        public static void MakeImage(ref Button button, Player player) //what about a queen?
        {
            Image image = new Image();
            image.Source = (player == Player.White) ? new BitmapImage(new Uri("Resources/whiteChecker.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/blackChecker.png", UriKind.Relative));
            image.Width = 80;

            StackPanel stackPanel = new StackPanel();

            stackPanel.Children.Add(image);
            button.Content = stackPanel;
        }
        public static void ClickOnChecker(object sender, EventArgs e, int row, int column)
        {
            if (prevButton != null)
            {
                prevButton.Background = Brushes.Transparent;
                UnPaintPossibleMoves((int)prevCoord.Item1, (int)prevCoord.Item2);
            }

            Button? pressedButton = sender as Button;


            if (pressedButton == null) throw new Exception("You clicked on a non-existent button");


            if (pressedButton == prevButton) //pressed the same button
            {
                isMoving = false;
                prevButton = null;
            }
            //pressed the first time, but player is able to eat with another checker
            else if ((canEat[currentPlayer]) && (!isMoving) && (!board[row, column].possibleEats.Any())) MessageBox.Show("Вы должны обязательно съесть шашку!");
            //pressed the second time
            else if ((isMoving) && (board[row, column] == null))
            {
                if (canEat[currentPlayer]) DeleteChecker((int)prevCoord.Item1 + ((row - (int)prevCoord.Item1) / 2), (int)prevCoord.Item2 + ((column - (int)prevCoord.Item2) / 2));

                ChangePosition((int)prevCoord.Item1, (int)prevCoord.Item2, row, column);
                prevButton.Content = null;
                MakeImage(ref pressedButton, currentPlayer);

                if (isContinue)
                {
                    pressedButton.IsEnabled = true;
                    buttons[(int)prevCoord.Item1, (int)prevCoord.Item2].IsEnabled = false;

                    PaintPossibleMoves(row, column);
                    pressedButton.Background = Brushes.Red;
                    prevButton = pressedButton;
                    prevCoord = new Tuple<int?, int?>(row, column);
                }
                else
                {
                    prevButton = null;
                    prevCoord = new Tuple<int?, int?>(null, null);
                    ExitMoveMode(row, column);
                }
            }
            else // pressed the first time or choose another checker
            {
                PaintPossibleMoves(row, column);
                isMoving = true;
                pressedButton.Background = Brushes.Red;
                prevButton = pressedButton;
                prevCoord = new Tuple<int?, int?>(row, column);
            }

        }
    }
}
