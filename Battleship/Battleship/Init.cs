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
using static Battleship.BotPlacement;

namespace Battleship
{
    public static class Init
    {
        public static Dictionary<Player, Button[,]> buttons = Data.buttons;
        public static MainWindow mainWindow;
        public static Dictionary<Player, Grid> grids;
        public static Dictionary<Button, int> placementButtonsToSize;
        public static Dictionary<int, TextBlock> sizeToPlacementNotes;

        public static void InitAll(MainWindow mainWindow)
        {
            if (mainWindow == null) throw new Exception("MainWindow is null!");

            Init.mainWindow = mainWindow;

            grids = new Dictionary<Player, Grid>()
            {
                {Player.Player, mainWindow.PlayerGrid},
                {Player.Opponent, mainWindow.OpponentGrid}
            };
            placementButtonsToSize = new Dictionary<Button, int>()
            {
                {mainWindow.PlacementButton1, 1 },
                {mainWindow.PlacementButton2, 2 },
                {mainWindow.PlacementButton3, 3 },
                {mainWindow.PlacementButton4, 4 }
            };
            sizeToPlacementNotes = new Dictionary<int, TextBlock>()
            {
                {1, mainWindow.PlacementNote1 },
                {2, mainWindow.PlacementNote2 },
                {3, mainWindow.PlacementNote3 },
                {4, mainWindow.PlacementNote4 }
            };

            InitCells();
            InitMainWindowButtons();
            InitColumnToLetter();
            StartBotPlacement();
            StartPlacement();
        }

        public static void InitMainWindowButtons()
        {
            foreach (var placementButton in placementButtonsToSize) placementButton.Key.Click += new RoutedEventHandler(PlacementButtonsClicker);

            mainWindow.StartButton.Click += new RoutedEventHandler(StartButtonClicker);
            mainWindow.RestartButton.Click += new RoutedEventHandler(RestartButtonClicker);
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
            button.IsEnabled = false;

            Grid.SetColumn(button, column);
            Grid.SetRow(button, row);

            return button;
        }
        public static void InitButton(int row, int column)
        {
            Button button = MakeButton(row, column);
            button.Click += new RoutedEventHandler((sender, e) => PlaceModeClicker(sender, e, row, column));
            
            grids[Player.Player].Children.Add(button);
            buttons[Player.Player][row, column] = button;

            button = MakeButton(row, column);
            button.Click += new RoutedEventHandler((sender, e) => ClickOnOpponentCell(sender, e, row, column));

            grids[Player.Opponent].Children.Add(button);
            buttons[Player.Opponent][row, column] = button;
        }
    }
}
