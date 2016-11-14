using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace Captcha
{


    // http://www.codeproject.com/Articles/8751/A-CAPTCHA-Server-Control-for-ASP-NET


    /// <summary>
    /// CAPTCHA image generation class
    /// </summary>
    /// <remarks>
    /// Adapted from the excellent code at 
    /// http://www.codeproject.com/aspnet/CaptchaImage.asp
    ///
    /// Jeff Atwood
    /// http://www.codinghorror.com/
    /// </remarks>
    public class Variant12
    {

        private int m_height;
        private int m_width;
        private Random m_rand;
        private DateTime m_generatedAt;
        private string m_randomText;
        private int m_randomTextLength;
        private string m_randomTextChars;
        private string m_fontFamilyName;
        private FontWarpFactor m_fontWarp;
        private BackgroundNoiseLevel m_backgroundNoise;
        private LineNoiseLevel m_lineNoise;
        private string m_guid;

        private string m_fontWhitelist;

        /// <summary>
        /// Amount of random font warping to apply to rendered text
        /// </summary>
        public enum FontWarpFactor
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        /// <summary>
        /// Amount of background noise to add to rendered image
        /// </summary>
        public enum BackgroundNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        /// <summary>
        /// Amount of curved line noise to add to rendered image
        /// </summary>
        public enum LineNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }





        /// <summary>
        /// Returns a GUID that uniquely identifies this Captcha
        /// </summary>
        public string UniqueId
        {
            get { return m_guid; }
        }


        /// <summary>
        /// Returns the date and time this image was last rendered
        /// </summary>
        public DateTime RenderedAt
        {
            get { return m_generatedAt; }
        }


        /// <summary>
        /// Font family to use when drawing the Captcha text. If no font is provided, a random font will be chosen from the font whitelist for each character.
        /// </summary>
        public string Font
        {
            get { return m_fontFamilyName; }
            set
            {
                try
                {
                    Font font1 = new Font(value, 12f);
                    m_fontFamilyName = value;
                    font1.Dispose();
                }
                catch (Exception ex)
                {
                    m_fontFamilyName = System.Drawing.FontFamily.GenericSerif.Name;
                }
            }
        }


        /// <summary>
        /// Amount of random warping to apply to the Captcha text.
        /// </summary>
        public FontWarpFactor FontWarp
        {
            get { return m_fontWarp; }
            set { m_fontWarp = value; }
        }


        /// <summary>
        /// Amount of background noise to apply to the Captcha image.
        /// </summary>
        public BackgroundNoiseLevel BackgroundNoise
        {
            get { return m_backgroundNoise; }
            set { m_backgroundNoise = value; }
        }


        public LineNoiseLevel LineNoise
        {
            get { return m_lineNoise; }
            set { m_lineNoise = value; }
        }


        /// <summary>
        /// A string of valid characters to use in the Captcha text. 
        /// A random character will be selected from this string for each character.
        /// </summary>
        public string TextChars
        {
            get { return m_randomTextChars; }
            set
            {
                m_randomTextChars = value;
                m_randomText = GenerateRandomText();
            }
        }


        /// <summary>
        /// Number of characters to use in the Captcha text. 
        /// </summary>
        public int TextLength
        {
            get { return m_randomTextLength; }
            set
            {
                m_randomTextLength = value;
                m_randomText = GenerateRandomText();
            }
        }


        /// <summary>
        /// Returns the randomly generated Captcha text.
        /// </summary>
        public string Text
        {
            get { return m_randomText; }
        }


        /// <summary>
        /// Width of Captcha image to generate, in pixels 
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set
            {
                if ((value <= 60))
                {
                    throw new ArgumentOutOfRangeException("width", value, "width must be greater than 60.");
                }
                m_width = value;
            }
        }


        /// <summary>
        /// Height of Captcha image to generate, in pixels 
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set
            {
                if (value <= 30)
                {
                    throw new ArgumentOutOfRangeException("height", value, "height must be greater than 30.");
                }
                m_height = value;
            }
        }


        /// <summary>
        /// A semicolon-delimited list of valid fonts to use when no font is provided.
        /// </summary>
        public string FontWhitelist
        {
            get { return m_fontWhitelist; }
            set { m_fontWhitelist = value; }
        }



        public Variant12()
        {
            m_rand = new Random();
            m_fontWarp = FontWarpFactor.Low;
            m_backgroundNoise = BackgroundNoiseLevel.Low;
            m_lineNoise = LineNoiseLevel.None;
            m_width = 180;
            m_height = 50;
            m_randomTextLength = 5;
            m_randomTextChars = "ACDEFGHJKLNPQRTUVXYZ2346789";
            m_fontFamilyName = "";
            // -- a list of known good fonts in on both Windows XP and Windows Server 2003
            m_fontWhitelist = "arial;arial black;comic sans ms;courier new;estrangelo edessa;franklin gothic medium;" + "georgia;lucida console;lucida sans unicode;mangal;microsoft sans serif;palatino linotype;" + "sylfaen;tahoma;times new roman;trebuchet ms;verdana";
            m_randomText = GenerateRandomText();
            m_generatedAt = DateTime.Now;
            m_guid = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        public Bitmap RenderImage()
        {
            return GenerateImagePrivate();
        }


        /// <summary>
        /// Returns a random font family from the font whitelist
        /// </summary>
        string[] static_RandomFontFamily_ff;
        private string RandomFontFamily()
        {
            //-- small optimization so we don't have to split for each char
            if (static_RandomFontFamily_ff == null)
            {
                static_RandomFontFamily_ff = m_fontWhitelist.Split(';');
            }
            return static_RandomFontFamily_ff[m_rand.Next(0, static_RandomFontFamily_ff.Length)];
        }


        /// <summary>
        /// generate random text for the CAPTCHA
        /// </summary>
        private string GenerateRandomText()
        {
            string retVal = null;

            System.Text.StringBuilder sb = new System.Text.StringBuilder(m_randomTextLength);
            int maxLength = m_randomTextChars.Length;
            for (int n = 0; n <= m_randomTextLength - 1; n++)
            {
                sb.Append(m_randomTextChars.Substring(m_rand.Next(maxLength), 1));
            }

            retVal = sb.ToString();
            sb.Length = 0;
            sb = null;

            return retVal;
        }


        /// <summary>
        /// Returns a random point within the specified x and y ranges
        /// </summary>
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(m_rand.Next(xmin, xmax), m_rand.Next(ymin, ymax));
        }


        /// <summary>
        /// Returns a random point within the specified rectangle
        /// </summary>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }


        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font
        /// </summary>
        private GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, Convert.ToInt32(f.Style), f.Size, r, sf);
            return gp;
        }


        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size 
        /// </summary>
        private Font GetFont()
        {
            float fsize = 0;
            string fname = m_fontFamilyName;

            if (string.IsNullOrEmpty(fname))
            {
                fname = RandomFontFamily();
            }

            switch (this.FontWarp)
            {
                case FontWarpFactor.None:
                    fsize = Convert.ToInt32(m_height * 0.7f);
                    break;
                case FontWarpFactor.Low:
                    fsize = Convert.ToInt32(m_height * 0.8f);
                    break;
                case FontWarpFactor.Medium:
                    fsize = Convert.ToInt32(m_height * 0.85f);
                    break;
                case FontWarpFactor.High:
                    fsize = Convert.ToInt32(m_height * 0.9f);
                    break;
                case FontWarpFactor.Extreme:
                    fsize = Convert.ToInt32(m_height * 0.95f);
                    break;
            }

            return new Font(fname, fsize, FontStyle.Bold);
        }


        /// <summary>
        /// Renders the CAPTCHA image
        /// </summary>
        private Bitmap GenerateImagePrivate()
        {
            Font fnt = null;
            Rectangle rect = default(Rectangle);
            Brush br = default(Brush);
            Bitmap bmp = new Bitmap(m_width, m_height, PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            //-- fill an empty white rectangle
            rect = new Rectangle(0, 0, m_width, m_height);
            br = new SolidBrush(Color.White);
            gr.FillRectangle(br, rect);

            int charOffset = 0;
            double charWidth = m_width / m_randomTextLength;
            Rectangle rectChar = default(Rectangle);

            foreach (char c in m_randomText)
            {
                //-- establish font and draw area
                fnt = GetFont();
                rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), m_height);

                //-- warp the character
                GraphicsPath gp = TextPath(c.ToString(), fnt, rectChar);
                WarpText(gp, rectChar);

                //-- draw the character
                br = new SolidBrush(Color.Black);
                gr.FillPath(br, gp);

                charOffset += 1;
            }

            AddNoise(gr, rect);
            AddLine(gr, rect);

            //-- clean up unmanaged resources
            fnt.Dispose();
            br.Dispose();
            gr.Dispose();

            return bmp;
        }


        /// <summary>
        /// Warp the provided text GraphicsPath by a variable amount
        /// </summary>
        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            float WarpDivisor = 0;
            float RangeModifier = 0;

            switch (m_fontWarp)
            {
                case FontWarpFactor.None:
                    return;
                case FontWarpFactor.Low:
                    WarpDivisor = 6;
                    RangeModifier = 1;
                    break;
                case FontWarpFactor.Medium:
                    WarpDivisor = 5;
                    RangeModifier = 1.3f;
                    break;
                case FontWarpFactor.High:
                    WarpDivisor = 4.5f;
                    RangeModifier = 1.4f;
                    break;
                case FontWarpFactor.Extreme:
                    WarpDivisor = 4;
                    RangeModifier = 1.5f;
                    break;
            }

            RectangleF rectF = default(RectangleF);
            rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            int hrange = Convert.ToInt32(rect.Height / WarpDivisor);
            int wrange = Convert.ToInt32(rect.Width / WarpDivisor);
            int left = rect.Left - Convert.ToInt32(wrange * RangeModifier);
            int top = rect.Top - Convert.ToInt32(hrange * RangeModifier);
            int width = rect.Left + rect.Width + Convert.ToInt32(wrange * RangeModifier);
            int height = rect.Top + rect.Height + Convert.ToInt32(hrange * RangeModifier);

            if (left < 0)
                left = 0;
            if (top < 0)
                top = 0;
            if (width > this.Width)
                width = this.Width;
            if (height > this.Height)
                height = this.Height;

            PointF leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            PointF rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            PointF leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            PointF rightBottom = RandomPoint(width - wrange, width, height - hrange, height);

            PointF[] points = new PointF[] {
			leftTop,
			rightTop,
			leftBottom,
			rightBottom
		};
            Matrix m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }


        /// <summary>
        /// Add a variable level of graphic noise to the image
        /// </summary>
        private void AddNoise(Graphics graphics1, Rectangle rect)
        {
            int density = 0;
            int size = 0;

            switch (m_backgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    return;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
            }

            SolidBrush br = new SolidBrush(Color.Black);
            int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

            for (int i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
            {
                graphics1.FillEllipse(br, m_rand.Next(rect.Width), m_rand.Next(rect.Height), m_rand.Next(max), m_rand.Next(max));
            }
            br.Dispose();
        }


        /// <summary>
        /// Add variable level of curved lines to the image
        /// </summary>

        private void AddLine(Graphics graphics1, Rectangle rect)
        {
            int length = 0;
            float width = 0;
            int linecount = 0;

            switch (m_lineNoise)
            {
                case LineNoiseLevel.None:
                    return;
                case LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle(m_height / 31.25f);
                    // 1.6
                    linecount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle(m_height / 27.7777f);
                    // 1.8
                    linecount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle(m_height / 25f);
                    // 2.0
                    linecount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle(m_height / 22.7272f);
                    // 2.2
                    linecount = 3;
                    break;
            }

            PointF[] pf = new PointF[length + 1];
            Pen p = new Pen(Color.Black, width);

            for (int l = 1; l <= linecount; l++)
            {
                for (int i = 0; i <= length; i++)
                {
                    pf[i] = RandomPoint(rect);
                }
                graphics1.DrawCurve(p, pf, 1.75f);
            }

            p.Dispose();
        }


    } // End Class


} // End Namespace 
