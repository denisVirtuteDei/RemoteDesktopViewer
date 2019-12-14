using System;
<<<<<<< HEAD
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
=======
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

namespace NotTeamViewer.Server
{
    /// <summary>
    /// Логика взаимодействия для server MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
<<<<<<< HEAD
        private TcpServer tcp;
        private bool back = false;
=======
        private readonly TcpServer tcp;
        private int width = 1920;
        private int height = 1080;
        private MouseFlags prevL = MouseFlags.Absolute,
                           prevR = MouseFlags.Absolute;
        private char keyPrev = default;
        

>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpServer(this);

<<<<<<< HEAD
            String host = System.Net.Dns.GetHostName();
            // Получение ip-адреса.
            System.Net.IPAddress ip = System.Net.Dns.GetHostEntry(host).AddressList[3];
            // Показ адреса в label'е.
            IDBox.Text = ip.ToString();

            ChangeColor_Click(null, null);
        }

=======
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
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
<<<<<<< HEAD
            
=======
            MessageBox.Show("Value changed");
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
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
<<<<<<< HEAD
=======
        

        private void Tcp_MouseMoveNotify(byte[][] commands)
        {
            InputSimulator input = new InputSimulator();

            double w = BitConverter.ToDouble(commands[0], 0) * (width*34);
            double h = BitConverter.ToDouble(commands[1], 0) * (height*60);
            char leftStatus = BitConverter.ToChar(commands[2], 0);
            char rightStatus = BitConverter.ToChar(commands[3], 0);
            char keyStatus = BitConverter.ToChar(commands[4], 0);
            VirtualKeyCode key = (VirtualKeyCode)BitConverter.ToInt32(commands[5], 0);
            int delta = BitConverter.ToInt32(commands[6], 0);
            

            mouse_event(MouseFlags.Absolute | MouseFlags.Move, (int)w, (int)h, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.MouseWheel, (int)w, (int)h, unchecked(delta), UIntPtr.Zero);

            if (leftStatus == 'd')
            {
                mouse_event(MouseFlags.LeftDown | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                prevL = MouseFlags.LeftDown;
            }
            else if (leftStatus == 'u' && prevL == MouseFlags.LeftDown)
            {
                mouse_event(MouseFlags.LeftUp | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                prevL = MouseFlags.LeftUp;
            }
            else
            {
                mouse_event(MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                prevL = MouseFlags.Absolute;
            }

            if (rightStatus == 'd')
            {
                mouse_event(MouseFlags.RightDown | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                prevR = MouseFlags.RightDown;
            }
            else if (rightStatus == 'u' && prevR == MouseFlags.RightDown)
            {
                mouse_event(MouseFlags.RightUp | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                prevR = MouseFlags.RightUp;
            }
            else
            {
                mouse_event(MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                prevR = MouseFlags.Absolute;
            }


            if (keyStatus == 'd')
            {
                input.Keyboard.KeyDown(key);
                keyPrev = keyStatus;
            }
            else if (keyStatus == 'u' && keyPrev == 'd')
            {
                input.Keyboard.KeyUp(key);
                keyPrev = 'u';
            }
            else
                keyPrev = 'f';


        }



        [Flags]
        enum MouseFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010,
            MouseWheel = 0x0800
        };

        [DllImport("User32.dll", SetLastError = true)]
        static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
    }
}
