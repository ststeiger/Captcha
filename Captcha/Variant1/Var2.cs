
namespace Captcha
{

    // https://github.com/dchest/captcha
    // https://stackoverflow.com/questions/10783127/how-to-implement-custom-audio-capcha-in-asp-net
    internal static partial class Variant2
    {


        internal static byte[] AudioCaptcha(this string text)
        {
            string en = "abcdefghijkoprstvx0123456789", Location = string.Concat(System.Web.HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString(), @"\bin\wav\");
            int dataLength = 0, length = 0, sampleRate = 0, plus = 37500, p = 0;
            System.Int16 bitsPerSample = 0, channels = 0;
            byte[] music, wav;
            System.Random r = new System.Random();
            p = r.Next(1, 4000000);
            p += (p % 150) + 44;
            byte[] rb = new byte[9 * plus];


            // read music
            using (System.IO.FileStream fs = new System.IO.FileStream(string.Format(Location + @"z{0}.wav", (r.Next() % 12) + 1)
                , System.IO.FileMode.Open
                , System.IO.FileAccess.Read
                , System.IO.FileShare.ReadWrite))
            {
                wav = new byte[44];
                fs.Read(wav, 0, 44);
                fs.Position = (long)p;
                fs.Read(rb, 0, rb.Length);
            }


            // make music
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                channels = System.BitConverter.ToInt16(wav, 22);
                sampleRate = System.BitConverter.ToInt32(wav, 24);
                bitsPerSample = System.BitConverter.ToInt16(wav, 34);
                length = rb.Length; dataLength = rb.Length;
                ms.Write(new byte[44], 0, 44); ms.Write(rb, 0, rb.Length);
                ms.Position = 0;
                using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms))
                {
                    bw.Write(new char[4] { 'R', 'I', 'F', 'F' }); bw.Write(length);
                    bw.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
                    bw.Write((int)16); bw.Write((System.Int16)1);
                    bw.Write(channels); bw.Write(sampleRate);
                    bw.Write((int)(sampleRate * ((bitsPerSample * channels) / 8)));
                    bw.Write((System.Int16)((bitsPerSample * channels) / 8));
                    bw.Write(bitsPerSample); bw.Write(new char[4] { 'd', 'a', 't', 'a' }); bw.Write(dataLength);
                    music = ms.ToArray();
                    p = 0;
                }
            }

