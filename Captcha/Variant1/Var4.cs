
// http://www.codeproject.com/Articles/5947/CAPTCHA-Image
namespace Captcha
{


    /// <summary>
    /// Summary description for CaptchaImage.
    /// </summary>
    public class Variant4
    {
        // Public properties (all read-only).
        public string Text
        {
            get { return this.text; }
        }
        public System.Drawing.Bitmap Image
        {
            get { return this.image; }
        }
        public int Width
        {
            get { return this.width; }
        }
        public int Height
        {
            get { return this.height; }
        }

        // Internal properties.
        private string text;
        private int width;
        private int height;
        private string familyName;
        private System.Drawing.Bitmap image;

        // For generating random numbers.
        private System.Random random = new System.Random();

        // ====================================================================
        // Initializes a new instance of the CaptchaImage class using the
        // specified text, width and height.
        // ====================================================================
        public Variant4(string s, int width, int height)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }

        // ====================================================================
        // Initializes a new instance of the CaptchaImage class using the
        // specified text, width, height and font family.
        // ====================================================================
        public Variant4(string s, int width, int height, string familyName)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.SetFamilyName(familyName);
            this.GenerateImage();
        }

        // ====================================================================
        // This member overrides Object.Finalize.
        // ====================================================================
        ~Variant4()
        {
            Dispose(false);
        }

        // ====================================================================
        // Releases all resources used by this object.
        // ====================================================================
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        // ====================================================================
        // Custom Dispose method to clean up unmanaged resources.
        // ====================================================================
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                this.image.Dispose();
        }

        // ====================================================================
        // Sets the image width and height.
        // ====================================================================
        private void SetDimensions(int width, int height)
        {
            // Check the width and height.
            if (width <= 0)
                throw new System.ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
            if (height <= 0)
                throw new System.ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
            this.width = width;
            this.height = height;
        }

        // ====================================================================
        // Sets the font used for the image text.
        // ====================================================================
        private void SetFamilyName(string familyName)
        {
            // If the named font is not installed, default to a system font.
            try
            {
                System.Drawing.Font font = new System.Drawing.Font(this.familyName, 12F);
                this.familyName = familyName;
                font.Dispose();
            }
            catch (System.Exception ex)
            {
                this.familyName = System.Drawing.FontFamily.GenericSerif.Name;
            }
        }

        // ====================================================================
        // Creates the bitmap image.
        // ====================================================================
        private void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(this.width, this.height
                , System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, this.width, this.height);

            // Fill in the background.
            System.Drawing.Drawing2D.HatchBrush hatchBrush = 
                new System.Drawing.Drawing2D.HatchBrush(
                    System.Drawing.Drawing2D.HatchStyle.SmallConfetti
                , System.Drawing.Color.LightGray
                , System.Drawing.Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            System.Drawing.SizeF size;
            float fontSize = rect.Height + 1;
            System.Drawing.Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new System.Drawing.Font(this.familyName, fontSize, System.Drawing.FontStyle.Bold);
                size = g.MeasureString(this.text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            format.Alignment = System.Drawing.StringAlignment.Center;
            format.LineAlignment = System.Drawing.StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddString(this.text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 4F;
            System.Drawing.PointF[] points =
			{
				new System.Drawing.PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new System.Drawing.PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new System.Drawing.PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
				new System.Drawing.PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
			};
            System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, System.Drawing.Drawing2D.WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new System.Drawing.Drawing2D.HatchBrush(
                System.Drawing.Drawing2D.HatchStyle.LargeConfetti
                , System.Drawing.Color.LightGray
                , System.Drawing.Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = System.Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = this.random.Next(rect.Width);
                int y = this.random.Next(rect.Height);
                int w = this.random.Next(m / 50);
                int h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            this.image = bitmap;
        }


    }


}
