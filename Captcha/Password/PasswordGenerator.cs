
namespace Captcha
{

    public class PasswordGenerator
    {

        public static void Test()
        {
            // http://stackoverflow.com/questions/5750203/how-to-write-unicode-characters-to-the-console
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            for (int i = 0; i < 100; ++i)
            {
                // string pw = CreatePassword(8);
                // string pw = RandomPassword(new PasswordOptions());


                // string pw = RandomPassword(new CyrillicPasswordOptions());

                //string pw = RandomPassword(new GenericPasswordOptions()
                //{
                //    PASSWORD_CHARS_LCASE = "æaàâäbcçdeéêëfgiïîjklmnoœôöpqrstuùûüüvwxyÿz",
                //    PASSWORD_CHARS_UCASE = "ÆAÀÂÄBCÇDEÉÊËFGHIÏÎJKLMNOŒÔÖPQRSTUÙÛÜÜVWXYŸZ",
                //    PASSWORD_CHARS_NUMERIC = "0123456789",
                //    PASSWORD_CHARS_SPECIAL = "*-+=_&!?$€£%{}()[]/\\|.:;",
                //    NumberOfLowerCaseCharacters = 5,
                //    NumberOfUpperCaseCharacters = 1,
                //    NumberOfNumericCharacters = 3,
                //    NumberOfSpecialCharacters = 1
                //});


                string pw = RandomPassword(new SafePasswordOptions());
                
                System.Console.WriteLine(pw);
            }
            
        }


        // Randomly generate the desired number of lowercase, uppercase, numeric and special chars
        // Add them to char array 
        // Shuffle array randomly
        public static string RandomPassword(AbstractPasswordOptions options)
        {
            int maxLenLowerCaseChars = options.PASSWORD_CHARS_LCASE.Length;
            int maxLenUpperCaseChars = options.PASSWORD_CHARS_UCASE.Length;
            int maxLenNumericChars = options.PASSWORD_CHARS_NUMERIC.Length;
            int maxLenSpecialChars = options.PASSWORD_CHARS_SPECIAL.Length;


            int k = 0;

            char[] pwChars = new char[options.TotalPasswordLength];

            using (Captcha.Cryptography.CryptoRandom rnd = new Captcha.Cryptography.CryptoRandom())
            {

                for (int i = 0; i < options.NumberOfLowerCaseCharacters; ++i)
                {
                    char c = options.PASSWORD_CHARS_LCASE[rnd.Next(0, maxLenLowerCaseChars)];
                    pwChars[k] = c;
                    k++;
                }


                for (int i = 0; i < options.NumberOfUpperCaseCharacters; ++i)
                {
                    char c = options.PASSWORD_CHARS_UCASE[rnd.Next(0, maxLenUpperCaseChars)];
                    pwChars[k] = c;
                    k++;
                }


                for (int i = 0; i < options.NumberOfNumericCharacters; ++i)
                {
                    char c = options.PASSWORD_CHARS_NUMERIC[rnd.Next(0, maxLenNumericChars)];
                    pwChars[k] = c;
                    k++;
                }


                for (int i = 0; i < options.NumberOfSpecialCharacters; ++i)
                {
                    char c = options.PASSWORD_CHARS_SPECIAL[rnd.Next(0, maxLenSpecialChars)];
                    pwChars[k] = c;
                    k++;
                }

                Shuffle(pwChars, rnd);
            } // End Using rnd 

            return new string(pwChars);
        } // End Function RandomPassword 


        public static void Shuffle<T>(T[] list)
        {
            // Random rnd = new Random();
            using (Captcha.Cryptography.CryptoRandom rnd = new Captcha.Cryptography.CryptoRandom())
            {
                Shuffle(list, rnd);
            } // End Using rnd 

        } // End Sub Shuffle 


        public static void Shuffle<T>(T[] list, System.Random rnd)
        {
            int n = list.Length;

            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            } // Whend 

        } // End Sub Shuffle 


        public static void Shuffle<T>(System.Collections.Generic.IList<T> list)
        {
            // Random rnd = new Random();
            using (Captcha.Cryptography.CryptoRandom rnd = new Captcha.Cryptography.CryptoRandom())
            {
                Shuffle<T>(list, rnd);
            } // End Using rnd 

        } // End Sub Shuffle 


        public static void Shuffle<T>(System.Collections.Generic.IList<T> list, System.Random rnd)
        {
            int n = list.Count;

            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            } // Whend 

        } // End Sub Shuffle 


        public static string CreatePassword(int length)
        {
            System.Text.StringBuilder res = new System.Text.StringBuilder();
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            // System.Random rnd = new System.Random();
            using (Captcha.Cryptography.CryptoRandom rnd = new Captcha.Cryptography.CryptoRandom())
            {
                // cr.Next(10, 100);

                while (0 < length--)
                {
                    res.Append(valid[rnd.Next(valid.Length)]);
                }
            }
            return res.ToString();
        }


    }
}
