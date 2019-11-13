using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using SharpDX.Direct3D9;

namespace NotTeamViewer.Server
{
    class TcpServer
    {
        private readonly int width = 1920;
        private readonly int height = 1080;
        private int divider = 8;
        private readonly int localPort = 1488;
        private bool inProc = false;
        private Device device;
        private Surface surface;
        private Rectangle rect = Screen.PrimaryScreen.Bounds;

        public TcpServer()
        {
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
            Bitmap first;
            TcpListener listener = new TcpListener(IPAddress.Any, localPort);
            listener.Start();

            using (TcpClient client = listener.AcceptTcpClient())
            {
                try
                {
                    NetworkStream stream = client.GetStream();

                    while (inProc)
                    {
                        first = Frame2();
                        formatter.Serialize(stream, first);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    inProc = false;
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

        private Bitmap Frame2()
        {
            device.GetFrontBufferData(0, surface);
            var bmp = (Bitmap)Image.FromStream(Surface.ToStream(surface, ImageFileFormat.Bmp));
            return bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
        }

        
        
        
        
        /// <summary>
        /// Split screenshot up to pictures. And set next divider based on last screen. 
        /// </summary>
        /// <param name="pieces">
        /// List of <see cref="Rectangle"/> which should be initialized before.
        /// </param>
        private List<byte[]> SplitIntoParts(List<Rectangle> pieces)
        {
            List<byte[]> bytes = new List<byte[]>();
            var pixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppRgb565;

            Bitmap bitmap;
            Bitmap backGround = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(backGround);

            // Don't know, why i added this properties. Maybe help, Amen!
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));

            //int count = 0;
            foreach (var elem in pieces)
            {
                bitmap = backGround.Clone(elem, pixelFormat);
                //var piece = ConvertToBytes(bitmap);
                bytes.Add(ConvertToBytes(bitmap));
                //count += piece.Length;
            }

            //SetDivider(count);
            return bytes;
        }
        
        private byte[] ConvertToBytes(Bitmap bmp)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bmp.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Create and fill <see cref="Rectangle"/>[] by dividerX * dividerY elements.
        /// </summary>
        private List<Rectangle> InitRectangleList()
        {
            List<Rectangle> pieces = new List<Rectangle>();

            int dividerX = 4;
            int dividerY = divider / 4;
            int localX = width / dividerX;
            int localY = height / dividerY;

            for (int j = 0; j < dividerY; j++)
            {
                for (int i = 0; i < dividerX; i++)
                {
                    Rectangle piece = new Rectangle()
                    {
                        X = i * localX,
                        Y = j * localY,
                        Width = localX,
                        Height = localY
                    };

                    // Will work, if window width does not wholly divided by 5.
                    if (i == dividerX - 1)
                    {
                        piece.Width = width - (i * localX);
                    }

                    pieces.Add(piece);
                }
            }

            return pieces;
        }

    }
}
