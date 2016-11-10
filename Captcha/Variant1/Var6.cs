
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Captcha
{
    public class Variant6
    {
        private string _captchaText;
        private Random rand = new Random();
        
        private Point[] GetRandomPoints()
        {
            Point[] points = { new Point(rand.Next(10, 150), rand.Next(10, 150)), new Point(rand.Next(10, 100), rand.Next(10, 100)) };
            return points;
        }

        public string CaptchaText
        {
            get { return _captchaText;  }
        }

        public byte[] CreateImage()
        {
            string _captchaText = Helpers.GetRandomText();
            return CreateImage(_captchaText);
        }


        public byte[] CreateImage(string _captchaText)
        {
            byte[] imageBytes = null;

            using (Bitmap bitmap = new Bitmap(200, 150, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {

                using (Graphics g = Graphics.FromImage(bitmap))
                {


                    Pen pen = new Pen(Color.Yellow);
                    Rectangle rect = new Rectangle(0, 0, 200, 150);
                    SolidBrush b = new SolidBrush(Color.DarkKhaki);
                    SolidBrush blue = new SolidBrush(Color.Blue);

                    //pen.Dispose
                    // b.Dispose
                    // blue.Dispose


                    int counter = 0;

                    g.DrawRectangle(pen, rect);

                    g.FillRectangle(b, rect);

                    for (int i = 0; i < _captchaText.Length; i++)
                    {
                        g.DrawString(_captchaText[i].ToString(), new Font("Verdena", 10 + rand.Next(14, 18)), blue, new PointF(10 + counter, 10));
                        counter += 20;
                    }

                    DrawRandomLines(g);

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        imageBytes = ms.ToArray();
                    } // End Using ms 

                } // End Using g 

            } // End Using bitmap 

            return imageBytes;
        }


        private void DrawRandomLines(Graphics g)
        {
            using (SolidBrush green = new SolidBrush(Color.Blue))
            {
                for (int i = 0; i < 30; i++)
                {
                    using (Pen p = new Pen(green, 2))
                    {
                        g.DrawLines(p, GetRandomPoints());
                    }

                    
                }
            }
        }

    }
}
