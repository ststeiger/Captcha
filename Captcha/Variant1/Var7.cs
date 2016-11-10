
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace Captcha
{


    public class Variant7
    {


        public static byte[] CreateImage(string code)
        {
            byte[] imageBytes = null;
            Random rand = new Random();
            
            using (Bitmap bitmap = new Bitmap(200, 50, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Yellow))
                    {
                        using (SolidBrush b = new SolidBrush(Color.Black))
                        {
                            using (SolidBrush White = new SolidBrush(Color.White))
                            {
                                Rectangle rect = new Rectangle(0, 0, 200, 50);
                                int counter = 0;

                                g.DrawRectangle(pen, rect);
                                g.FillRectangle(b, rect);

                                for (int i = 0; i < code.Length; i++)
                                {
                                    g.DrawString(code[i].ToString(), new Font("Georgia", 10 + rand.Next(14, 18)), White, new PointF(10 + counter, 10));
                                    counter += 20;
                                }

                                DrawRandomLines(g, rand);

                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    imageBytes = ms.ToArray();
                                } // End Using ms 
                            } // End Using White
                        } // End Using b
                    } // End Using pen 

                } // End Using g
            } // End Using bitmap 

            return imageBytes;
        }


        private static void DrawRandomLines(Graphics g, Random rand)
        {
            using (SolidBrush green = new SolidBrush(Color.Green))
            {
                //For Creating Lines on The Captcha
                for (int i = 0; i < 20; i++)
                {
                    g.DrawLines(new Pen(green, 2), GetRandomPoints(rand));
                }
            }
            

        }


        private static Point[] GetRandomPoints(Random rand)
        {
            Point[] points = { new Point(rand.Next(10, 150), rand.Next(10, 150)), new Point(rand.Next(10, 100), rand.Next(10, 100)) };
            return points;
        }

        string code;
     
    }
}
