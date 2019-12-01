using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace NotTeamViewer.Server
{
    /// <summary>
    /// Логика взаимодействия для server MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpServer tcp;
        private int width = 1920;
        private int height = 1080;
        private MouseFlags prevL = MouseFlags.Absolute,
                           prevR = MouseFlags.Absolute;
        private char keyPrev = default;
        


        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpServer(this);
            tcp.MouseMoveNotify += Tcp_MouseMoveNotify;
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
            MessageBox.Show("Value changed");
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
    }
}
