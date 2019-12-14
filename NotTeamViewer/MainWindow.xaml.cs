using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace NotTeamViewer.Server
{
    /// <summary>
    /// Логика взаимодействия для server MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpServer tcp;
        private bool back = false;

        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpServer(this);

            String host = System.Net.Dns.GetHostName();
            // Получение ip-адреса.
            System.Net.IPAddress ip = System.Net.Dns.GetHostEntry(host).AddressList[3];
            // Показ адреса в label'е.
            IDBox.Text = ip.ToString();

            ChangeColor_Click(null, null);
        }

        private async void StartServer()
        {
            if (!tcp.GetinProc())
            {
                await Task.Run(() => tcp.Run());
                StartStopItem.Header = "Start";
            }
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            if (back)
            {
                background.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDEAA8"));
                Menu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC7722"));
                P1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#808080"));
                P2.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#808080"));
                checkBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                PasswordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDEAA8"));
                IDBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDEAA8"));
                back = !back;
            }
            else
            {
                background.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBF7F7"));
                Menu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9D00F"));
                P1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0AE2C"));
                P2.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0AE2C"));
                checkBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9D00F"));
                PasswordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBF7F7"));
                IDBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBF7F7"));
                back = !back;
            }
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
            if (tcp.GetinProc())
            {
                if (!tcp.GetEnd())
                {
                    using (TcpClient client = new TcpClient())
                        client.Connect("127.0.0.1", 5001);
                }
                tcp.SetinProc(false);
                StartStopItem.Header = "Start";
            }
            else
            {
                StartStopItem.Header = "Stop";
                StartServer();
            }
        }
    }
}
