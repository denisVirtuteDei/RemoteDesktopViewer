using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace NotTeamViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpServer tcp;


        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpServer();

            StartServerButton_Click();
        }

        private async void StartServerButton_Click()
        {
            if (!tcp.GetinProc())
            {
                tcp.SetinProc(true);
                await Task.Run(() => tcp.Run());
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            tcp.SetinProc(false);
        }
    }
}
