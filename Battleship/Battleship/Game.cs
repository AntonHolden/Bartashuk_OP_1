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
using static Battleship.Data;
using static Battleship.Init;
using static Battleship.BotPlacement;
using System.Data.Common;
using System.Threading;

namespace Battleship
{
    public static class Game
    {
        public static Dictionary<Player, Button[,]> buttons = Placement.buttons;
        public static List<Tuple<int, int>> possibleCoords = new List<Tuple<int, int>>();
        public static List<Tuple<int, int>> hittedCoords = new List<Tuple<int, int>>();
        //TODO:
        //1. Fix async delay (new list)
        //2. Fix State.Width
        //3. Make smarter botmoves

        public static void StartGame()
        {
            mainWindow.StartButton.Visibility = Visibility.Hidden;
            foreach (var placementButton in placementButtonsToSize) placementButton.Key.Visibility = Visibility.Hidden;
            foreach (var placementNote in sizeToPlacementNotes) placementNote.Value.Visibility = Visibility.Hidden;
            mainWindow.PlayerShipsLeftNote.Visibility = Visibility.Visible;
            mainWindow.OpponentShipsLeftNote.Visibility = Visibility.Visible;

            FillPossibleCoords();

            DisableAllButtons(Player.Player);
            EnableAllButtons(Player.Opponent);
            UpdateShipsLeftNotes();
        }

        public static void FillPossibleCoords()
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++) possibleCoords.Add(new Tuple<int, int>(i, j));
            }
        }

        public static void UpdateShipsLeftNotes()
        {
            string text = "У вас осталось:";
            foreach (var shipSize in shipsPlaced[Player.Player]) text += $"\n{shipSize.Key}-палубных кораблей: {shipSize.Value}";
            mainWindow.PlayerShipsLeftNote.Text = text;

            text = "У противника осталось:";
            foreach (var shipSize in shipsPlaced[Player.Opponent]) text += $"\n{shipSize.Key}-палубных кораблей: {shipSize.Value}";
            mainWindow.OpponentShipsLeftNote.Text = text;
        }

        public static bool IsPlayerWon(Player player)
        {
            foreach (var ship in shipsPlaced[3 - player])
            {
                if (ship.Value != 0) return false;
            }
            return true;
        }

        async public static void BotMove()
        {
            await Task.Delay(1);
            Thread.Sleep(999);

            if (hittedCoords.Count != 0) LookForPlayerShip();
            else
            {
                var coord = possibleCoords[randomizer.Next(possibleCoords.Count)];
                possibleCoords.Remove(coord);

                if (field[Player.Player][coord.Item1, coord.Item2] == null)
                {
                    MakeMissImage(Player.Player, coord.Item1, coord.Item2);
                    mainWindow.State.Foreground = Brushes.Blue;
                    mainWindow.State.Text = $"Ход:{columnToLetter[coord.Item2]}{coord.Item1}\nПротивник промахнулся!";
                }
                else
                {
                    MakeHitImage(Player.Player, coord.Item1, coord.Item2);
                    field[Player.Player][coord.Item1, coord.Item2].Hit(coord.Item1, coord.Item2);
                }
            }

            if (IsPlayerWon(Player.Opponent)) End(false);
        }

        public static void LookForPlayerShip()
        {
            if (hittedCoords.Count > 1) LookForPlayerShipNext();
            else
            {
                
            }
        }

        public static void LookForPlayerShipNext()
        {

        }

        public static void End(bool isPlayerWon)
        {
            if (isPlayerWon)
            {
                mainWindow.State.Foreground = Brushes.Green;
                mainWindow.State.FontSize = 42;
                mainWindow.State.Text = "Вы победили!";
            }
            else
            {
                mainWindow.State.Foreground = Brushes.Red;
                mainWindow.State.FontSize = 42;
                mainWindow.State.Text = "Вы проиграли!";
            }
            DisableAllButtons(Player.Opponent);
        }
    }
}
