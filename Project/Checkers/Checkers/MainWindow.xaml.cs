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
    public partial class MainWindow : Window
    {
        const int boardSize = 8;

        int[,] board = new int[boardSize, boardSize];
        public MainWindow()
        {
            InitializeComponent();

            if ((boardSize != CellsGrid.RowDefinitions.Count) || (boardSize != CellsGrid.RowDefinitions.Count)) throw new Exception("Change the number of cells!");
            InitBoard();



            InitButtons();
        }

        /// <TODO>
        /// 1. Make a better window size;
        /// 2. Take care of MouseOverColor
        /// 
        /// </TODO>


        public void InitBoard()
        {
            board = new int[boardSize, boardSize]
            {
                {2,0,2,0,2,0,2,0 },
                {0,2,0,2,0,2,0,2 },
                {2,0,2,0,2,0,2,0 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {0,1,0,1,0,1,0,1 },
                {1,0,1,0,1,0,1,0 },
                {0,1,0,1,0,1,0,1 },

            };
        }
        public void InitButtons()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    InitChecker(i, j);
                }
            }
        }

        public void InitChecker(int row, int column)
        {
            Button button = new Button();
            button.Background = Brushes.Transparent;

            if (board[row, column] == 1)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri("Resources/whiteChecker.png", UriKind.Relative));
                image.Width = 80;

                StackPanel stackPanel = new StackPanel();


                //stackPanel.Orientation = Orientation.Horizontal;
                //stackPanel.Margin = new Thickness(10);
                stackPanel.Children.Add(image);
                button.Content = stackPanel;
                button.Click += Button_White;
            }
            else if (board[row, column] == 2)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri("Resources/blackChecker.png", UriKind.Relative));
                image.Width = 80;

                StackPanel stackPanel = new StackPanel();


                //stackPanel.Orientation = Orientation.Horizontal;
                //stackPanel.Margin = new Thickness(10);
                stackPanel.Children.Add(image);
                button.Content = stackPanel;
                button.Click += Button_Black;
            }
            else
            {
                button.Click += Button_Click;

            }

            addButtonToGrid(button, row, column);

        }

        public void addButtonToGrid(Button button, int row, int column)
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

