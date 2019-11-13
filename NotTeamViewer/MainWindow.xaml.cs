using System.Windows;
using System.Threading.Tasks;

namespace NotTeamViewer.Server
{
    /// <summary>
    /// Логика взаимодействия для server MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TcpServer tcp;

        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpServer();            
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

        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
            if(tcp.GetinProc())
            {
                tcp.SetinProc(false);
                
                StartStopItem.Header = "Start";
            }
            else
            {
                StartStopItem.Header = "Stop";
                StartServer();
            }
        }
    }
}
