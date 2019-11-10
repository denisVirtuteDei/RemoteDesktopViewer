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
        private TcpServer tcp;


        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpServer();
            StartServer();
        }

        private async void StartServer()
        {
            if (!tcp.GetinProc())
            {
                tcp.SetinProc(true);
                await Task.Run(() => tcp.Run());
            }
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Value changed");
        }
    }
}
