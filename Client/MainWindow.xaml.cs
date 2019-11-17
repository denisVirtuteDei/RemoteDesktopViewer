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

        public delegate void MouseMoves(object sender, MouseEventArgs e);
        public event MouseMoves MouseMoveNotify;
        public delegate void MouseClicks(object sender, MouseButtonEventArgs e);
        public event MouseClicks MouseClickNotify;
        public delegate void KeyClicks(object sender, KeyEventArgs e);
        public event KeyClicks KeyClickNotify;

        /// <summary>
        /// Constructor client <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
            this.MouseMove += MainWindow_MouseMove;
            this.MouseDown += MainWindow_MouseDown;

            tcp = new TcpClient_d(this);
        }
        

        private async void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            await Task.Run(() => MouseMoveNotify(sender, e));
        }

        private async void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Run(() => MouseClickNotify(sender, e));
        }

        private async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            await Task.Run(() => KeyClickNotify(sender, e));
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
