
namespace Captcha
{
    public class Variant6
    {
        private string _captchaText;
        private System.Random rand = new System.Random();
        
        private System.Drawing.Point[] GetRandomPoints()
        {
            System.Drawing.Point[] points = {
                new System.Drawing.Point(rand.Next(10, 150), rand.Next(10, 150))
                , new System.Drawing.Point(rand.Next(10, 100), rand.Next(10, 100)) };
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

            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(200, 150, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {

                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {


                    System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Yellow);
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 200, 150);
                    System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(System.Drawing.Color.DarkKhaki);
                    System.Drawing.SolidBrush blue = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

                    //pen.Dispose
                    // b.Dispose
                    // blue.Dispose


                    int counter = 0;

                    g.DrawRectangle(pen, rect);

                    g.FillRectangle(b, rect);

                    for (int i = 0; i < _captchaText.Length; i++)
                    {
                        g.DrawString(_captchaText[i].ToString()
                            , new System.Drawing.Font("Verdena", 10 + rand.Next(14, 18)), blue
                            , new System.Drawing.PointF(10 + counter, 10));
                        counter += 20;
                    }

                    DrawRandomLines(g);

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imageBytes = ms.ToArray();
                    } // End Using ms 

                } // End Using g 

            } // End Using bitmap 

            return imageBytes;
        }


        private void DrawRandomLines(System.Drawing.Graphics g)
        {
            using (System.Drawing.SolidBrush green = 
                new System.Drawing.SolidBrush(System.Drawing.Color.Blue))
            {
                for (int i = 0; i < 30; i++)
                {
                    using (System.Drawing.Pen p = new System.Drawing.Pen(green, 2))
                    {
                        g.DrawLines(p, GetRandomPoints());
                    }

                    
                }
            }
        }

    }
}
