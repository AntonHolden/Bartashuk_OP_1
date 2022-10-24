using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Battleship.Data;
using static Battleship.Placement;

namespace Battleship
{
    public static class Init
    {
        public static Grid playerGrid;
        public static Grid opponentGrid;
        public static Dictionary<Player, Button[,]> buttons = Data.buttons;
        public static MainWindow mainWindow;


        public static void InitAll(MainWindow mainWindow)
        {
            if (mainWindow==null) throw new Exception("MainWindow is null!");

            Init.mainWindow = mainWindow;
            playerGrid = mainWindow.PlayerGrid;
            opponentGrid = mainWindow.OpponentGrid;
            InitCells();
            StartPlacement();
        }

        public static void InitCells()
        {
            for (int row = 1; row <= fieldSize; row++)
            {
                for (int column = 1; column <= fieldSize; column++)
                {
                    InitButton(row, column);
                }
            }
        }

        public static Button MakeButton(int row, int column)
        {
            Button button = new Button();
            button.Style = (Style)button.FindResource("CellStyle");

            Grid.SetColumn(button, column);
            Grid.SetRow(button, row);

            return button;
        }
        public static void InitButton(int row, int column)
        {
            Button button = MakeButton(row, column);
            button.Click += new RoutedEventHandler((sender, e) => PlaceModeClicker(sender, e, row, column));

            playerGrid.Children.Add(button);
            buttons[Player.Player][row, column] = button;

            button = MakeButton(row, column);
            button.Click += new RoutedEventHandler((sender, e) => ClickOnOpponentCell(sender, e, row, column));
            button.IsEnabled = false;

            opponentGrid.Children.Add(button);
            buttons[Player.Opponent][row, column] = button;
        }

        public static void EnableOpponentGrids()
        {

        }

    }
}
