using System.Windows;
using System.Threading.Tasks;

namespace NotTeamViewer.Client
{
    /// <summary>
    /// Логика взаимодействия для Client MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpClient_d tcp;

        /// <summary>
        /// Constructor client <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpClient_d(this);
        }

        /// <summary>
        /// Event for start button click.
        /// </summary>
        private async void TCP_Start(object sender, RoutedEventArgs e)
        {
            if (!tcp.GetinProc())
            {
                tcp.SetinProc(true);
                await Task.Run(() => tcp.Run());
            }
        }

        /// <summary>
        /// Event for stop button click.
        /// </summary>
        private void Stop_But_Click(object sender, RoutedEventArgs e)
        {
            tcp.SetinProc(false);
        }
    }
}
