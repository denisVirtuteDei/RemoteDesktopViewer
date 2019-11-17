using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System;

namespace NotTeamViewer.Server
{
    /// <summary>
    /// Логика взаимодействия для server MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpServer tcp;
        private readonly int width = 1920;
        private readonly int height = 1080;


        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpServer(this);
            tcp.MouseMoveNotify += Tcp_MouseMoveNotify;
        }

        private void Tcp_MouseMoveNotify(string str)
        {
            var loc = str.Split(' ');
            int left = loc[2] == "1" ? 1 : 0;
            double w = Convert.ToDouble(loc[0]) * width;
            double h = Convert.ToDouble(loc[1]) * height;
            SetCursorPos((int)w, (int)h);
            //var ev = new MouseEventArgs(MouseButtons.Left, left, 630, 630, 0);
        }

        private async void StartServer()
        {          
            if (!tcp.GetinProc())
            {
                tcp.SetinProc(true);
                await Task.Run(() => tcp.Run());
            }
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Value changed");
        }

        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
            if(tcp.GetinProc())
            {
                tcp.SetinProc(false);
                
                StartStopItem.Header = "Start";
            }
            else
            {
                StartStopItem.Header = "Stop";
                StartServer();
            }
        }


        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);
    }
}
