using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public Checker(Player player, int row, int column)
            {
                Player = player;
                IsQueen = false;
                this.row = row;
                this.column = column;
            }

            public void MakeAQueen() => IsQueen = true;

        }

        public const int boardSize = 8;

        public static Checker[,] board = new Checker[boardSize, boardSize];
    }
}
