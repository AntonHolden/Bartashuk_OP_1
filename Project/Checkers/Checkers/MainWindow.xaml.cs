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

            if ((boardSize != CellsGrid.RowDefinitions.Count) || (boardSize != CellsGrid.RowDefinitions.Count)) throw new Exception("Change the number of cells!");

            InitAll(ref CellsGrid);
            Start();
        }

        /// <TODO>
        /// 1. Make a better window size;
        /// </TODO>

    }
}

