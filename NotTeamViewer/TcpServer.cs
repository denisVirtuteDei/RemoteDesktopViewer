using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
<<<<<<< HEAD
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
=======
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using SharpDX.Direct3D9;
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

namespace NotTeamViewer.Server
{
    class TcpServer
    {
<<<<<<< HEAD
        private readonly int localPort = 5001;
=======
        private readonly int width = 1920;
        private readonly int height = 1080;
        private readonly int localPort = 1488;
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        private bool inProc = false;
        private bool end = true;
        private Rectangle rect = Screen.PrimaryScreen.Bounds;
        private MainWindow main;
        private static Bitmap bmp;
        private static Size size;
        private static EncoderParameters myEncoderParameters;
<<<<<<< HEAD
        private string _password = "";
        private MouseFlags prevL = MouseFlags.Absolute,
                           prevR = MouseFlags.Absolute;

        private AutoResetEvent auto;
        private Thread _listener;
        private static byte[][] commands = null;


        [Flags]
        private enum MouseFlags
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
        private static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);



=======


        public delegate void MouseMoves(byte[][] str);
        public event MouseMoves MouseMoveNotify;
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

        public TcpServer(MainWindow _main)
        {
            main = _main;
<<<<<<< HEAD
            main.PasswordBox.Dispatcher.Invoke(() =>
            {
                _password = main.PasswordBox.Text;
            });
            Init();
=======

            bmp = new Bitmap(rect.Width, rect.Height);
            size = new Size(rect.Width, rect.Height);
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 40L);

            AllScreen();
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        }

        public bool GetinProc()
        {
            return inProc;
        }

        public bool GetEnd()
        {
            return end;
        }

        public void SetinProc(bool value)
        {
            inProc = value;
        }

        public void Run()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            NetworkStream stream = default;
            TcpListener listener = new TcpListener(IPAddress.Any, localPort);
            inProc = true;
            end = false;
            listener.Start();

            using (TcpClient client = listener.AcceptTcpClient())
            {
                try
                {
                    stream = client.GetStream();

                    var password = (string)formatter.Deserialize(stream);
                    if (_password == password)
                        formatter.Serialize(stream, 1);
                    else
                    {
<<<<<<< HEAD
                        inProc = false;
                        formatter.Serialize(stream, 0);
                    }

                    while (inProc)
                    {
                        formatter.Serialize(stream, Frame());
                        var str = (byte[][])formatter.Deserialize(stream);
                        SetComm(str);
=======
                        formatter.Serialize(stream, Frame());

                        var commands = (byte[][])formatter.Deserialize(stream);
                        MouseMoveNotify(commands);

>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
                    }

                }
                catch (Exception)
                {
<<<<<<< HEAD
                    
=======
                    System.Windows.MessageBox.Show(ex.Message);
                    System.Windows.MessageBox.Show(ex.Source);
                    System.Windows.MessageBox.Show(ex.StackTrace);
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
                }
                finally
                {
                    if (stream != null)
                        stream.Close();

                    end = true;
                    commands = null;
                    inProc = false;
<<<<<<< HEAD
                    listener.Stop();
=======
                    stream.Close();
                    listener.Stop();
                    main.Dispatcher.Invoke(() =>
                    {
                        main.StartStopItem.Header = "Start";
                    });
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
                }
            }
        }


<<<<<<< HEAD
        private void Do()
=======
        private void AllScreen()
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        {
            while (true)
            {
                auto.WaitOne();
                if (commands != null)
                    Ev(commands);
            }
        }

<<<<<<< HEAD
        private void SetComm(byte[][] vs)
        {
            commands = vs;
            auto.Set();
        }

        private void Ev(object send)
        {
            byte[][] commands = send as byte[][];

            InputSimulator input = new InputSimulator();

            double w = BitConverter.ToDouble(commands[0], 0) * 65727;
            double h = BitConverter.ToDouble(commands[1], 0) * 65610;
            char leftStatus = BitConverter.ToChar(commands[2], 0);
            char rightStatus = BitConverter.ToChar(commands[3], 0);
            char keyStatus = BitConverter.ToChar(commands[4], 0);

            int key = BitConverter.ToInt32(commands[5], 0);
            VirtualKeyCode vKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey((Key)key);
            int sysKey = BitConverter.ToInt32(commands[6], 0);
            VirtualKeyCode vSysKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey((Key)sysKey);

            int delta = BitConverter.ToInt32(commands[7], 0);
            bool move = BitConverter.ToBoolean(commands[8], 0);
            bool clicks = BitConverter.ToBoolean(commands[9], 0);


            mouse_event(MouseFlags.MouseWheel, (int)w, (int)h, unchecked(delta), UIntPtr.Zero);

            if (clicks)
            {
                input.Mouse.LeftButtonDoubleClick();
            }
            else
            {
                if (leftStatus == 'd' && prevL != MouseFlags.LeftDown)
                {
                    mouse_event(MouseFlags.LeftDown | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                    prevL = MouseFlags.LeftDown;
                }
                else if (leftStatus == 'u' && prevL == MouseFlags.LeftDown)
                {
                    mouse_event(MouseFlags.LeftUp | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                    prevL = MouseFlags.LeftUp;
                }
                else if (prevL != MouseFlags.LeftDown)
                {
                    mouse_event(MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                    prevL = MouseFlags.Absolute;
                }

                if (rightStatus == 'd' && prevR != MouseFlags.RightDown)
                {
                    mouse_event(MouseFlags.RightDown | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                    prevR = MouseFlags.RightDown;
                }
                else if (rightStatus == 'u' && prevR == MouseFlags.RightDown)
                {
                    mouse_event(MouseFlags.RightUp | MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                    prevR = MouseFlags.RightUp;
                }
                else if (prevR != MouseFlags.RightDown)
                {
                    mouse_event(MouseFlags.Absolute, (int)w, (int)h, 0, UIntPtr.Zero);
                    prevR = MouseFlags.Absolute;
                }
            }

            if (move)
                input.Mouse.MoveMouseTo(w, h);


            if (keyStatus == 'u')
            {
                input.Keyboard.KeyPress(vKey);
            }
        }

        private byte[] Frame()
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, size);
            }

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Png);

            MemoryStream memoryStream = new MemoryStream();


            var bitmap = bmp.Clone(rect, PixelFormat.Format16bppArgb1555);

            bitmap.Save(memoryStream, jpgEncoder, myEncoderParameters);


            return memoryStream.ToArray();
=======
        private byte[] Frame()
        {            
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.CopyFromScreen(0, 0, 0, 0, size);
            }

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Png);

            MemoryStream memoryStream = new MemoryStream();

            var bitmap = bmp.Clone(rect, PixelFormat.Format16bppRgb555);

            bitmap.Save(memoryStream, jpgEncoder, myEncoderParameters);


            return memoryStream.ToArray();
            //return (Bitmap)Image.FromStream(memoryStream);
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
<<<<<<< HEAD

        private void Init()
        {
            bmp = new Bitmap(rect.Width, rect.Height);
            size = new Size(rect.Width, rect.Height);
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 25L);

            bmp.SetResolution((int)(bmp.HorizontalResolution * 0.8), (int)(bmp.VerticalResolution * 0.8));

            auto = new AutoResetEvent(true);
            _listener = new Thread(new ThreadStart(Do))
            {
                IsBackground = true
            };
            _listener.Start();
        }
=======
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
    }
}
