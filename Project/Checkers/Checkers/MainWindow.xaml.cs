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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Checkers.Data;
using static Checkers.Init;
using static Checkers.Game;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            if ((Data.boardSize != CellsGrid.RowDefinitions.Count) || (Data.boardSize != CellsGrid.RowDefinitions.Count)) throw new Exception("Change the number of cells!");


            InitAll(ref CellsGrid);
            //DoSmth();
        }

        /// <TODO>
        /// 1. Make a better window size;
        /// 
        /// Buisness Logic:
        /// 1. Реализовать передвижение шашек (хоть куда-нибудь)
        /// 2. Реализовать возможность кликания только на определённые клетки
        /// 3. 
        /// 
        /// </TODO>

    }
}

