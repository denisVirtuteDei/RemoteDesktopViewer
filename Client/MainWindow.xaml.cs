using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
//using System.Windows.Forms;
using System.Diagnostics;

namespace NotTeamViewer.Client
{
    /// <summary>
    /// Логика взаимодействия для Client MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpClient_d tcp;

        public delegate void KeyClicks(Key key, bool up, bool down);
        public event KeyClicks KeyClickNotify;
        public delegate void MouseClicks(MouseButtonState l, MouseButtonState r);
        public event MouseClicks MouseClickNotify;
        public delegate void MouseMoves(int x, int y);
        public event MouseMoves MouseMoveNotify;
        public delegate void MouseWheelMove(int delta);
        public event MouseWheelMove MouseWheelNotify;

        /// <summary>
        /// Constructor client <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyClick;
            this.KeyUp += MainWindow_KeyClick;
            
            tcp = new TcpClient_d(this);
        }


        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            var loc = e.GetPosition(ImagePanel);
            MouseMoveNotify((int)loc.X, (int)loc.Y);
        }

        private void Image_MouseClick(object sender, MouseButtonEventArgs e)
        {
            MouseClickNotify(e.LeftButton, e.RightButton);
        }

        private void MainWindow_KeyClick(object sender, KeyEventArgs e)
        {
            KeyClickNotify(e.Key, e.IsUp, e.IsDown);
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheelNotify(e.Delta);
        }


        /// <summary>
        /// Start button click event.
        /// </summary>
        private async void TCP_Start(object sender, RoutedEventArgs e)
        {
            if (!tcp.GetinProc() && tcp.SetIP(ipAddress.Text))
            {
                await Task.Run(() => tcp.Run());
            }
        }

        /// <summary>
        /// Stop button click event.
        /// </summary>
        private void Stop_But_Click(object sender, RoutedEventArgs e)
        {
            tcp.SetinProc(false);
        }

    }

}
