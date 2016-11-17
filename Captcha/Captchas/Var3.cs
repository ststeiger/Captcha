
// http://www.codeproject.com/Articles/169371/Captcha-Image-using-C-in-ASP-NET
public class RandomImage
{
    //Default Constructor 
    public RandomImage() { }


    //property
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


    //Private variable
    private string text;
    private int width;
    private int height;
    private System.Drawing.Bitmap image;
    private System.Random random = new System.Random();


    //Methods declaration
    public RandomImage(string s, int width, int height)
    {
        this.text = s;
        this.SetDimensions(width, height);
        this.GenerateImage();
    }


    public void Dispose()
    {
        System.GC.SuppressFinalize(this);
        this.Dispose(true);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            this.image.Dispose();
    }


    private void SetDimensions(int width, int height)
    {
        if (width <= 0)
            throw new System.ArgumentOutOfRangeException("width", width,
                "Argument out of range, must be greater than zero.");
        if (height <= 0)
            throw new System.ArgumentOutOfRangeException("height", height,
                "Argument out of range, must be greater than zero.");
        this.width = width;
        this.height = height;
    }


    private System.Drawing.Imaging.ImageFormat m_format;

    public System.Drawing.Imaging.ImageFormat Format
    {
        get
        {
            if (this.m_format == null)
            {
                this.m_format = System.Drawing.Imaging.ImageFormat.Png;
            }

            return this.m_format;
        }
        set
        {
            this.m_format = value;
        }
    }


    public byte[] Bytes
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
    

    private void GenerateImage()
    {
        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap
          (this.width, this.height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, this.width, this.height);
        System.Drawing.Drawing2D.HatchBrush hatchBrush = 
            new System.Drawing.Drawing2D.HatchBrush(
                System.Drawing.Drawing2D.HatchStyle.SmallConfetti,
            System.Drawing.Color.LightGray, System.Drawing.Color.White);
        g.FillRectangle(hatchBrush, rect);
        System.Drawing.SizeF size;
        float fontSize = rect.Height + 1;
        System.Drawing.Font font;

        do
        {
            fontSize--;
            font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, fontSize, System.Drawing.FontStyle.Bold);
            size = g.MeasureString(this.text, font);
        } while (size.Width > rect.Width);
        System.Drawing.StringFormat format = new System.Drawing.StringFormat();
        format.Alignment = System.Drawing.StringAlignment.Center;
        format.LineAlignment = System.Drawing.StringAlignment.Center;
        System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
        //path.AddString(this.text, font.FontFamily, (int) font.Style, 
        //    font.Size, rect, format);
        path.AddString(this.text, font.FontFamily, (int)font.Style, 75, rect, format);
        float v = 4F;
        System.Drawing.PointF[] points =
          {
                new System.Drawing.PointF(this.random.Next(rect.Width) / v, this.random.Next(
                   rect.Height) / v),
                new System.Drawing.PointF(rect.Width - this.random.Next(rect.Width) / v, 
                    this.random.Next(rect.Height) / v),
                new System.Drawing.PointF(this.random.Next(rect.Width) / v, 
                    rect.Height - this.random.Next(rect.Height) / v),
                new System.Drawing.PointF(rect.Width - this.random.Next(rect.Width) / v,
                    rect.Height - this.random.Next(rect.Height) / v)
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
            int x = this.random.Next(rect.Width);
            int y = this.random.Next(rect.Height);
            int w = this.random.Next(m / 50);
            int h = this.random.Next(m / 50);
            g.FillEllipse(hatchBrush, x, y, w, h);
        }
        font.Dispose();
        hatchBrush.Dispose();
        g.Dispose();
        this.image = bitmap;
    }


}