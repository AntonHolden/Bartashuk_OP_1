using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Battleship.Data;
using static Battleship.Init;

namespace Battleship
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangeCellsColor((Brush)(new BrushConverter().ConvertFrom("#FF2F4F4F")));
            FitScreenSize();

            InitAll(this);
        }

        public static double stateFontSize = (SystemParameters.PrimaryScreenWidth * 0.8) / 36;

        void FitScreenSize()
        {
            this.Height = SystemParameters.PrimaryScreenHeight * 0.8;
            this.Width = SystemParameters.PrimaryScreenWidth * 0.8;

            var gridGapUp = this.Width * 0.11;
            var gridHorizontalGap = this.Width * 0.01;
            var placementButtonWidth = this.Width * 0.09;
            var placementButtonHeight = this.Height * 0.06;
            var placementNoteFontSize = this.Width / 78;
            var placementButtonFontSize = this.Width / 76;
            var gridWidth = this.Width * 0.47;
            var placementButtonHorizontalGap = gridWidth / 5;
            var gridHeight = this.Height * 0.73;
            var placementButtonsGap = 7;
            var fieldNoteGapUp = gridGapUp * 0.85;
            var placementButtonsGapUp = gridGapUp * 0.647;
            var placementNotesGapUp = placementButtonsGapUp * 0.78;

            PlayerGrid.Height = gridHeight;
            PlayerGrid.Width = gridWidth;
            PlayerGridBorder.Margin = new Thickness(gridHorizontalGap, gridGapUp, 0, 0);

            OpponentGrid.Height = gridHeight;
            OpponentGrid.Width = gridWidth;
            OpponentGridBorder.Margin = new Thickness(0, gridGapUp, gridHorizontalGap, 0);

            YourFieldNote.Margin = new Thickness(gridHorizontalGap, fieldNoteGapUp, 0, 0);
            OpponentFieldNote.Margin = new Thickness(0, fieldNoteGapUp, gridHorizontalGap, 0);

            PlacementButton4.Width = placementButtonWidth;
            PlacementButton4.Height = placementButtonHeight;
            PlacementButton4.Margin = new Thickness(placementButtonHorizontalGap, placementButtonsGapUp, 0, 0);

            PlacementNote4.Width = placementButtonWidth;
            PlacementNote4.Height = placementButtonHeight / 2;
            PlacementNote4.FontSize = placementNoteFontSize;
            PlacementNote4.Margin = new Thickness(placementButtonHorizontalGap, placementNotesGapUp, 0, 0);

            PlacementButton3.Width = placementButtonWidth;
            PlacementButton3.Height = placementButtonHeight;
            PlacementButton3.Margin = new Thickness(placementButtonHorizontalGap + placementButtonWidth + placementButtonsGap, placementButtonsGapUp, 0, 0);

            PlacementNote3.Width = placementButtonWidth;
            PlacementNote3.Height = placementButtonHeight / 2;
            PlacementNote3.FontSize = placementNoteFontSize;
            PlacementNote3.Margin = new Thickness(placementButtonHorizontalGap + placementButtonWidth + placementButtonsGap, placementNotesGapUp, 0, 0);

            PlacementButton2.Width = placementButtonWidth;
            PlacementButton2.Height = placementButtonHeight;
            PlacementButton2.Margin = new Thickness(placementButtonHorizontalGap + 2 * placementButtonWidth + 2 * placementButtonsGap, placementButtonsGapUp, 0, 0);

            PlacementNote2.Width = placementButtonWidth;
            PlacementNote2.Height = placementButtonHeight / 2;
            PlacementNote2.FontSize = placementNoteFontSize;
            PlacementNote2.Margin = new Thickness(placementButtonHorizontalGap + 2 * placementButtonWidth + 2 * placementButtonsGap, placementNotesGapUp, 0, 0);

            PlacementButton1.Width = placementButtonWidth;
            PlacementButton1.Height = placementButtonHeight;
            PlacementButton1.Margin = new Thickness(placementButtonHorizontalGap + 3 * placementButtonWidth + 3 * placementButtonsGap, placementButtonsGapUp, 0, 0);

            PlacementNote1.Width = placementButtonWidth;
            PlacementNote1.Height = placementButtonHeight / 2;
            PlacementNote1.FontSize = placementNoteFontSize;
            PlacementNote1.Margin = new Thickness(placementButtonHorizontalGap + 3 * placementButtonWidth + 3 * placementButtonsGap, placementNotesGapUp, 0, 0);

            PlacementButton1TextBlock.FontSize = placementButtonFontSize;
            PlacementButton2TextBlock.FontSize = placementButtonFontSize;
            PlacementButton3TextBlock.FontSize = placementButtonFontSize;
            PlacementButton4TextBlock.FontSize = placementButtonFontSize;

            var startButtonsWidth = this.Width * 0.15;
            var startButtonsHeight = this.Height * 0.1;
            var startButtonsFontSize = this.Width / 60;
            var startButtonsGapUp = this.Width * 0.009;

            StartButton.Width = startButtonsWidth;
            StartButton.Height = startButtonsHeight;
            StartButtonTextBlock.FontSize = startButtonsFontSize;
            StartButton.Margin = new Thickness(0, startButtonsGapUp, gridHorizontalGap + startButtonsWidth + 2 * placementButtonsGap, 0);

            RestartButton.Width = startButtonsWidth;
            RestartButton.Height = startButtonsHeight;
            RestartButtonTextBlock.FontSize = startButtonsFontSize;
            RestartButton.Margin = new Thickness(0, startButtonsGapUp, gridHorizontalGap, 0);

            State.Margin = new Thickness(0, startButtonsGapUp, 0, 0);
            State.FontSize = stateFontSize;

            var shipsLeftNoteGapUp = this.Height * 0.074;
            var shipsLeftNoteHorizontalGap = gridHorizontalGap * 8.3;
            var shipsLeftNoteHeight = this.Height * 0.2;
            var shipsLeftNoteWidth = this.Width * 0.2;
            var shipsLeftNoteFontSize = this.Width / 94;

            PlayerShipsLeftNote.Height = shipsLeftNoteHeight;
            PlayerShipsLeftNote.Width = shipsLeftNoteWidth;
            PlayerShipsLeftNote.Margin = new Thickness(shipsLeftNoteHorizontalGap, shipsLeftNoteGapUp, 0, 0);
            PlayerShipsLeftNote.FontSize = shipsLeftNoteFontSize;

            OpponentShipsLeftNote.Height = shipsLeftNoteHeight;
            OpponentShipsLeftNote.Width = shipsLeftNoteWidth;
            OpponentShipsLeftNote.FontSize = shipsLeftNoteFontSize;
            OpponentShipsLeftNote.Margin = new Thickness(0, shipsLeftNoteGapUp, shipsLeftNoteHorizontalGap, 0);
        }

        void ChangeCellsColor(Brush color)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++)
                {
                    Border? border = (Border?)GetGridBorder(PlayerGrid, i, j);
                    if (border != null) border.BorderBrush = color;

                    border = (Border?)GetGridBorder(OpponentGrid, i, j);
                    if (border != null) border.BorderBrush = color;
                }
            }
        }
    }
}
