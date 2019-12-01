using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using SharpDX.Direct3D9;

namespace NotTeamViewer.Server
{
    class TcpServer
    {
        private readonly int width = 1920;
        private readonly int height = 1080;
        private readonly int localPort = 1488;
        private bool inProc = false;
        private Device device;
        private Surface surface;
        private Rectangle rect = Screen.PrimaryScreen.Bounds;
        private MainWindow main;
        private static Bitmap bmp;
        private static Size size;
        private static EncoderParameters myEncoderParameters;


        public delegate void MouseMoves(byte[][] str);
        public event MouseMoves MouseMoveNotify;

        public TcpServer(MainWindow _main)
        {
            main = _main;

            bmp = new Bitmap(rect.Width, rect.Height);
            size = new Size(rect.Width, rect.Height);
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 40L);

            AllScreen();
        }

        public bool GetinProc()
        {
            return inProc;
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
            listener.Start();

            using (TcpClient client = listener.AcceptTcpClient())
            {
                try
                {
                    stream = client.GetStream();

                    while (inProc)
                    {
                        formatter.Serialize(stream, Frame());

                        var commands = (byte[][])formatter.Deserialize(stream);
                        MouseMoveNotify(commands);

                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    System.Windows.MessageBox.Show(ex.Source);
                    System.Windows.MessageBox.Show(ex.StackTrace);
                }
                finally
                {
                    inProc = false;
                    stream.Close();
                    listener.Stop();
                    main.Dispatcher.Invoke(() =>
                    {
                        main.StartStopItem.Header = "Start";
                    });
                }
            }
        }


        private void AllScreen()
        {
            var format = Format.A8R8G8B8;
            PresentParameters pp = new PresentParameters()
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard,
                BackBufferWidth = width,
                BackBufferHeight = height,
                MultiSampleType = MultisampleType.None,
                DeviceWindowHandle = default,
                PresentationInterval = PresentInterval.Default,
                FullScreenRefreshRateInHz = 0
            };

            device = new Device(new Direct3D(), 0, DeviceType.Hardware, pp.DeviceWindowHandle, CreateFlags.HardwareVertexProcessing, pp);
            surface = Surface.CreateOffscreenPlain(device, width, height, format, Pool.Scratch);
        }

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
    }
}
