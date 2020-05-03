namespace TreeViewMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GithubButton_Click(object sender, System.Windows.RoutedEventArgs e)

        {
            System.Diagnostics.Process.Start("https://github.com/GENiEBEN/GENAIO/releases");
        }
    }
}
