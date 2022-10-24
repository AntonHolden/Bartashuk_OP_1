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
using static Battleship.Placement;
using static Battleship.Init;

namespace Battleship
{
    public static class Data
    {
        public enum Player
        {
            Player = 1,
            Opponent
        }

        public class Ship
        {
            public int shipSize;
            public List<Tuple<int, int>> shipCoords;
            public bool isHitted = false;
            private Player player;
            public Ship(int shipSize, List<Tuple<int, int>> shipCoords, Player player)
            {
                this.shipSize = shipSize;
                this.shipCoords = shipCoords;
                this.player = player;
            }

            public void Hit()
            {
                isHitted = true;
                CheckForDefeat();
            }
            private void CheckForDefeat()
            {
                foreach (var coord in shipCoords)
                {
                    if (!field[player][coord.Item1, coord.Item2].isHitted) return;
                }
                //realize info about defeating?
                //Some kind of "ShipCount[player]--";
            }
        }

        public static UIElement? GetGridBorder(Grid grid, int row, int column)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement child = grid.Children[i];
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column && child is Border)
                    return child;
            }
            return null;
        }

        public const int fieldSize = 10;

        public static Dictionary<Player, Button[,]> buttons = new Dictionary<Player, Button[,]>()
        {
            {Player.Player, new Button[fieldSize + 1, fieldSize + 1]},
            {Player.Opponent,new Button[fieldSize + 1, fieldSize + 1] }
        };

        public static Dictionary<Player, Ship[,]> field = new Dictionary<Player, Ship[,]>()
        {
            {Player.Player, new Ship[fieldSize + 1, fieldSize + 1]},
            {Player.Opponent,new Ship[fieldSize + 1, fieldSize + 1] }
        };

        public static Player currentPlayer = Player.Player;

        static Button? prevButton = null;
        static Tuple<int?, int?> prevCoord = new Tuple<int?, int?>(null, null);

        public static void ClickOnOpponentCell(object sender, EventArgs e, int row, int column)
        {

        }

        public static Dictionary<int, int> shipsPlaced = new Dictionary<int, int>     //key - size, value - count
        {
            { 1,0 },
            { 2,0 },
            { 3,0 },
            { 4,0 }
        };

        public static void DisableEmptyCells(Player player)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++)
                {
                    buttons[player][i, j].Background = Brushes.Transparent;

                    if (field[player][i, j] != null) buttons[player][i, j].IsEnabled = true;
                    else buttons[player][i, j].IsEnabled = false;
                }
            }
        }

        public static Button? prevPlacementButton = null;
        public static void PlacementButtonsClicker(object sender, EventArgs e)
        {
            Button? pressedButton = sender as Button;
            if (pressedButton == null) throw new Exception("You clicked on a non-existent placementButton");

            if (pressedButton == prevPlacementButton)
            {
                DisableEmptyCells(Player.Player);
                prevPlacementButton = null;
                selectedShipSize = 0;
            }
            else
            {
                prevPlacementButton = pressedButton;
                selectedShipSize = placementButtonsToSize[pressedButton];
                PaintCells();
            }
            prevPlacementCoords.Clear();
        }



    }
}