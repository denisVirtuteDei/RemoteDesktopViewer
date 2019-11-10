using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;

namespace Client
{
    class TcpClient_t
    {
        private int localPort = 1488;
        private bool inProc = false;
        private readonly MainWindow main;

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public TcpClient_t(MainWindow main)
        {
            this.main = main;
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
            var rect = Screen.PrimaryScreen.Bounds;
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.Black);

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect("192.168.1.10", localPort);
                    NetworkStream stream = client.GetStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    Bitmap bitmap;

                    while (inProc && client.Connected)
                    {
                        bitmap = (Bitmap)formatter.Deserialize(stream);
                        g.DrawImage(new Bitmap(bitmap, rect.Width, rect.Height), 0, 0);

                        main.ImagePanel.Dispatcher.Invoke(() =>
                        {
                            var handle = bmp.GetHbitmap();
                            try
                            {
                                main.ImagePanel.Source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                                DeleteObject(handle);
                            }
                            catch (Exception)
                            {
                                DeleteObject(handle);
                                return;
                            }
                        }
                        );
                    }

                    stream.Close();
                }
                finally
                {
                    inProc = false;                  
                }
            }
        }
    }
}
