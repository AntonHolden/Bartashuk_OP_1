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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using static Checkers.Game;
using static Checkers.Init;

namespace Checkers
{
    /// <summary>
    /// Логика взаимодействия для ClosingWindow.xaml
    /// </summary>
    public partial class ClosingWindow : Window
    {
        public ClosingWindow()
        {
            InitializeComponent();
            this.SourceInitialized += new EventHandler(BlockingTheCloseButton);
        }
        public void ClickOnNewGame(object sender, EventArgs e)
        {
            Reset();
            Start(mainWindow);
            Close();
        }

        public void ClickOnEnding(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //blocking the close button
        // --------------------

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);

        [DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
        private static extern int GetMenuItemCount(IntPtr hmenu);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

        [DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
        private static extern int DrawMenuBar(IntPtr hwnd);

        private const int MF_BYPOSITION = 0x0400;
        private const int MF_DISABLED = 0x0002;

        void BlockingTheCloseButton(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            IntPtr windowHandle = helper.Handle;
            IntPtr hmenu = GetSystemMenu(windowHandle, 0);
            int cnt = GetMenuItemCount(hmenu);
            RemoveMenu(hmenu, cnt - 1, MF_DISABLED | MF_BYPOSITION);
            RemoveMenu(hmenu, cnt - 2, MF_DISABLED | MF_BYPOSITION);
            DrawMenuBar(windowHandle);
        }
        // --------------------
    }

}
