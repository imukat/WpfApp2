using System;
using System.Windows;
using System.Windows.Navigation;
using Quobject.SocketIoClientDotNet.Client;
using System.Diagnostics;
using System.Windows.Threading;
using System.Net;
using Newtonsoft.Json;

namespace WpfApp2
{
    public static class Class1
    {
        public static string[] ips = {
            "192.168.1.56",
            "10.10.56.56"
        };

        public static Uri page1 = new Uri("Page1.xaml", UriKind.RelativeOrAbsolute);
        public static Uri page2 = new Uri("Page2.xaml", UriKind.RelativeOrAbsolute);

        public static WebClient cli = new WebClient();
        public static Config config = new Config();

        public static MainWindow main    = null;
        public static string ip          = null;
        public static string configJson  = null;
        public static dynamic configData = null;

        public static void ByeBye()
        {
            MessageBoxResult result = MessageBox.Show(
                "Close this app?",
                "This App",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        public static bool GetConfig()
        {
            foreach (string ipa in ips)
            {
                if (GetConfigJson(ipa))
                {
                    configData = JsonConvert.DeserializeObject(configJson);
                    SetConfig();
                    return true;
                }
            }

            return false;
        }

        public static bool GetConfigJson(string ipa)
        {
            bool ans = true;
            string err = "";
            string url = "http://" + ipa + "/client/";

            try
            {
                configJson = cli.DownloadString(url);
            }
            catch (System.Net.WebException e)
            {
                err = e.Message;
                ans = false;
            }

            ip = ipa;

            return ans;
        }

        public static void SetConfig()
        {
            config.ipa = ip;
            config.sts = configData.sts;
            config.req = configData.req;
        }

        public static ApiAnswer TrmtApi(string param)
        {
            ApiAnswer ans;
            string url = "http://" + ip + "/" + param;

            try
            {
                string ret = cli.DownloadString(url);
                int len = ret.Length;
                ans.err = "";
                ans.json = ret.Substring(6, len - 8);
                ans.dat = JsonConvert.DeserializeObject(ans.json);
            }
            catch (System.Net.WebException e)
            {
                ans.err = e.Message;
                ans.json = "";
                ans.dat = null;
            }

            return ans;
        }
    }

    public struct Config
    {
        public string ipa;
        public string sts;
        public string req;
        public string con;
        public string mode;
        public int cnt;
    }

    public struct ApiAnswer
    {
        public string err;
        public string json;
        public dynamic dat;
    }
}
