using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using SharpDX.Direct3D9;

namespace NotTeamViewer
{
    class TcpServer
    {
        private readonly int width = 1920;
        private readonly int height = 1080;
        private readonly int localPort = 1488;
        private bool inProc = false;
        private Device device;
        private Surface surface;
        private Direct3D direct3d;


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
            TcpListener listener = new TcpListener(IPAddress.Any, localPort);
            listener.Start();

            while(true)
            {
                using (TcpClient client = listener.AcceptTcpClient())
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryFormatter formatter = new BinaryFormatter();

                        while (inProc && client.Connected)
                        {
                            formatter.Serialize(stream, Frame());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }            
        }

        private Bitmap Frame()
        {            
            device.GetFrontBufferData(0, surface);
            return new Bitmap(Surface.ToStream(surface, SharpDX.Direct3D9.ImageFileFormat.Bmp));
        }

        private void AllScreen(IntPtr hWnd = default)
        {
            direct3d = new Direct3D();
            PresentParameters pp = new PresentParameters()
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard,
                BackBufferFormat = Format.A8R8G8B8,
                BackBufferWidth = width,
                BackBufferHeight = height,
                MultiSampleType = MultisampleType.None,
                DeviceWindowHandle = hWnd,
                PresentationInterval = PresentInterval.Default,
                FullScreenRefreshRateInHz = 0
            };

            device = new Device(direct3d, 0, DeviceType.Hardware, pp.DeviceWindowHandle, CreateFlags.SoftwareVertexProcessing, pp);
            surface = Surface.CreateOffscreenPlain(device, width, height, Format.A8R3G3B2, Pool.Scratch);
        }

    }
}
