
using System.Web;
using System.Web.SessionState;



namespace Captcha
{

    // http://www.codeproject.com/Articles/99148/Simple-CAPTCHA-Create-your-own-in-C
    // Very nice, but maybe you should update the examples because the Random class uses the max value as EXCLUSIVE upper bound (the lower bound is inclusive). 
    // Btw. a very common error, but I have never seen a Random implementation in any language where this wasn't the case, so I assume this is a "old" convention.
    //  Must use without -1 :
// Next(aHatchStyles.Length)
// Next(aFontEmSizes.Length)
    // Return Value
    // A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not MaxValue. If minValue equals maxValue, minValue is returned.
    public class Variant1 : IHttpHandler, IReadOnlySessionState
    {


        public void ProcessRequest(HttpContext context)
        {
            //Get Captcha in Session
            string sCaptchaText = context.Session["Captcha"].ToString();

            context.Response.Clear();
            context.Response.Headers.Clear();
            context.Response.ClearContent();

            context.Response.ContentType = "image/png";


            byte[] captchaBytes = GetCaptchaImage(sCaptchaText, System.Drawing.Imaging.ImageFormat.Png);
            context.Response.BinaryWrite(captchaBytes);
            
            // context.Response.End();
            context.Response.Flush(); // Sends all currently buffered output to the client.
            context.Response.SuppressContent = true; // Gets or sets a value indicating whether to send HTTP content to the client.
            context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }


        public static byte[] GetCaptchaImage(string sCaptchaText, System.Drawing.Imaging.ImageFormat format)
        {
            int iHeight = 80;
            int iWidth = 190;
            System.Random oRandom = new System.Random();

            int[] aBackgroundNoiseColor = new int[] { 150, 150, 150 };
            int[] aTextColor = new int[] { 0, 0, 0 };
            int[] aFontEmSizes = new int[] { 15, 20, 25, 30, 35 };

            string[] aFontNames = new string[]
            {
                 "Comic Sans MS",
                 "Arial",
                 "Times New Roman",
                 "Georgia",
                 "Verdana",
                 "Geneva"
            };

            System.Drawing.FontStyle[] aFontStyles = new System.Drawing.FontStyle[]
            {
                 System.Drawing.FontStyle.Bold,
                 System.Drawing.FontStyle.Italic,
                 System.Drawing.FontStyle.Regular,
                 System.Drawing.FontStyle.Strikeout,
                 System.Drawing.FontStyle.Underline
            };

            System.Drawing.Drawing2D.HatchStyle[] aHatchStyles = new System.Drawing.Drawing2D.HatchStyle[]
            {
                  System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.Cross
                , System.Drawing.Drawing2D.HatchStyle.DashedDownwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.DashedHorizontal
                , System.Drawing.Drawing2D.HatchStyle.DashedUpwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.DashedVertical
                , System.Drawing.Drawing2D.HatchStyle.DiagonalBrick
                , System.Drawing.Drawing2D.HatchStyle.DiagonalCross
                , System.Drawing.Drawing2D.HatchStyle.Divot
                , System.Drawing.Drawing2D.HatchStyle.DottedDiamond
                , System.Drawing.Drawing2D.HatchStyle.DottedGrid
                , System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.Horizontal
                , System.Drawing.Drawing2D.HatchStyle.HorizontalBrick
                , System.Drawing.Drawing2D.HatchStyle.LargeCheckerBoard
                , System.Drawing.Drawing2D.HatchStyle.LargeConfetti
                , System.Drawing.Drawing2D.HatchStyle.LargeGrid
                , System.Drawing.Drawing2D.HatchStyle.LightDownwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.LightHorizontal
                , System.Drawing.Drawing2D.HatchStyle.LightUpwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.LightVertical
                , System.Drawing.Drawing2D.HatchStyle.Max
                , System.Drawing.Drawing2D.HatchStyle.Min
                , System.Drawing.Drawing2D.HatchStyle.NarrowHorizontal
                , System.Drawing.Drawing2D.HatchStyle.NarrowVertical
                , System.Drawing.Drawing2D.HatchStyle.OutlinedDiamond
                , System.Drawing.Drawing2D.HatchStyle.Plaid
                , System.Drawing.Drawing2D.HatchStyle.Shingle
                , System.Drawing.Drawing2D.HatchStyle.SmallCheckerBoard
                , System.Drawing.Drawing2D.HatchStyle.SmallConfetti
                , System.Drawing.Drawing2D.HatchStyle.SmallGrid
                , System.Drawing.Drawing2D.HatchStyle.SolidDiamond
                , System.Drawing.Drawing2D.HatchStyle.Sphere
                , System.Drawing.Drawing2D.HatchStyle.Trellis
                , System.Drawing.Drawing2D.HatchStyle.Vertical
                , System.Drawing.Drawing2D.HatchStyle.Wave
                , System.Drawing.Drawing2D.HatchStyle.Weave
                , System.Drawing.Drawing2D.HatchStyle.WideDownwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.WideUpwardDiagonal
                , System.Drawing.Drawing2D.HatchStyle.ZigZag
            };



            //Creates an output Bitmap
            System.Drawing.Bitmap oOutputBitmap = new System.Drawing.Bitmap(iWidth, iHeight
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Graphics oGraphics = System.Drawing.Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //Create a Drawing area
            System.Drawing.RectangleF oRectangleF = 
                new System.Drawing.RectangleF(0, 0, iWidth, iHeight);
            System.Drawing.Brush oBrush = default(System.Drawing.Brush);


            //Draw background (Lighter colors RGB 100 to 255)
            oBrush = new System.Drawing.Drawing2D.HatchBrush(
                 aHatchStyles[oRandom.Next(aHatchStyles.Length - 1)]
                , System.Drawing.Color.FromArgb
                (
                     oRandom.Next(100, 255)
                    ,oRandom.Next(100, 255)
                    ,oRandom.Next(100, 255)
                )
                , System.Drawing.Color.White
            );

            oGraphics.FillRectangle(oBrush, oRectangleF);

            System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
            int i = 0;
            for (i = 0; i <= sCaptchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;
                int x = iWidth / (iChars + 1) * i;
                int y = iHeight / 2;

                //Rotate text Random
                oMatrix.RotateAt(oRandom.Next(-40, 40), new System.Drawing.PointF(x, y));
                oGraphics.Transform = oMatrix;

                //Draw the letters with Random Font Type, Size and Color
                oGraphics.DrawString
                (
                    //Text
                    sCaptchaText.Substring(i, 1),
                        //Random Font Name and Style
                    new System.Drawing.Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                       aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                       aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                        //Random Color (Darker colors RGB 0 to 100)
                    new System.Drawing.SolidBrush(
                        System.Drawing.Color.FromArgb(
                             oRandom.Next(0, 100)
                            ,oRandom.Next(0, 100)
                            ,oRandom.Next(0, 100)))
                            ,x
                            ,oRandom.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }

            byte[] captchaBytes = null;

            using (System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream())
            {
                oOutputBitmap.Save(oMemoryStream, format);
                captchaBytes = oMemoryStream.GetBuffer();
            } // End Using oMemoryStream 

            return captchaBytes;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }


}
