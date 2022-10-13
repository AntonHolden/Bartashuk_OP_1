using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static Checkers.Data;
using static Checkers.Init;

namespace Checkers
{
    public static class Game
    {

        static Grid CellsGrid = Init.CellsGrid!;
        public static Player currentPlayer = Init.currentPlayer;
        static Checker[,] board = Init.board;
        static Button[,] buttons = Init.buttons;
        public static bool isMoving = false;
        public static bool isContinue = false;
        public static bool isEating = false;
        static MainWindow mainWindow;

        public static Dictionary<Player, bool> canEat = new Dictionary<Player, bool>()
        {
            { Player.White, false },
            { Player.Black, false }
        };

        public static Dictionary<Player, bool> canMove = new Dictionary<Player, bool>()
        {
            { Player.White, false },
            { Player.Black, false }
        };

        public static Dictionary<Player, int> checkersLeft = new Dictionary<Player, int>()
        {
            { Player.White, 12 },
            { Player.Black, 12 }
        };


        public static void Start(MainWindow window)
        {
            mainWindow = window;
            UpdateAllMoves();
        }


        public static void ChangePlayer()
        {
            currentPlayer = 3 - currentPlayer;
        }


        public static void UpdateAllMoves()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] != null) board[i, j].UpdatePossibleMoves();
                }
            }
        }

        public static void DeleteChecker(int row, int column)
        {
            checkersLeft[board[row, column].Player]--;
            board[row, column] = null;
            buttons[row, column].Content = null;
            isContinue = false;
            canEat[currentPlayer] = false;
            isEating = true;
        }

        public static void ChangePosition(int prevRow, int prevColumn, int newRow, int newColumn)
        {

            board[newRow, newColumn] = board[prevRow, prevColumn];
            board[prevRow, prevColumn] = null;
            board[newRow, newColumn].row = newRow;
            board[newRow, newColumn].column = newColumn;
            board[newRow, newColumn].UpdateChecker();
            if ((canEat[currentPlayer]) && (isEating)) isContinue = true;
        }

        public static void ExitMoveMode(int row, int column)
        {
            isMoving = false;
            isContinue = false;
            isEating = false;
            canEat[Player.White] = false;
            canEat[Player.Black] = false;
            canMove[Player.White] = false;
            canMove[Player.Black] = false;

            ChangePlayer();
            ResetButtons();
            UpdateAllMoves();

            if (canMove[currentPlayer]==false) End();
        }

        public static void End()
        {
            ClosingWindow closingWindow = new ClosingWindow();
            
            closingWindow.Result.Text += "Ничья!";
            closingWindow.ShowDialog();
        }

        public static void ResetButtons()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if ((board[i, j] != null) && (board[i, j].Player == currentPlayer)) buttons[i, j].IsEnabled = true;
                    else buttons[i, j].IsEnabled = false;
                }
            }
        }
        public static void DisableAllButtons()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++) buttons[i, j].IsEnabled = false;
            }
        }

        public static void EnableButtons()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if ((board[i, j] != null) && (board[i, j].Player == currentPlayer)) buttons[i, j].IsEnabled = true;
                }
            }
        }

    }
}

