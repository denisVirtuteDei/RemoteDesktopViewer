using System.Windows;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpClient_t tcp;


        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpClient_t(this);
        }
        

        private async void TCP_Start(object sender, RoutedEventArgs e)
        {
            if (!tcp.GetinProc())
            {
                tcp.SetinProc(true);
                await Task.Run(() => tcp.Run());
            }
        }

        private void Stop_But_Click(object sender, RoutedEventArgs e)
        {
            tcp.SetinProc(false);
        }
    }
}
