﻿using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

namespace NotTeamViewer.Client
{
    /// <summary>
    /// Логика взаимодействия для Client MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpClient_d tcp;

<<<<<<< HEAD
        public delegate void KeyClicks(Key sysKey, Key key, bool up);
        public event KeyClicks KeyClickNotify;
        public delegate void MouseClicks(MouseButtonState l, MouseButtonState r);
        public event MouseClicks MouseClickNotify;
        public delegate void MouseDbl(int clicks);
        public event MouseDbl MouseDblNotify;
=======
        public delegate void KeyClicks(Key key, bool up, bool down);
        public event KeyClicks KeyClickNotify;
        public delegate void MouseClicks(MouseButtonState l, MouseButtonState r);
        public event MouseClicks MouseClickNotify;
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
        public delegate void MouseMoves(int x, int y);
        public event MouseMoves MouseMoveNotify;
        public delegate void MouseWheelMove(int delta);
        public event MouseWheelMove MouseWheelNotify;
<<<<<<< HEAD
        public delegate void ResizeImagePanel(double w, double h);
        public event ResizeImagePanel ResizeImagePanelNotify;

=======
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

        /// <summary>
        /// Constructor client <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
<<<<<<< HEAD
            EventInitialize();
=======
            this.KeyDown += MainWindow_KeyClick;
            this.KeyUp += MainWindow_KeyClick;
            
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
            tcp = new TcpClient_d(this);
            ResizeImagePanelNotify(ImagePanel.ActualWidth, ImagePanel.ActualHeight);
        }

<<<<<<< HEAD
        /// <summary>
        /// Start button click event.
        /// </summary>
        private async void TCP_Start(object sender, RoutedEventArgs e)
        {
            if (!tcp.GetinProc() && tcp.SetIP(IPAddress.Text))
            {
                await Task.Run(() => tcp.Run());
            }
        }

        private void Stop_But_Click(object sender, RoutedEventArgs e)
        {
            tcp.SetinProc(false);
            Password.Text = "Password";
        }

        private void EventInitialize()
        {
            this.KeyDown += MainWindow_KeyClick;
            this.KeyUp += MainWindow_KeyClick;

            this.ImagePanel.MouseDown += Image_MouseClick;
            this.ImagePanel.MouseUp += Image_MouseClick;
            this.ImagePanel.MouseMove += Image_MouseMove;
            this.ImagePanel.MouseWheel += Image_MouseWheel;
            this.ImagePanel.SizeChanged += ImagePanel_SizeChanged;
            this.MouseDoubleClick += Window_MouseDoubleClick;

        }
=======

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            var loc = e.GetPosition(ImagePanel);
            MouseMoveNotify((int)loc.X, (int)loc.Y);
        }

        private void Image_MouseClick(object sender, MouseButtonEventArgs e)
        {
            MouseClickNotify(e.LeftButton, e.RightButton);
        }

        private void MainWindow_KeyClick(object sender, KeyEventArgs e)
        {
            KeyClickNotify(e.Key, e.IsUp, e.IsDown);
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheelNotify(e.Delta);
        }

>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e

        private void ImagePanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
<<<<<<< HEAD
            ResizeImagePanelNotify(e.NewSize.Width, e.NewSize.Height);
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            var loc = e.GetPosition(ImagePanel);
            MouseMoveNotify((int)loc.X, (int)loc.Y);
        }

        private void Image_MouseClick(object sender, MouseButtonEventArgs e)
        {
            //TextBlock.Text = "Status: " + e.LeftButton + " " + e.RightButton;
            MouseClickNotify(e.LeftButton, e.RightButton);
        }

        private void MainWindow_KeyClick(object sender, KeyEventArgs e)
        {
            //TextBlock.Text = "Status: " + e.SystemKey + " " + e.Key;
            KeyClickNotify(e.SystemKey, e.Key, e.IsUp);
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheelNotify(e.Delta);
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(ImagePanel);
            if((ImagePanel.ActualWidth - point.X) >= 0 &&
               (ImagePanel.ActualHeight - point.Y) >= 0)
            {
                MouseDblNotify(e.ClickCount);
=======
            if (!tcp.GetinProc() && tcp.SetIP(ipAddress.Text))
            {
                await Task.Run(() => tcp.Run());
>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
            }
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Text.Length == 0)
                Password.Text = "Password";
        }

        private void IPAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IPAddress.Text.Length == 0)
                IPAddress.Text = "IPAddress";
        }
<<<<<<< HEAD
=======

>>>>>>> 33b82a59a32b81d3973c69d1054ce6c2b4de9f4e
    }

}
