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
            public Dictionary<Tuple<int, int>, Tuple<int, int>> possibleEats = new Dictionary<Tuple<int, int>, Tuple<int, int>>(); //key - empty cell, value - checker to eat


            public Checker(Player player, int row, int column)
            {
                Player = player;
                IsQueen = false;
                this.row = row;
                this.column = column;

            }

            public void UpdateChecker()
            {
                CheckForQueen();
                UpdatePossibleMoves();
            }

            public void CheckForQueen()
            {
                if ((!IsQueen) && (((Init.currentPlayer == Player) && (row == 0)) || ((Init.currentPlayer != Player) && (row == boardSize - 1)))) IsQueen = true;
            }

            public void UpdatePossibleMoves()
            {
                if (IsQueen)
                {
                    UpdatePossibleMovesForQueen();
                    return;
                }
                possibleEats.Clear();
                possibleMoves.Clear();
                foreach (int rowDiff in new List<int> { -1, 1 })
                {
                    foreach (int columnDiff in new List<int> { -1, 1 })
                    {
                        if ((row + rowDiff <= boardSize - 1) &&
                            (row + rowDiff >= 0) &&
                            (column + columnDiff <= boardSize - 1) &&
                            (column + columnDiff >= 0))
                        {
                            if (board[row + rowDiff, column + columnDiff] == null)
                            {
                                if ((Player == Init.currentPlayer) && (rowDiff == -1)) possibleMoves.Add(new Tuple<int, int>(row + rowDiff, column + columnDiff));
                                else if ((Player != Init.currentPlayer) && (rowDiff == 1)) possibleMoves.Add(new Tuple<int, int>(row + rowDiff, column + columnDiff));
                            }
                            else if ((board[row + rowDiff, column + columnDiff].Player == 3 - Player) &&
                                (row + 2 * rowDiff <= boardSize - 1) &&
                                (row + 2 * rowDiff >= 0) &&
                                (column + 2 * columnDiff <= boardSize - 1) &&
                                (column + 2 * columnDiff >= 0) &&
                                (board[row + 2 * rowDiff, column + 2 * columnDiff] == null))
                                possibleEats[new Tuple<int, int>(row + 2 * rowDiff, column + 2 * columnDiff)] = new Tuple<int, int>(row + rowDiff, column + columnDiff);
                        }
                    }
                }
                if (possibleEats.Any()) canEat[Player] = true;
                if (possibleMoves.Any() || possibleEats.Any()) canMove[Player] = true;
            }

            public void UpdatePossibleMovesForQueen()
            {
                possibleEats.Clear();
                possibleMoves.Clear();

                foreach (int rowDiff in new List<int> { -1, 1 })
                {
                    foreach (int columnDiff in new List<int> { -1, 1 })
                    {
                        for (int diff = 1; diff < boardSize; diff++)
                        {
                            int currentRow = row + (diff * rowDiff);
                            int currentColumn = column + (diff * columnDiff);

                            if ((currentRow <= boardSize - 1) &&
                                (currentRow >= 0) &&
                                (currentColumn <= boardSize - 1) &&
                                (currentColumn >= 0))
                            {
                                if (board[currentRow, currentColumn] == null) possibleMoves.Add(new Tuple<int, int>(currentRow, currentColumn));
                                else if (board[currentRow, currentColumn].Player == 3 - Player)
                                {
                                    for (int mult = 1; mult < boardSize; mult++)
                                    {
                                        if ((currentRow + (mult * rowDiff) <= boardSize - 1) &&
                                            (currentRow + (mult * rowDiff) >= 0) &&
                                            (currentColumn + (mult * columnDiff) <= boardSize - 1) &&
                                            (currentColumn + (mult * columnDiff) >= 0) &&
                                            (board[currentRow + (mult * rowDiff), currentColumn + (mult * columnDiff)] == null))
                                            possibleEats[new Tuple<int, int>(currentRow + (mult * rowDiff), currentColumn + (mult * columnDiff))] = new Tuple<int, int>(currentRow, currentColumn);
                                        else break;
                                    }
                                    break;
                                }
                                else break;
                            }
                            else break;
                        }
                    }
                }
                if (possibleEats.Any()) canEat[Player] = true;
                if (possibleMoves.Any() || possibleEats.Any()) canMove[Player] = true;
            }
        }

        public const int boardSize = 8;

        public static Checker[,] board = new Checker[boardSize, boardSize];

        public static Button[,] buttons = new Button[boardSize, boardSize];

        public static Button? prevButton = null;
        public static Tuple<int?, int?> prevCoord = new Tuple<int?, int?>(null, null);

        public static void PaintPossibleMoves(int row, int column)
        {
            if (board[row, column].possibleEats.Any())
            {
                foreach (var possibleMove in board[row, column].possibleEats)
                {
                    buttons[possibleMove.Key.Item1, possibleMove.Key.Item2].IsEnabled = true;
                    buttons[possibleMove.Key.Item1, possibleMove.Key.Item2].BorderBrush = Brushes.Red;
                }
            }
            else
            {
                foreach (var possibleMove in board[row, column].possibleMoves)
                {
                    buttons[possibleMove.Item1, possibleMove.Item2].IsEnabled = true;
                    buttons[possibleMove.Item1, possibleMove.Item2].BorderBrush = Brushes.Red;
                }
            }
        }

        public static void UnPaintPossibleMoves(int row, int column)
        {
            if (board[row, column].possibleEats.Any())
            {
                foreach (var possibleMove in board[row, column].possibleEats)
                {
                    buttons[possibleMove.Key.Item1, possibleMove.Key.Item2].IsEnabled = false;
                    buttons[possibleMove.Key.Item1, possibleMove.Key.Item2].BorderBrush = Brushes.Transparent;
                }
            }
            else
            {
                foreach (var possibleMove in board[row, column].possibleMoves)
                {
                    buttons[possibleMove.Item1, possibleMove.Item2].IsEnabled = false;
                    buttons[possibleMove.Item1, possibleMove.Item2].BorderBrush = Brushes.Transparent;
                }
            }
        }

        public static void MakeImage(ref Button button, Player player, bool isQueen)
        {
            Image image = new Image();
            if (!isQueen) image.Source = (player == Player.White) ? new BitmapImage(new Uri("Resources/whiteChecker.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/blackChecker.png", UriKind.Relative));
            else image.Source = (player == Player.White) ? new BitmapImage(new Uri("Resources/whiteCheckerQueen.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/blackCheckerQueen.png", UriKind.Relative));
            image.Width = 80;

            StackPanel stackPanel = new StackPanel();

            stackPanel.Children.Add(image);
            button.Content = stackPanel;
        }

        public static void ClickOnChecker(object sender, EventArgs e, int row, int column)
        {
            End();
            if ((isContinue) && (board[row, column] != null))
            {
                MessageBox.Show("Вы должны обязательно съесть шашку!");
                return;
            }

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
                if (canEat[currentPlayer]) DeleteChecker(board[(int)prevCoord.Item1, (int)prevCoord.Item2].possibleEats[new Tuple<int, int>(row, column)].Item1, board[(int)prevCoord.Item1, (int)prevCoord.Item2].possibleEats[new Tuple<int, int>(row, column)].Item2);
                ChangePosition((int)prevCoord.Item1, (int)prevCoord.Item2, row, column);
                prevButton.Content = null;
                MakeImage(ref pressedButton, currentPlayer, board[row, column].IsQueen);

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
