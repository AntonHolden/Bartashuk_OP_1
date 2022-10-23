using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Init;

namespace Battleship
{
    public static class Placement
    {
        public static MainWindow mainWindow = Init.mainWindow;
        public static List<int> shipSizes = new List<int>() { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

        public static void StartPlacement()
        {
            foreach (var shipSize in shipSizes)
            {
                
            }
            RemoveLabel();
        }

        public static void MakeLabel(int size)
        {
            mainWindow.Note.Text = "Поставьте "+size.ToString;
        }

        public static void RemoveLabel()
        {

        }
    }
}
