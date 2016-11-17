
using System.Windows.Forms;


namespace Captcha
{


    static class Program
    {


        public static string MapProjectPath(string path)
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
            dir = System.IO.Path.Combine(dir, "../..");
            dir = System.IO.Path.Combine(dir, path);
            dir = System.IO.Path.GetFullPath(dir);

            return dir;
        }


        public class IpBlock
        {
            public string network;
            public string geoname_id; // Country where the IP is used
            public string registered_country_geoname_id; // Country the IP is registered in 
            public string represented_country_geoname_id; // Country that IP represents, e.g. US military base in Rammstein, Germany
            public string is_anonymous_proxy;
            public string is_satellite_provider;
        }


        public class Location
        {
            public string geoname_id;
            public string locale_code; // Localization language
            public string continent_code;
            public string continent_name;
            public string country_iso_code; // ISO 3166-2,
            public string country_name;

        }


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            // IPv4Helper.Test();
            // IPv6Helper.Test();
            System.Console.WriteLine(System.Text.Encoding.UTF8.CodePage);
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("iso-8859-1");
            System.Console.WriteLine(enc.CodePage);

            string fileIPv4 = MapProjectPath(@"IP\GeoLite\GeoLite2-Country-Blocks-IPv4.csv");
            string fileIPv6 = MapProjectPath(@"IP\GeoLite\GeoLite2-Country-Blocks-IPv6.csv");
            fileIPv4 = System.IO.File.ReadAllText(fileIPv4, System.Text.Encoding.UTF8);
            fileIPv6 = System.IO.File.ReadAllText(fileIPv6, System.Text.Encoding.UTF8);


            fileIPv4 = fileIPv4.Replace("\r\n", "\n").Replace("\r", "\n");
            fileIPv6 = fileIPv6.Replace("\r\n", "\n").Replace("\r", "\n");

            string[] recordsIPv4 = fileIPv4.Split('\n');
            string[] recordsIPv6 = fileIPv6.Split('\n');

            for (long i = 0; i < recordsIPv4.LongLength; ++i)
            {
                string line = recordsIPv4[i];
                string[] values = line.Split(',');
                System.Console.WriteLine(values);
                System.Console.WriteLine(line);
            }





                System.Console.WriteLine(fileIPv4);
            System.Console.WriteLine(fileIPv6);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        } // End Sub Main 


    } // End Class Program 


} // End Namespace Captcha 
