
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
        } // End Function MapProjectPath 


        public class IpBlock
        {
            public string network;
            public string geoname_id; // Country where the IP is used
            public string registered_country_geoname_id; // Country the IP is registered in 
            public string represented_country_geoname_id; // Country that IP represents, e.g. US military base in Rammstein, Germany
            public string is_anonymous_proxy;
            public string is_satellite_provider;
        } // End Class IpBlock 


        public class Location
        {
            public string geoname_id;
            public string locale_code; // Localization language
            public string continent_code;
            public string continent_name;
            public string country_iso_code; // ISO 3166-2,
            public string country_name;
        } // End Class Location 


        public static void InsertSQL(System.Text.StringBuilder sb)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            
            csb.DataSource = System.Environment.MachineName;
            csb.InitialCatalog = "Blogz";

            csb.IntegratedSecurity = true;
            csb.PersistSecurityInfo = false;
            csb.MultipleActiveResultSets = true;
            csb.PacketSize = 4096;
            

            if (!csb.IntegratedSecurity)
            {
                csb.UserID = "";
                csb.Password = "";
            } // End if (!csb.IntegratedSecurity) 

            using (System.Data.Common.DbConnection con = new System.Data.SqlClient.SqlConnection(csb.ConnectionString))
            {
                using (System.Data.Common.DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sb.ToString();
                    
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    cmd.ExecuteNonQuery();

                    if (con.State != System.Data.ConnectionState.Closed)
                        con.Close();

                } // End Using cmd 

            } // End Using con 

        } // End Sub InsertSQL 


        public static void ReadLineByLine(string fileName)
        {
            int counter = 1;
            string line;

            int commandCounter = 0;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Read the file and display it line by line.
            using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                { 

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            counter++;
                            continue;
                        } // End if (string.IsNullOrEmpty(line)) 
                            

                        sb.AppendLine(line);
                        commandCounter++;

                        if (commandCounter == 100)
                        {
                            InsertSQL(sb);
                            commandCounter = 0;
                            sb.Length = 0;
                        } // End if (commandCounter == 100) 


                        System.Console.WriteLine("Reading line {0}", counter);
                        System.Console.WriteLine(line);
                        counter++;
                    } // Whend 

                } // End Using sr 

                fs.Close();
            } // End Using fs 

            if (sb.Length != 0)
                InsertSQL(sb);
        } // End Sub ReadLineByLine 


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            IPv4Helper.Test();
            // IPv6Helper.Test();

            string fileIPv4 = MapProjectPath(@"IP/GeoLite/GeoLite2-Country-Blocks-IPv4.csv");
            string fileIPv6 = MapProjectPath(@"IP/GeoLite/GeoLite2-Country-Blocks-IPv6.csv");

            string sqlFile = MapProjectPath(@"IP/GeoLite/geoip_locations_temp.sql");
            ReadLineByLine(sqlFile);
            sqlFile = MapProjectPath(@"IP/GeoLite/geoip_blocks_temp.sql");
            ReadLineByLine(sqlFile);

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
            } // Next i 



            System.Console.WriteLine(fileIPv4);
            System.Console.WriteLine(fileIPv6);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        } // End Sub Main 


    } // End Class Program 


} // End Namespace Captcha 
