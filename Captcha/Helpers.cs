
namespace Captcha
{


    public class Helpers
    {

        public static string foo()
        {
            string token = null;

            using (System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                token = System.Convert.ToBase64String(tokenData);
            }

            return token;
        }



        public static string GetMimeType(System.Drawing.Image image)
        {
            System.Guid formatUID = image.RawFormat.Guid;

            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; ++i)
            {
                if (codecs[i].FormatID == formatUID)
                    return codecs[i].MimeType;
            }

            return "image/unknown";
        }


        public static string GetMimeType(System.Drawing.Imaging.ImageFormat format)
        {
            System.Guid formatUID = format.Guid;

            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; ++i)
            {
                if (codecs[i].FormatID == formatUID)
                    return codecs[i].MimeType;
            }

            return "image/unknown";
        }









        // Returns available fonts to use.
        public string GetFontNames()
        {
            string result = "";
            System.Drawing.FontFamily[] fntfamily = System.Drawing.FontFamily.Families;

            for (int i = 0; i < fntfamily.Length; i++)
            {
                result += "<fontname>" + fntfamily[i].Name.ToString() + "</fontname>";
            }

            return result;
        }


        //
        // Returns a string of six random digits.
        //
        public static string GenerateRandomNumber()
        {
            System.Random random = new System.Random();

            string s = "";
            for (int i = 0; i < 6; i++)
                s = string.Concat(s, random.Next(10).ToString());
            return s;
        }


        public static string GetRandomTextWithNumbers()
        {
            System.Text.StringBuilder randomText = new System.Text.StringBuilder();
            string alphabets = "abcdefghijklmnopqrstuvwxyz1234567890";

            System.Random r = new System.Random();
            for (int j = 0; j <= 5; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }

            string code = randomText.ToString();
            randomText.Length = 0;
            randomText = null;

            return code;
        }


        public static string GetRandomText()
        {
            System.Text.StringBuilder randomText = new System.Text.StringBuilder();
            string code = string.Empty;

            string alphabets = "abcdefghijklmnopqrstuvwxyz";

            System.Random r = new System.Random();

            for (int j = 0; j <= 5; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }

            code = randomText.ToString();
            return code;
        }


        // Function to generate random string with Random class.
        public static string GenerateRandomCode()
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


        public static System.Drawing.Image ByteArrayToImage(byte[] image)
        {
            System.Drawing.Image img = null;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(image))
            {
                img = System.Drawing.Image.FromStream(ms);
            } // End Using ms

            return img;
        }


    }


}
