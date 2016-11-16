using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Captcha
{
    static class Program
    {

        public class foof : Captchaaaa
        {

            // static int seed;
            // static readonly System.Threading.ThreadLocal<Random> random;


            static foof()
            {
                // seed = Environment.TickCount;
                // random = new System.Threading.ThreadLocal<Random>(() => new Random(System.Threading.Interlocked.Increment(ref seed)));
            }


            public override System.Drawing.Image Image
            {
                get;
                set;
            }


            private int width;
            private int height;





            private System.Drawing.Image GenerateImage()
            {
                System.Random random = new System.Random(System.Environment.TickCount);

                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(this.width, this.height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, this.width, this.height);

                System.Drawing.Drawing2D.HatchBrush hatchBrush = new System.Drawing.Drawing2D.HatchBrush(
                        System.Drawing.Drawing2D.HatchStyle.SmallConfetti,
                        System.Drawing.Color.LightGray, System.Drawing.Color.White
                );

                g.FillRectangle(hatchBrush, rect);
                System.Drawing.SizeF size;
                float fontSize = rect.Height + 1;
                System.Drawing.Font font;

                do
                {
                    fontSize--;
                    font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, fontSize, System.Drawing.FontStyle.Bold);
                    size = g.MeasureString(this.Text, font);
                } while (size.Width > rect.Width);

                System.Drawing.StringFormat format = new System.Drawing.StringFormat();
                format.Alignment = System.Drawing.StringAlignment.Center;
                format.LineAlignment = System.Drawing.StringAlignment.Center;
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                // path.AddString(this.text, font.FontFamily, (int) font.Style, 
                // font.Size, rect, format);
                path.AddString(this.Text, font.FontFamily, (int)font.Style, 75, rect, format);
                float v = 4F;
                System.Drawing.PointF[] points =
                {
                    new System.Drawing.PointF(random.Next(rect.Width) / v, random.Next(
                        rect.Height) / v),
                    new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v, 
                        random.Next(rect.Height) / v),
                    new System.Drawing.PointF(random.Next(rect.Width) / v, 
                        rect.Height - random.Next(rect.Height) / v),
                    new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v,
                        rect.Height - random.Next(rect.Height) / v)
                };

                System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Translate(0F, 0F);
                path.Warp(points, rect, matrix, System.Drawing.Drawing2D.WarpMode.Perspective, 0F);
                hatchBrush = new System.Drawing.Drawing2D.HatchBrush(
                    System.Drawing.Drawing2D.HatchStyle.Percent10
                    , System.Drawing.Color.Black, System.Drawing.Color.SkyBlue);
                g.FillPath(hatchBrush, path);
                int m = System.Math.Max(rect.Width, rect.Height);
                for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
                {
                    int x = random.Next(rect.Width);
                    int y = random.Next(rect.Height);
                    int w = random.Next(m / 50);
                    int h = random.Next(m / 50);
                    g.FillEllipse(hatchBrush, x, y, w, h);
                }
                font.Dispose();
                hatchBrush.Dispose();
                g.Dispose();
                return bitmap;
            }





        }


        public abstract class Captchaaaa
        {

            public string m_text;
            private System.Drawing.Imaging.ImageFormat m_imageFormat;


            public virtual string Text
            {
                get {
                    if (string.IsNullOrEmpty(this.m_text))
                        this.m_text = this.RandomToken;

                    return this.m_text; 
                }
                set { this.m_text = value; }
            }


            public virtual string RandomToken
            {
                get
                {
                    System.Random r = new System.Random();
                    string s = "";
                    for (int j = 0; j < 5; j++)
                    {
                        int i = r.Next(3);
                        int ch;
                        switch (i)
                        {
                            case 1:
                                ch = r.Next(0, 9);
                                s = s + ch.ToString();
                                break;
                            case 2:
                                ch = r.Next(65, 90);
                                s = s + System.Convert.ToChar(ch).ToString();
                                break;
                            case 3:
                                ch = r.Next(97, 122);
                                s = s + System.Convert.ToChar(ch).ToString();
                                break;
                            default:
                                ch = r.Next(97, 122);
                                s = s + System.Convert.ToChar(ch).ToString();
                                break;
                        }
                        r.NextDouble();
                        r.Next(100, 1999);
                    }
                    return s;
                }
            }


            public virtual System.Drawing.Imaging.ImageFormat Format
            {
                get
                {
                    if (m_imageFormat == null)
                        m_imageFormat = System.Drawing.Imaging.ImageFormat.Png;

                    return m_imageFormat;
                }
                set {
                    this.m_imageFormat = value;
                }
            }


            private string m_MimeType;

            public virtual string MimeType
            {
                get
                {
                    if (!String.IsNullOrEmpty(m_MimeType))
                    {
                        return this.m_MimeType;
                    }

                    System.Guid formatUID = this.Format.Guid;

                    System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                    for (int i = 0; i < codecs.Length; ++i)
                    {
                        if (codecs[i].FormatID == formatUID)
                        {
                            this.m_MimeType = codecs[i].MimeType;
                            return this.m_MimeType;
                        }
                            
                    }

                    this.m_MimeType = "image/unknown";
                    return this.m_MimeType;
                }
            }


            public abstract System.Drawing.Image Image
            {
                get;
                set;
            }
                
            

            public virtual byte[] ImageBytes
            {
                get
                {
                    byte[] imageBytes = null;

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        this.Image.Save(ms, this.Format);
                        imageBytes = ms.ToArray();
                    }

                    return imageBytes;
                }
            }

            public virtual string Base64()
            {
                return System.Convert.ToBase64String(this.ImageBytes);
            }


        }



        // https://www.digitalocean.com/community/tutorials/understanding-ip-addresses-subnets-and-cidr-notation-for-networking
        public static void CIDR2IP(string IP)
        {   
            string[] parts = IP.Split('.', '/');

            uint ipnum = (System.Convert.ToUInt32(parts[0]) << 24) |
                (System.Convert.ToUInt32(parts[1]) << 16) |
                (System.Convert.ToUInt32(parts[2]) << 8) |
                System.Convert.ToUInt32(parts[3]);

            int maskbits = System.Convert.ToInt32(parts[4]);
            uint mask = 0xffffffff;
            mask <<= (32 - maskbits);

            uint ipstart = ipnum & mask;
            uint ipend = ipnum | (mask ^ 0xffffffff);

            string fromRange = string.Format("{0}.{1}.{2}.{3}", ipstart >> 24, (ipstart >> 16) & 0xff, (ipstart >> 8) & 0xff, ipstart & 0xff);
            string toRange = string.Format("{0}.{1}.{2}.{3}", ipend >> 24, (ipend >> 16) & 0xff, (ipend >> 8) & 0xff, ipend & 0xff);

            System.Console.WriteLine(fromRange + " - " + toRange);
        }


        public static uint IP2num(string ip)
        {
            string[] nums = ip.Split('.');
            uint first = System.UInt32.Parse(nums[0]);
            uint second = System.UInt32.Parse(nums[1]);
            uint third = System.UInt32.Parse(nums[2]);
            uint fourth = System.UInt32.Parse(nums[3]);

            return (first << 24) | (second << 16) | (third << 8) | (fourth);
        }


        // https://dev.maxmind.com/geoip/
        // https://stackoverflow.com/questions/461742/how-to-convert-an-ipv4-address-into-a-integer-in-c
        public static string IPrange2CIDR(string ip1, string ip2)
        {
            uint startAddr = IP2num(ip1);
            uint endAddr = IP2num(ip2);

            // uint startAddr = 0xc0a80001; // 192.168.0.1
            // uint endAddr = 0xc0a800fe;   // 192.168.0.254
            // uint startAddr = System.BitConverter.ToUInt32(System.Net.IPAddress.Parse(ip1).GetAddressBytes(), 0);
            // uint endAddr = System.BitConverter.ToUInt32(System.Net.IPAddress.Parse(ip2).GetAddressBytes(), 0);

            if (startAddr > endAddr)
            {
                uint temp = startAddr;
                startAddr = endAddr;
                endAddr = temp;
            }

            // uint diff = endAddr - startAddr -1;
            // int bits =  32 - (int)System.Math.Ceiling(System.Math.Log10(diff) / System.Math.Log10(2));
            // return ip1 + "/" + bits;

            uint diffs = startAddr ^ endAddr;

            // Now count the number of consecutive zero bits starting at the most significant
            int bits = 32;
            // int mask = 0;

            // We keep shifting diffs right until it's zero (i.e. we've shifted all the non-zero bits off)
            while (diffs != 0)
            {
                diffs >>= 1;
                bits--; // Every time we shift, that's one fewer consecutive zero bits in the prefix
                // Accumulate a mask which will have zeros in the consecutive zeros of the prefix and ones elsewhere
                // mask = (mask << 1) | 1;
            }

            string res = ip1 + "/" + bits;
            System.Console.WriteLine(res);
            return res;
        }


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string IP = "5.39.40.96/27";
            // IP = "88.84.128.0/19";
            CIDR2IP(IP);

            // IPrange2CIDR("88.84.128.0", "88.84.159.255");
            IPrange2CIDR("5.39.40.96", "5.39.40.127");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
