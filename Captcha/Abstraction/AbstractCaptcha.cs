
namespace Captcha
{


    public abstract class AbstractCaptcha
    {

        public string m_text;
        private System.Drawing.Imaging.ImageFormat m_imageFormat;


        public virtual string Text
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_text))
                    this.m_text = this.RandomToken;

                return this.m_text;
            }
            set { this.m_text = value; }
        }


        public virtual string RandomToken
        {
            get
            {
                System.Random r = new System.Random();
                string s = "";
                for (int j = 0; j < 5; j++)
                {
                    int i = r.Next(3);
                    int ch;
                    switch (i)
                    {
                        case 1:
                            ch = r.Next(0, 9);
                            s = s + ch.ToString();
                            break;
                        case 2:
                            ch = r.Next(65, 90);
                            s = s + System.Convert.ToChar(ch).ToString();
                            break;
                        case 3:
                            ch = r.Next(97, 122);
                            s = s + System.Convert.ToChar(ch).ToString();
                            break;
                        default:
                            ch = r.Next(97, 122);
                            s = s + System.Convert.ToChar(ch).ToString();
                            break;
                    }
                    r.NextDouble();
                    r.Next(100, 1999);
                }
                return s;
            }
        }


        public virtual System.Drawing.Imaging.ImageFormat Format
        {
            get
            {
                if (m_imageFormat == null)
                    m_imageFormat = System.Drawing.Imaging.ImageFormat.Png;

                return m_imageFormat;
            }
            set
            {
                this.m_imageFormat = value;
            }
        }


        private string m_MimeType;

        public virtual string MimeType
        {
            get
            {
                if (!string.IsNullOrEmpty(m_MimeType))
                {
                    return this.m_MimeType;
                }

                System.Guid formatUID = this.Format.Guid;

                System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                for (int i = 0; i < codecs.Length; ++i)
                {
                    if (codecs[i].FormatID == formatUID)
                    {
                        this.m_MimeType = codecs[i].MimeType;
                        return this.m_MimeType;
                    }

                }

                this.m_MimeType = "image/unknown";
                return this.m_MimeType;
            }
        }


        public abstract System.Drawing.Image Image
        {
            get;
            set;
        }



        public virtual byte[] ImageBytes
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

        public virtual string Base64()
        {
            return System.Convert.ToBase64String(this.ImageBytes);
        }


    }


}
