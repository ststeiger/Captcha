
namespace Captcha
{


    public class Variant7
    {


        public static byte[] CreateImage(string code)
        {
            byte[] imageBytes = null;
            System.Random rand = new System.Random();
            
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(200, 50
                , System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Yellow))
                    {
                        using (System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
                        {
                            using (System.Drawing.SolidBrush White = new System.Drawing.SolidBrush(System.Drawing.Color.White))
                            {
                                System.Drawing.Rectangle rect = 
                                    new System.Drawing.Rectangle(0, 0, 200, 50);
                                int counter = 0;

                                g.DrawRectangle(pen, rect);
                                g.FillRectangle(b, rect);

                                for (int i = 0; i < code.Length; i++)
                                {
                                    g.DrawString(code[i].ToString()
                                        , new System.Drawing.Font("Georgia", 10 + rand.Next(14, 18))
                                        , White
                                        , new System.Drawing.PointF(10 + counter, 10));
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


        private static void DrawRandomLines(System.Drawing.Graphics g, System.Random rand)
        {
            using (System.Drawing.SolidBrush green = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
            {
                //For Creating Lines on The Captcha
                for (int i = 0; i < 20; i++)
                {
                    g.DrawLines(new System.Drawing.Pen(green, 2), GetRandomPoints(rand));
                }
            }
            
        }


        private static System.Drawing.Point[] GetRandomPoints(System.Random rand)
        {
            System.Drawing.Point[] points = {
                new System.Drawing.Point(rand.Next(10, 150), rand.Next(10, 150))
                    , new System.Drawing.Point(rand.Next(10, 100), rand.Next(10, 100)) };
            return points;
        }

        string code;
    }


}
