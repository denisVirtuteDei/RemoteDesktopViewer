using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NotTeamViewer.Client
{
    /// <summary>
    /// 100% originality <see cref="TcpClient"/> for originality Team Viewer.
    /// </summary>
    class TcpClient_d
    {
        private readonly int localPort = 1488;
        private bool inProc = false;
        private readonly MainWindow main;
        Rectangle rect = Screen.PrimaryScreen.Bounds;
        private string ipAddress = "";

        private int delta = 0;
        private double mouseX = 0;
        private double mouseY = 0;
        private bool keyDown = false;
        private bool keyUp = false;
        private Key key = default;
        private MouseButtonState lState, rState;
        

        /// <summary>
        /// Constructor <see cref="TcpClient_d"/>.
        /// </summary>
        public TcpClient_d(MainWindow main)
        {
            this.main = main;
            this.main.MouseMoveNotify += Main_MouseMoveNotify;
            this.main.MouseClickNotify += Main_MouseClickNotify;
            this.main.KeyClickNotify += Main_KeyClickNotify;
            this.main.MouseWheelNotify += Main_MouseWheelNotify;
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
            else
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

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(ipAddress, localPort);
                    stream = client.GetStream();
                    inProc = true;

                    while (inProc)
                    {
                        var second = (byte[])formatter.Deserialize(stream);

                        DrawNudes(second);

                        main.TextBlock.Dispatcher.Invoke(() =>
                        {
                            main.TextBlock.Text = key.ToString();
                        });

                        formatter.Serialize(stream, GetEvents());
                    }

                }
                catch (Exception exc)
                {
                    main.TextBlock.Dispatcher.Invoke(() =>
                    {
                        main.TextBlock.Text = "Status: " + exc.Message;
                    });
                }
                finally
                {
                    if(stream.DataAvailable)
                        stream.Close();
                    
                    inProc = false;
                    ipAddress = "";
                    ResetVal();
                }
            }
        }

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

        /// <inheritdoc/>
        private BitmapImage BitmapToImageSource(Bitmap bmp)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

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
        }

    }
}
