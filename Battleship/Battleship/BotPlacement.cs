using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Placement;
using static Battleship.Data;
using System.Drawing;
using System.Threading;
using System.Data.Common;

namespace Battleship
{
    public static class BotPlacement
    {
        enum Direction
        {
            Horizontal = 0,
            Vertical
        }

        static Random randomizer = new Random();
        static int currentSize;
        static Dictionary<Direction, List<Tuple<int, int>>> goodCellsCoord = new Dictionary<Direction, List<Tuple<int, int>>>
        {
            {Direction.Vertical, new List<Tuple<int, int>>() },
            {Direction.Horizontal, new List<Tuple<int, int>>() }
        };

        public static void StartBotPlacement()
        {
            currentSize = 4;

            while (!AllFull(Player.Opponent))
            {
                UpdateGoodCells();
                //if ((!goodCellsCoord[Direction.Horizontal].Any()) && (!goodCellsCoord[Direction.Vertical].Any())) Restart();

                Direction randomDirection = (Direction)randomizer.Next(2);
                if (!goodCellsCoord[randomDirection].Any()) randomDirection = 1 - randomDirection;

                Tuple<int, int> randomCoords = goodCellsCoord[randomDirection][randomizer.Next(goodCellsCoord[randomDirection].Count)];

                List<Tuple<int, int>> coords = new List<Tuple<int, int>>();
                if (randomDirection == Direction.Horizontal)
                {
                    for (int column = randomCoords.Item2; column < randomCoords.Item2 + currentSize; column++)
                        coords.Add(new Tuple<int, int>(randomCoords.Item1, column));
                }
                else
                {
                    for (int row = randomCoords.Item1; row < randomCoords.Item1 + currentSize; row++)
                        coords.Add(new Tuple<int, int>(row, randomCoords.Item2));
                }

                foreach (var coord in coords) field[Player.Opponent][coord.Item1, coord.Item2] = new Ship(currentSize, coords, Player.Opponent);
                shipsPlaced[Player.Opponent][currentSize]++;

                ClearGoodCells();
                if (IsFull(Player.Opponent, currentSize)) currentSize--;
            }
        }

        public static void ClearGoodCells()
        {
            goodCellsCoord[Direction.Vertical].Clear();
            goodCellsCoord[Direction.Horizontal].Clear();
        }

        public static bool IsGoodUpCell(int size, int row, int column)
        {
            for (int newRow = row; newRow < row + size; newRow++)
            {
                if (!IsPlaceGood(Player.Opponent, newRow, column)) return false;
            }
            return true;
        }

        public static bool IsGoodLeftCell(int size, int row, int column)
        {
            for (int newColumn = column; newColumn < column + size; newColumn++)
            {
                if (!IsPlaceGood(Player.Opponent, row, newColumn)) return false;
            }
            return true;
        }
        public static void UpdateGoodCells()
        {
            for (int row = 1; row <= fieldSize; row++)
            {
                for (int column = 1; column <= fieldSize; column++)
                {
                    if ((field[Player.Opponent][row, column] == null))
                    {
                        if (IsGoodUpCell(currentSize, row, column)) goodCellsCoord[Direction.Vertical].Add(new Tuple<int, int>(row, column));
                        if (IsGoodLeftCell(currentSize, row, column)) goodCellsCoord[Direction.Horizontal].Add(new Tuple<int, int>(row, column));
                    }
                }
            }
        }

    }
}
