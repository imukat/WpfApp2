using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Page1.xaml の相互作用ロジック
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();

            Class1.config.cnt = 0;

            if (Class1.configData == null)
            {
                GetConfig();
            }

            if (Class1.configData != null)
            {
                Class1.main.GotoPage(Class1.page2);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Class1.ByeBye();
        }

        private void GetConfig()
        {
            if (Class1.GetConfig())
            {
                TxtBox1.Text = "Success";
            }
            else
            {
                TxtBox1.Text = "Can't get configuration data.";
            }
        }
    }
}
