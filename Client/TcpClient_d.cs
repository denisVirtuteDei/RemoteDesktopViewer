﻿using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Windows.Input;

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
        private Rectangle rect;
        private string ipAddress = "127.0.0.1";
        private double mouseX;
        private double mouseY;
        private bool mouseLeftDown;
        private bool mouseRightDown;
        private Key key; 



        /// <summary>
        /// Constructor <see cref="TcpClient_d"/>.
        /// </summary>
        public TcpClient_d(MainWindow main)
        {
            this.main = main;
            this.main.MouseMoveNotify += Main_MouseMoveNotify;
            this.main.MouseClickNotify += Main_MouseClickNotify;
            this.main.KeyClickNotify += Main_KeyClickNotify;
            main.Dispatcher.Invoke(() =>
            {
                rect = new Rectangle(0, 0,
                       (int)main.ImagePanel.MaxWidth,
                       (int)main.ImagePanel.MaxHeight);
                
            });
        }

        private void Main_KeyClickNotify(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.InputSource.Dispatcher.Invoke(() =>
            {
                key = e.Key;
            }
            );
        }

        private void Main_MouseClickNotify(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Dispatcher.Invoke(() =>
            {
                mouseLeftDown = e.LeftButton == MouseButtonState.Pressed;
                mouseRightDown = e.RightButton == MouseButtonState.Pressed;
            }
            );           
        }

        private void Main_MouseMoveNotify(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.MouseDevice.Dispatcher.Invoke(() =>
            {
                var loc = e.GetPosition(null);
                mouseX = loc.X;
                mouseY = loc.Y;
            }
            );            
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
            var pieces = InitRectangleMas(8);
            BinaryFormatter formatter = new BinaryFormatter();
            Bitmap first;

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(ipAddress, localPort);
                    NetworkStream stream = client.GetStream();
                    inProc = true;

                    while (inProc)
                    {
                        first = (Bitmap)formatter.Deserialize(stream); 
                        
                        stream.FlushAsync();
                        formatter.Serialize(stream, GetEvents());

                        DrawNudes(first);
                    }

                    stream.Close();
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
                    inProc = false;
                }
            }
        }

        private void DrawNudes(Bitmap source)
        {
            main.ImagePanel.Dispatcher.Invoke(() =>
            {
                main.ImagePanel.Source = BitmapToImageSource(source);
            }
            );
        }

        private byte[][] GetEvents()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(BitConverter.GetBytes(mouseX / rect.Width));
            bytes.Add(BitConverter.GetBytes(mouseY / rect.Height));
            bytes.Add(BitConverter.GetBytes(mouseLeftDown));
            bytes.Add(BitConverter.GetBytes(mouseRightDown));

            return bytes.ToArray();
        }

        /// <summary>
        /// Convert bitmap to image source. 
        /// </summary>
        private BitmapImage BitmapToImageSource(Bitmap bmp)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        /// <summary>
        /// Create and fill <see cref="Rectangle"/>[] by dividerX * dividerY elements.
        /// </summary>
        private Rectangle[] InitRectangleMas(int divider)
        {
            List<Rectangle> pieces = new List<Rectangle>();

            int dividerX = 4;
            int dividerY = divider / 4;
            int localX = rect.Width / dividerX;
            int localY = rect.Height / dividerY;


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
                        piece.Width = rect.Width - (i * localX);
                    }

                    pieces.Add(piece);
                }
            }

            return pieces.ToArray();
        }
    }
}
