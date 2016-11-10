

/*
 * RNCaptcha
 * 
 * version 1.0
 * 
 * Copyright (c) 2014, Roger Ngo <rogerngo90@gmail.com>
 * 
 * GPL License
 * http://www.gnu.org/licenses/gpl.html
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


// https://rogerngo.wordpress.com/2014/05/11/creating-a-simple-captcha-in-net/
namespace Captcha
{


    public class RNCaptcha
    {
        private int width;
        private int height;
        private int fontSize;
        private string answer;


        public RNCaptcha(string answer)
        {
            this.width = 192;
            this.height = 48;
            this.fontSize = 24;
            this.answer = answer;
        }

        public RNCaptcha(int width, int height, int fontSize, string answer)
        {
            this.width = width;
            this.height = height;
            this.fontSize = fontSize;
            this.answer = answer;
        }

        public static string GenerateRandomString()
        {
            string universe = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@$#%^&*";
            string result = "";

            Random rand = new Random();

            for (int i = 0; i < 8; i++)
            {
                int randomIndex = rand.Next(0, universe.Length - 1);

                result = result + universe.Substring(randomIndex, 1);
            }

            return result;
        }

        public static string GenerateMarkup(string captchaInput)
        {
            StringBuilder html = new StringBuilder();

            html.AppendFormat("<img src=\"data:image/png;base64,{0}\" />", captchaInput);

            return html.ToString();
        }

        public string GetCaptcha()
        {
            string retVal = null;

            using (Bitmap bmp = new Bitmap(this.width, this.height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(240, 240, 240));
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (Font fo = new Font("Tahoma", this.fontSize, FontStyle.Bold ^ FontStyle.Italic))
                    {
                        // using (System.Drawing.Brush cfb = Brushes.CornflowerBlue) // Framework-Bug...
                        RectangleF rect = new RectangleF(8, 5, 0, 0);
                        g.DrawString(this.answer, fo, Brushes.CornflowerBlue, rect);
                    } // End Using fo 


                    Random rand = new Random();

                    for (int i = 0; i < 10; i++)
                    {
                        int x0 = rand.Next(0, this.width);
                        int y0 = rand.Next(0, this.height);
                        int x1 = rand.Next(0, this.width);
                        int y1 = rand.Next(0, this.height);

                        using (System.Drawing.Pen p = new Pen(Color.FromArgb(192, 75, 75, 75)))
                        {
                            g.DrawLine(p, x0, y0, x1, y1);
                        } // End Using p 

                    } // Next i 


                    for (int i = 0; i < 10; i++)
                    {
                        int x0 = rand.Next(0, this.width);
                        int y0 = rand.Next(0, this.height);
                        int x1 = rand.Next(0, this.width);
                        int y1 = rand.Next(0, this.height);

                        using (System.Drawing.Pen p = new Pen(Color.FromArgb(192, 125, 75, 255)))
                        {
                            g.DrawLine(p, x0, y0, x1, y1);
                        } // End Using p 

                    } // Next i 


                    using (MemoryStream mem = new MemoryStream())
                    {
                        bmp.Save(mem, ImageFormat.Png);
                        retVal = Convert.ToBase64String(mem.ToArray());
                    } // End Using ms 

                } // End Using g 

            } // End Using bmp 
            return retVal;
        }


    }
}
