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

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    

    public partial class MainWindow : Window
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

        const int boardSize = 8;

        Checker[,] board = new Checker[boardSize, boardSize];

        Player currentPlayer = Player.White;

        int whiteCheckersLeft = 12;
        int blackCheckersLeft = 12;

        public MainWindow()
        {
            InitializeComponent();

            if ((boardSize != CellsGrid.RowDefinitions.Count) || (boardSize != CellsGrid.RowDefinitions.Count)) throw new Exception("Change the number of cells!");

            InitCells();
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


        public void InitCells()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++) InitCell(i, j);
            }
        }


        public void MakeImage(ref Button button, Player player)
        {
            Image image = new Image();
            image.Source = (player == Player.White) ? new BitmapImage(new Uri("Resources/whiteChecker.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/blackChecker.png", UriKind.Relative));
            image.Width = 80;

            StackPanel stackPanel = new StackPanel();

            stackPanel.Children.Add(image);
            button.Content = stackPanel;
        }

        public bool IsCellBlack(int row, int column)
        {
            return ((row + column) % 2 != 0);
        }
        public void InitCell(int row, int column)
        {
            Button button = new Button();
            button.Style = (Style)button.FindResource("CellStyle");

            if (((row > 2) && (row < boardSize - 3)) || (!IsCellBlack(row, column))) button.IsEnabled = false;

            else if ((IsCellBlack(row,column))&&(row>=boardSize-3))
            {
                MakeImage(ref button, currentPlayer);
                board[row, column] = new Checker(currentPlayer, row, column);
            }
            else
            {
                MakeImage(ref button, 3-currentPlayer);
                board[row, column] = new Checker(3-currentPlayer, row, column);
                button.IsEnabled = false;
            }

            addButtonToGrid(ref button, row, column);

        }

        public void addButtonToGrid(ref Button button, int row, int column)
        {
            Grid.SetColumn(button, column);
            Grid.SetRow(button, row);
            CellsGrid.Children.Add(button);
        }





        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Кнопка нажата");
        }
        private void Button_White(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Белая нажата");
        }
        private void Button_Black(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Чёрная нажата");
        }
    }
}

