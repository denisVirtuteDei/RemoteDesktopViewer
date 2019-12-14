using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
=======
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NotTeamViewer.Client
{
    /// <summary>
    /// 100% originality <see cref="TcpClient"/> for originality Team Viewer.
    /// </summary>
    partial class TcpClient_d
    {
        private readonly int localPort = 5001;
        private bool inProc = false;
        private readonly MainWindow main;
<<<<<<< HEAD
        //Rectangle rect = Screen.PrimaryScreen.Bounds;
        private string ipAddress = "";

        private int delta = 0;
        private double mouseX = 0;
        private double mouseY = 0;
        private bool click = false;
        private bool moveState = false;
        private bool keyUp = false;
        private Key key = default;
        private Key sysKey = default;
        private MouseButtonState lState, rState;
        private double rectW = 1, rectH = 1;

        private AutoResetEvent auto;
        private Thread listener;
        private byte[] bmp;
        //private Stack<Key> keys;
=======
        Rectangle rect = Screen.PrimaryScreen.Bounds;
        private string ipAddress = "";

        private int delta = 0;
        private double mouseX = 0;
        private double mouseY = 0;
        private bool keyDown = false;
        private bool keyUp = false;
        private Key key = default;
        private MouseButtonState lState, rState;
        
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

        /// <summary>
        /// Constructor <see cref="TcpClient_d"/>.
        /// </summary>
        public TcpClient_d(MainWindow main)
        {
            this.main = main;
            Init();
        }

        private void Init()
        {
            this.main.MouseMoveNotify += Main_MouseMoveNotify;
            this.main.MouseClickNotify += Main_MouseClickNotify;
            this.main.KeyClickNotify += Main_KeyClickNotify;
            this.main.MouseWheelNotify += Main_MouseWheelNotify;
<<<<<<< HEAD
            this.main.ResizeImagePanelNotify += Main_ResizeImagePanelNotify;
            this.main.MouseDblNotify += Main_MouseDblNotify;

            bmp = null;
            auto = new AutoResetEvent(true);
            listener = new Thread(new ThreadStart(Do))
            {
                IsBackground = true
            };
            listener.Start();
        }



        private void SetBMP(byte[] value)
        {
            bmp = value;
            auto.Set();
        }

        private void Main_ResizeImagePanelNotify(double w, double h)
        {
            rectW = w;
            rectH = h;
        }

        private void Main_KeyClickNotify(Key sysKey, Key key, bool up)
        {
            if (inProc)
            {
                this.key = key;
                this.sysKey = sysKey;
                keyUp = up;
            }
        }

        private void Main_MouseClickNotify(MouseButtonState l, MouseButtonState r)
        {
            if (inProc)
            {
                lState = l;
                rState = r;
            }
        }

        private void Main_MouseMoveNotify(int x, int y)
        {
            if (inProc)
            {
                mouseX = x;
                mouseY = y;
                moveState = true;
            }
        }

        private void Main_MouseWheelNotify(int delta)
        {
            if (inProc)
            {
                this.delta = delta;
            }
        }

        private void Main_MouseDblNotify(int clicks)
        {
            if(inProc)
            {
                click = true;
            }
        }
=======
        }

       

        private void Main_KeyClickNotify(Key key, bool up, bool down)
        {
            if (inProc)
            {
                this.key = key;
                keyUp = up;
                keyDown = down;
            }

        }

        private void Main_MouseClickNotify(MouseButtonState l, MouseButtonState r)
        {
            if (inProc)
            {
                lState = l;
                rState = r;
            }
        }

        private void Main_MouseMoveNotify(int x, int y)
        {
            if (inProc)
            {
                mouseX = x;
                mouseY = y;
            }
        }

        private void Main_MouseWheelNotify(int delta)
        {
            if (inProc)
            {
                this.delta = delta;
            }
        }

>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

        /// <inheritdoc/>
        public bool GetinProc()
        {
            return inProc;
        }

        /// <inheritdoc/>
        public void SetinProc(bool value)
        {
            inProc = value;
        }

        /// <inheritdoc/>
        public bool SetIP(string ip)
        {
            var strIP = ip.Split('.');

            if (strIP.Length != 4)
            {
                MessageBox.Show("Uncorrect ip address");
                return false;
            }
            
            ipAddress = ip;
            return true;
        }

        /// <summary>
        /// Function of message exchange with TcpServer.
        /// </summary>
        public void Run()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            NetworkStream stream = default;
<<<<<<< HEAD
            inProc = true;
=======
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(ipAddress, localPort);
                    stream = client.GetStream();
<<<<<<< HEAD
                    string password = "";

                    main.Password.Dispatcher.Invoke(() =>
                    {
                        password = main.Password.Text;
                    });

                    formatter.Serialize(stream, password);
                    int start = (int)formatter.Deserialize(stream);

                    if (start != 1)
                    {
                        inProc = false;
                        MessageBox.Show("Incorrect password");
                    }

                    while (inProc)
                    {
                        var str = (byte[])formatter.Deserialize(stream);
                        SetBMP(str);
=======
                    inProc = true;

                    while (inProc)
                    {
                        var second = (byte[])formatter.Deserialize(stream);

                        DrawNudes(second);

                        main.TextBlock.Dispatcher.Invoke(() =>
                        {
                            main.TextBlock.Text = key.ToString();
                        });

>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
                        formatter.Serialize(stream, GetEvents());
                    }

                }
                catch (Exception ex)
                {
                    main.TextBlock.Dispatcher.Invoke(() =>
                    {
                        main.TextBlock.Text = "Status: " + ex.Message;
                    });
                }
                finally
                {
<<<<<<< HEAD
                    if(stream != null)
                        stream.Close();

                    bmp = null;
=======
                    if(stream.DataAvailable)
                        stream.Close();
                    
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
                    inProc = false;
                    ipAddress = "";
                    ResetVal();
                }
            }
        }

