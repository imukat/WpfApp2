using System;
using System.Windows.Navigation;

namespace WpfApp2
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "My Title (^^;";
            Class1.main = this;
            this.Navigate(Class1.page1);
        }

        public void GotoPage(Uri page)
        {
            this.Navigate(page);
        }
    }
}
