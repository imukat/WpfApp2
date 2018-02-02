using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Page2.xaml の相互作用ロジック
    /// </summary>
    public partial class Page2 : Page
    {
        private Socket socket;

        public Page2()
        {
            InitializeComponent();

            Class1.config.con  = "Not connected";
            Class1.config.mode = "(none)";

            SetCmbBox2();
            Connect();
            GetImage();

            Check();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Class1.ByeBye();
        }

        private void Check()
        {
            TxtBox1.Text = "Host IP: " + Class1.config.ipa + "\n"
                         + "Status: " + Class1.config.sts + "\n"
                         + "Request: " + Class1.config.req + "\n"
                         + "Connection: " + Class1.config.con + "\n"
                         + "Mode: " + Class1.config.mode + "\n"
                         + "Count: " + Class1.config.cnt.ToString() + "\n"
                         ;
        }

        private void Connect()
        {
            var options = new IO.Options() { IgnoreServerCertificateValidation = true, AutoConnect = true, ForceNew = true };
            socket = IO.Socket("https://" + Class1.config.ipa +":3000", options);

            // 接続時のイベント
            socket.On(Socket.EVENT_CONNECT, async () =>
            {
                // UIとは別スレッドなので Dispatcher を利用する
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    Class1.config.con = "Connected";
                    Check();
                }));
            });

            // "msg" 受信時
            socket.On("message", async (data) =>
            {
                // UIとは別スレッドなので Dispatcher を利用する
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtBox2.Text = Convert.ToString(data);
                    Class1.config.cnt++;
                    Check();
                }));
            });
        }

        private void SetCmbBox2()
        {
            ComboBoxItem cmb;

            CmbBox2.Background = Brushes.LightBlue;

            cmb = new ComboBoxItem();
            cmb.Tag = "111";
            cmb.Content = "Created with C#";
            cmb.IsSelected = false;
            CmbBox2.Items.Add(cmb);

            cmb = new ComboBoxItem();
            cmb.Tag = "112";
            cmb.Content = "Created with VB";
            cmb.IsSelected = true;
            CmbBox2.Items.Add(cmb);
        }

        private void CmbBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = ((sender as ComboBox).SelectedItem as ComboBoxItem).Tag as string
                        + ": "
                        + ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string
                        ;
            Class1.config.mode = text;
            Check();
        }

        private void GetImage()
        {
            Uri uri = new Uri("http://" + Class1.config.ipa + "/samples/webgl/images/SwedishRoyalCastle/px.jpg");
            BitmapImage bm = new BitmapImage(uri);
            Img1.Source = bm;
        }

        private void BtnGetDat_Click(object sender, RoutedEventArgs e)
        {
            string param = "api?request=getTournamentList&arg={\"token\"%3A\"d2DdVwoB5r5jldZnIGC0PkJqidIhNL4tQo9jk0uXgno%3D\"}";
            ApiAnswer ans = Class1.TrmtApi(param);
            string msg = ans.dat.msg;
            int i = 0;
            Console.WriteLine(ans.dat);
            TxtBox3.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            TxtBox3.Text = ans.json + "\nmsg = " + msg;
            foreach (dynamic lst in ans.dat.list)
            {
                TxtBox3.Text += "\nlist[" + i + "]";
                TxtBox3.Text += "\n  id: " + lst.id;
                TxtBox3.Text += "\n  name: " + lst.name;
                TxtBox3.Text += "\n  begin: " + lst.begin;
                TxtBox3.Text += "\n  end: " + lst.end;
            }
        }
    }
}
