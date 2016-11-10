using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captcha
{


    public class Helpers
    {



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
            Random random = new Random();

            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }


        private string GetRandomTextWithNumbers()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "abcdefghijklmnopqrstuvwxyz1234567890";

            Random r = new Random();
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
            StringBuilder randomText = new StringBuilder();
            string code = String.Empty;

            string alphabets = "abcdefghijklmnopqrstuvwxyz";

            Random r = new Random();

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
            Random r = new Random();
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
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
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
