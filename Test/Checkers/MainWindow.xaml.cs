using System;
using System.Collections.Generic;
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

            InitAll(this);


            ChangeCellsColor((Brush)(new BrushConverter().ConvertFrom("#FF2F4F4F")));
        }
        //--------------
        // TODO:
        // 1.Make
        //--------------
        void ChangeCellsColor(Brush color)
        {
            for (int i = 1; i <= fieldSize; i++)
            {
                for (int j = 1; j <= fieldSize; j++)
                {
                    Border? border = (Border?)GetGridBorder(PlayerGrid, i, j);
                    if (border != null) border.BorderBrush=color;

                    border = (Border?)GetGridBorder(OpponentGrid, i, j);
                    if (border != null) border.BorderBrush = color;
                }
            }
        }
    }
}
