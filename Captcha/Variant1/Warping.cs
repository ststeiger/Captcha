
namespace Captcha
{


    // http://www.planetclegg.com/projects/WarpingTextToSplines.html
    // http://www.csharpprogramming.tips/2013/08/warping-text-along-bezier-spline.html
    public class Warping
    {


        public static byte[] GetWarpCaptcha()
        {
            byte[] imageBytes = null;

            string text = "I can haz Peanut";
            System.Drawing.Drawing2D.GraphicsPath textPath = new System.Drawing.Drawing2D.GraphicsPath();

            float fontSize = 23;

            // textPath.AddString("st", null, 123, 12, )

            // the baseline should start at 0,0, so the next line is not quite correct
            textPath.AddString(text, System.Drawing.FontFamily.GenericSansSerif
                , (int)System.Drawing.FontStyle.Bold, fontSize
                , new System.Drawing.Point(0, 0)
                , System.Drawing.StringFormat.GenericTypographic);

            System.Drawing.RectangleF textBounds = textPath.GetBounds();


            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(500, 250))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                {
                    // GraphicsPath bezierPath = textPath;
                    // GraphicsPath bezierPath = BezierWarp(textPath, new System.Drawing.Size((int)textBounds.Width, (int)textBounds.Height));
                    System.Drawing.Drawing2D.GraphicsPath bezierPath = 
                        BezierWarp(textPath, new System.Drawing.Size(bmp.Width, bmp.Height));

                    // draw the transformed text path		
                    g.DrawPath(System.Drawing.Pens.Black, bezierPath);

                    // Draw the Bézier for reference
                    // g.DrawBezier(Pens.Black, P0, P1, P2, P3);
                } // End Using g 

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    imageBytes = ms.ToArray();
                } // End Using ms 

            } // End Using bmp 

            return imageBytes;
        } // End Function GetWarpCaptcha 


        public static System.Drawing.Drawing2D.GraphicsPath BezierWarp(
            System.Drawing.Drawing2D.GraphicsPath text, System.Drawing.Size size)
        {
            // Control points for a cubic Bézier spline
            System.Drawing.PointF P0 = new System.Drawing.PointF();
            System.Drawing.PointF P1 = new System.Drawing.PointF();
            System.Drawing.PointF P2 = new System.Drawing.PointF();
            System.Drawing.PointF P3 = new System.Drawing.PointF();

            float shrink = 20;
            float shift = 0;

            P0.X = shrink;
            P0.Y = shrink + shift;
            P1.X = size.Width - shrink;
            P1.Y = shrink;
            P2.X = shrink;
            P2.Y = size.Height - shrink;
            P3.X = size.Width - shrink;
            P3.Y = size.Height - shrink - shift;

            // Calculate coefficients A thru H from the control points
            float A = P3.X - 3 * P2.X + 3 * P1.X - P0.X;
            float B = 3 * P2.X - 6 * P1.X + 3 * P0.X;
            float C = 3 * P1.X - 3 * P0.X;
            float D = P0.X;

            float E = P3.Y - 3 * P2.Y + 3 * P1.Y - P0.Y;
            float F = 3 * P2.Y - 6 * P1.Y + 3 * P0.Y;
            float G = 3 * P1.Y - 3 * P0.Y;
            float H = P0.Y;

            System.Drawing.PointF[] pathPoints = text.PathPoints;
            System.Drawing.RectangleF textBounds = text.GetBounds();

            for (int i = 0; i < pathPoints.Length; i++)
            {
                System.Drawing.PointF pt = pathPoints[i];
                float textX = pt.X;
                float textY = pt.Y;

                // Normalize the x coordinate into the parameterized
                // value with a domain between 0 and 1.
                float t = textX / textBounds.Width;
                float t2 = (t * t);
                float t3 = (t * t * t);

                // Calculate spline point for parameter t
                float Sx = A * t3 + B * t2 + C * t + D;
                float Sy = E * t3 + F * t2 + G * t + H;

                // Calculate the tangent vector for the point
                float Tx = 3 * A * t2 + 2 * B * t + C;
                float Ty = 3 * E * t2 + 2 * F * t + G;

                // Rotate 90 or 270 degrees to make it a perpendicular
                float Px = -Ty;
                float Py = Tx;

                // Normalize the perpendicular into a unit vector
                float magnitude = (float)System.Math.Sqrt((Px * Px) + (Py * Py));
                Px /= magnitude;
                Py /= magnitude;

                // Assume that input text point y coord is the "height" or
                // distance from the spline.  Multiply the perpendicular
                // vector with y. it becomes the new magnitude of the vector
                Px *= textY;
                Py *= textY;

                // Translate the spline point using the resultant vector
                float finalX = Px + Sx;
                float finalY = Py + Sy;

                pathPoints[i] = new System.Drawing.PointF(finalX, finalY);
            }

            return new System.Drawing.Drawing2D.GraphicsPath(pathPoints, text.PathTypes); 
        } 


    } 


} 