            using (System.IO.MemoryStream final = new System.IO.MemoryStream())
            {
                final.Write(music, 0, 44);
                // make voice
                using (System.IO.MemoryStream msvoice = new System.IO.MemoryStream())
                {
                    msvoice.Write(new byte[plus / 2], 0, plus / 2);
                    length += plus; dataLength += plus / 2; p += plus / 2;
                    for (var i = 0; i < text.Length; i++)
                    {
                        string fn = string.Format(Location + @"{0}\{1}.wav", (r.Next() % 3), en.Substring(en.IndexOf(text.Substring(i, 1)), 1)).Replace("?", "qm");
                        wav = System.IO.File.ReadAllBytes(fn);
                        int size = System.BitConverter.ToInt32(wav, 4);
                        {
                            msvoice.Write(new byte[plus / 2], 0, plus / 2);
                            length += plus; dataLength += plus / 2; p += plus / 2;
                        }
                        msvoice.Write(wav, 44, wav.Length - 44);
                        length += size; dataLength += size - 36;
                    }
                    msvoice.Position = 0;
                    System.IO.MemoryStream msmusic = new System.IO.MemoryStream();
                    msmusic.Write(music, 0, music.Length);
                    msmusic.Position = 44;
                    //merge;
                    while (final.Length < msmusic.Length)
                        final.WriteByte((byte)(msvoice.ReadByte() - msmusic.ReadByte()));
                    return final.ToArray();
                }
            }
        }

        public static byte[] VisualCaptcha(string source)
        {
            try
            {
                System.Random r = new System.Random();
                int w = 250, h = 75;
                string family = "Arial Rounded MT Bold";
                using (var bmp = new System.Drawing.Bitmap(w, h
                    , System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    int m = 0, nm = 0;
                    System.Drawing.Color tc;
                    using (var g = System.Drawing.Graphics.FromImage(bmp))
                    {
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.Clear(System.Drawing.Color.White);
                        System.Drawing.SizeF size;
                        m = r.Next() % 9 + 1;
                        nm = r.Next() % 3;
                        tc = System.Drawing.Color.FromArgb(255, 255, 255);
                        size = g.MeasureString(source
                            , new System.Drawing.Font(family, h * 1.2f, System.Drawing.FontStyle.Bold)
                            , new System.Drawing.SizeF(w * 1F, h * 1F));
                        using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                            new System.Drawing.Rectangle(0, 0, w, h)
                            , System.Drawing.Color.Black
                            , System.Drawing.Color.Black, 45, false))
                        {
                            System.Drawing.Drawing2D.ColorBlend blend = 
                                new System.Drawing.Drawing2D.ColorBlend(6);
                            for (var i = 0; i < 6; i++) { blend.Positions[i] = i * (1 / 5F); blend.Colors[i] = r.RandomColor(255, 64, 128); }
                            brush.InterpolationColors = blend;

                            for (int wave = 0; wave < 2; wave++)
                            {
                                int min = (15 + wave * 20);
                                System.Drawing.PointF[] pt = new System.Drawing.PointF[] {
                                      new System.Drawing.PointF(16f, (float)r.Next(min, min + 10))
                                    , new System.Drawing.PointF(240f, (float)r.Next(min + 10, min + 20))
                                };
                                System.Collections.Generic.List<System.Drawing.PointF> PointList = 
                                    new System.Collections.Generic.List<System.Drawing.PointF>();

                                float curDist = 0, distance = 0;
                                for (int i = 0; i < pt.Length - 1; i++)
                                {
                                    System.Drawing.PointF ptA = pt[i], ptB = pt[i + 1];
                                    float deltaX = ptB.X - ptA.X, deltaY = ptB.Y - ptA.Y;
                                    curDist = 0;
                                    distance = (float)System.Math.Sqrt(System.Math.Pow(deltaX, 2) + System.Math.Pow(deltaY, 2));
                                    while (curDist < distance)
                                    {
                                        curDist++;
                                        float offsetX = (float)((double)curDist / (double)distance * (double)deltaX);
                                        float offsetY = (float)((double)curDist / (double)distance * (double)deltaY);
                                        PointList.Add(new System.Drawing.PointF(ptA.X + offsetX, ptA.Y + offsetY));
                                    }
                                }
                                for (int i = 0; i < PointList.Count - 24; i = i + 24)
                                {
                                    float x1 = PointList[i].X, y1 = PointList[i].Y, x2 = PointList[i + 24].X, y2 = PointList[i + 24].Y;
                                    float angle = (float)((System.Math.Atan2(y2 - y1, x2 - x1) * 180 / 3.14159265));
                                    g.TranslateTransform(x1, y1);
                                    g.RotateTransform(angle);
                                    int pm = r.Next() % 2 + 1;
                                    System.Drawing.Point[] p1 = new System.Drawing.Point[] {
                                        new System.Drawing.Point(0, 0)
                                        , new System.Drawing.Point(3, -3 * pm)
                                        , new System.Drawing.Point(6, -4 * pm)
                                        , new System.Drawing.Point(9, -3 * pm)
                                        , new System.Drawing.Point(12, 0)
                                        , new System.Drawing.Point(15, 3 * pm)
                                        , new System.Drawing.Point(18, 4 * pm)
                                        , new System.Drawing.Point(21, 3 * pm)
                                        , new System.Drawing.Point(24, 0) };
                                    using (var path = new System.Drawing.Drawing2D.GraphicsPath()) g.DrawLines(new System.Drawing.Pen(brush, 2f), p1);
                                    g.RotateTransform(-angle);
                                    g.TranslateTransform(-x1, -y1);
                                }
                            }
                            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                            {
                                System.Drawing.PointF[] points = new System.Drawing.PointF[] { };
                                if (m == 1 || m == 2 || m == 3) // star trek inverse
                                {
                                    path.AddString(source
                                        , new System.Drawing.FontFamily(family), 1, h * 0.75F
                                        , new System.Drawing.PointF((w - size.Width) / 2F
                                        , (h * 0.9F - size.Height) / 2F)
                                        , System.Drawing.StringFormat.GenericTypographic);
                                    points = new System.Drawing.PointF[] {
                                        new System.Drawing.PointF(0, 0)
                                        , new System.Drawing.PointF(w, 0)
                                        , new System.Drawing.PointF(w * 0.2F, h)
                                        , new System.Drawing.PointF(w * 0.8F, h) };
                                }
                                else if (m == 4 || m == 5) // star trek
                                {
                                    path.AddString(source, new System.Drawing.FontFamily(family), 1, h * 0.75F
                                        , new System.Drawing.PointF((w - size.Width) / 2F
                                        , (h * 1.2F - size.Height) / 2F + 2F)
                                        , System.Drawing.StringFormat.GenericTypographic);
                                    points = new System.Drawing.PointF[] { new System.Drawing.PointF(w * 0.2F, 0)
                                        , new System.Drawing.PointF(w * 0.8F, 0)
                                        , new System.Drawing.PointF(0, h)
                                        , new System.Drawing.PointF(w, h) };
                                }
                                else if (m == 6 || m == 7) // grow from left
                                {
                                    path.AddString(source, new System.Drawing.FontFamily(family), 1, h * 0.75F
                                        , new System.Drawing.PointF((w * 1.15F - size.Width) / 2F
                                        , (h - size.Height) / 2F)
                                        , System.Drawing.StringFormat.GenericTypographic);
                                    points = new System.Drawing.PointF[] {
                                        new System.Drawing.PointF(0, h * 0.25F)
                                        , new System.Drawing.PointF(w, 0)
                                        , new System.Drawing.PointF(0, h * 0.75F)
                                        , new System.Drawing.PointF(w, h) };
                                }
                                else if (m == 8 || m == 9) // grow from right
                                {
                                    path.AddString(source, new System.Drawing.FontFamily(family), 1, h * 0.75F
                                        , new System.Drawing.PointF((w * 0.85F - size.Width) / 2F
                                        , (h - size.Height) / 2F)
                                        , System.Drawing.StringFormat.GenericTypographic);

                                    points = new System.Drawing.PointF[] {
                                        new System.Drawing.PointF(w * 0.1F, 0)
                                        , new System.Drawing.PointF(w * 0.9F, h * 0.25F)
                                        , new System.Drawing.PointF(w * 0.1F, h)
                                        , new System.Drawing.PointF(w * 0.9F, h * 0.75F) };
                                }
                                path.Warp(points, new System.Drawing.RectangleF(0, 0, w, h));
                                g.FillPath(System.Drawing.Brushes.White, path);
                                g.DrawPath(new System.Drawing.Pen(brush, 2F), path);
                            }
                        }
                    }
                    using (var thumb = new System.Drawing.Bitmap(128, 40
                        , System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(thumb))
                        {
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            System.Drawing.Rectangle tr = new System.Drawing.Rectangle(0, 0, thumb.Width, thumb.Height);
                            g.DrawImage(bmp, tr);
                            g.DrawRectangle(
                                new System.Drawing.Pen(System.Drawing.Brushes.White)
                                , new System.Drawing.Rectangle(0, 0, 127, 39));
                        }
                        using (var ms = new System.IO.MemoryStream())
                        {
                            ((System.Drawing.Image)thumb).Save(ms
                                , System.Drawing.Imaging.ImageFormat.Png);
                            return ms.ToArray();
                        }
                    }
                }
            }
            catch { return null; }
        }


        private static System.Drawing.Color RandomColor(this System.Random rnd, int alpha, int min, int max)
        {
            return System.Drawing.Color.FromArgb(
                alpha
                , rnd.Next(min, max)
                , rnd.Next(min, max)
                , rnd.Next(min, max)
            );
        }


    }


}