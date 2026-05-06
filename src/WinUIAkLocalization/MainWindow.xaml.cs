using Microsoft.UI.Xaml;

namespace WinUIAkLocalization
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RootFrame.Navigate(typeof(MainPage));
        }
    }
}