<<<<<<< HEAD

        private void DrawNudes()
        {
            main.Dispatcher.Invoke(() =>
            {
                main.ImagePanel.Source = BitmapToImageSource();
            });
=======
        /// <inheritdoc/>
        private void DrawNudes(Bitmap source)
        {
            main.ImagePanel.Dispatcher.Invoke(() =>
            {
                main.ImagePanel.Source = BitmapToImageSource(source);
            }
            );
        }

        private void DrawNudes(byte[] source)
        {
            main.ImagePanel.Dispatcher.Invoke(() =>
            {
                main.ImagePanel.Source = BitmapToImageSource(source);
            }
            );
        }



        /// <inheritdoc/>
        private byte[][] GetEvents()
        {
            char mouseLStatus,
                 mouseRStatus,
                 keyStatus;

            double rectW = 1, rectH = 1;
            main.ImagePanel.Dispatcher.Invoke(() =>
            {
                rectW = main.ImagePanel.ActualWidth;
                rectH = main.ImagePanel.ActualHeight;
            }
            );

            if (lState == MouseButtonState.Pressed)
                mouseLStatus = 'd';
            else if (lState == MouseButtonState.Released)
                mouseLStatus = 'u';
            else
                mouseLStatus = 'f';


            if (rState == MouseButtonState.Pressed)
                mouseRStatus = 'd';
            else if (rState == MouseButtonState.Released)
                mouseRStatus = 'u';
            else
                mouseRStatus = 'f';


            if (keyDown)
                keyStatus = 'd';
            else if (keyUp)
                keyStatus = 'u';
            else
                keyStatus = 'f';


            List<byte[]> bytes = new List<byte[]>
            {
                BitConverter.GetBytes(
                    (mouseX<0?0:mouseX) / rectW),
                BitConverter.GetBytes(
                    (mouseY<0?0:mouseY) / rectH),
                BitConverter.GetBytes(mouseLStatus),
                BitConverter.GetBytes(mouseRStatus),
                BitConverter.GetBytes(keyStatus),
                BitConverter.GetBytes((int)key),
                BitConverter.GetBytes(delta)
            };

            delta = 0;

            return bytes.ToArray();
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        }
        
        private void ResetVal()
        {
            //mouseLeftDown = false;
            //mouseLeftUp = false;
            //mouseRightDown = false;
            //mouseRightUp = false;
            keyDown = false;
            keyUp = false;
            key = default;
        }

<<<<<<< HEAD
        private BitmapImage BitmapToImageSource()
=======
        /// <inheritdoc/>
        private BitmapImage BitmapToImageSource(Bitmap bmp)
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        {
            using (MemoryStream stream = new MemoryStream(bmp))
            {
<<<<<<< HEAD
=======
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

<<<<<<< HEAD

        private void Do()
        {
            while (true)
            {
                auto.WaitOne();
                if (bmp != null)
                    DrawNudes();
            }
        }

        private byte[][] GetEvents()
        {
            char mouseLStatus,
                 mouseRStatus,
                 keyStatus;
            

            if (lState == MouseButtonState.Pressed)
                mouseLStatus = 'd';
            else if (lState == MouseButtonState.Released)
                mouseLStatus = 'u';
            else
                mouseLStatus = 'f';


            if (rState == MouseButtonState.Pressed)
                mouseRStatus = 'd';
            else if (rState == MouseButtonState.Released)
                mouseRStatus = 'u';
            else
                mouseRStatus = 'f';


            if (keyUp)
                keyStatus = 'u';
            else
                keyStatus = 'f';


            List<byte[]> bytes = new List<byte[]>
            {
                BitConverter.GetBytes(
                    (mouseX<0?0:mouseX) / rectW),
                BitConverter.GetBytes(
                    (mouseY<0?0:mouseY) / rectH),
                BitConverter.GetBytes(mouseLStatus),
                BitConverter.GetBytes(mouseRStatus),
                BitConverter.GetBytes(keyStatus),
                BitConverter.GetBytes((int)key),
                BitConverter.GetBytes((int)sysKey),
                BitConverter.GetBytes(delta),
                BitConverter.GetBytes(moveState),
                BitConverter.GetBytes(click)
            };

            delta = 0;
            moveState = false;
            keyUp = false;
            click = false;

            return bytes.ToArray();
        }
        
        private void ResetVal()
        {
            delta = 0;
            moveState = false;
            keyUp = false;
            key = default;
            sysKey = default;
=======
        private BitmapImage BitmapToImageSource(byte[] bmp)
        {
            using (MemoryStream stream = new MemoryStream(bmp))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        }

    }
}