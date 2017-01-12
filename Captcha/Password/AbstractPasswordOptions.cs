
namespace Captcha
{


    public class AbstractPasswordOptions
    {

        Cryptography.CryptoRandom m_Seed;

        public virtual Cryptography.CryptoRandom RandomNumberGenerator
        {
            get
            {
                if (m_Seed != null && m_Seed.Disposed)  m_Seed = null;

                if (m_Seed == null)
                    m_Seed = new Cryptography.CryptoRandom();

                return this.m_Seed;
            }
            set
            {
                this.m_Seed = value;
            }
        }


        public virtual string PASSWORD_CHARS_LCASE
        {
            get;
            set;
        }

        public virtual string PASSWORD_CHARS_UCASE
        {
            get;
            set;
        }

        public virtual string PASSWORD_CHARS_NUMERIC
        {
            get;
            set;
        }

        public virtual string PASSWORD_CHARS_SPECIAL
        {
            get;
            set;
        }

        public virtual int NumberOfLowerCaseCharacters
        {
            get;
            set;
        }

        public virtual int NumberOfUpperCaseCharacters
        {
            get;
            set;
        }

        public virtual int NumberOfNumericCharacters
        {
            get;
            set;
        }

        public virtual int NumberOfSpecialCharacters
        {
            get;
            set;
        }

        public virtual int TotalPasswordLength
        {
            get
            {
                return this.NumberOfLowerCaseCharacters
                    + this.NumberOfUpperCaseCharacters
                    + this.NumberOfNumericCharacters
                    + this.NumberOfSpecialCharacters;
            }
        }

    }


    public class GenericPasswordOptions : AbstractPasswordOptions
    {

    } // End Class GenericPasswordOptions 


    public class SafePasswordOptions : AbstractPasswordOptions
    {

        public SafePasswordOptions()
        {
            this.PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";   
            this.PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            this.PASSWORD_CHARS_NUMERIC = "23456789";
            this.PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}";

            this.NumberOfLowerCaseCharacters = 5;
            this.NumberOfUpperCaseCharacters = 1;
            this.NumberOfNumericCharacters = 1;
            this.NumberOfSpecialCharacters = 1;
        } // End Constructor 

    } // End Class SafePasswordOptions 


    public class EnglishPasswordOptions : AbstractPasswordOptions
    {

        public EnglishPasswordOptions()
        {
            // this.PASSWORD_CHARS_LCASE = "abcdefghijkmnopqrstwxyz";
            this.PASSWORD_CHARS_LCASE = "abcdefghijklmnopqrstuvwxyz";

            // this.PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            this.PASSWORD_CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // this.PASSWORD_CHARS_NUMERIC = "23456789";
            this.PASSWORD_CHARS_NUMERIC = "0123456789";

            // this.PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
            this.PASSWORD_CHARS_SPECIAL = "*-+=_&!?$%{}()[]/\\|.:;";


            this.NumberOfLowerCaseCharacters = 5;
            this.NumberOfUpperCaseCharacters = 1;
            this.NumberOfNumericCharacters = 3;
            this.NumberOfSpecialCharacters = 1;
        } // End Constructor 

    } // End Class EnglishPasswordOptions 


    public class CyrillicPasswordOptions : AbstractPasswordOptions
    {

        public CyrillicPasswordOptions()
        {
            // this.PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
            this.PASSWORD_CHARS_LCASE = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            // this.PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            this.PASSWORD_CHARS_UCASE = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

            // this.PASSWORD_CHARS_NUMERIC = "23456789";
            this.PASSWORD_CHARS_NUMERIC = "0123456789";

            // this.PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
            this.PASSWORD_CHARS_SPECIAL = "*-+=_&!?$%{}()[]/\\|.:;";


            this.NumberOfLowerCaseCharacters = 5;
            this.NumberOfUpperCaseCharacters = 1;
            this.NumberOfNumericCharacters = 3;
            this.NumberOfSpecialCharacters = 1;
        } // End Constructor 

    } // End Class CyrillicPasswordOptions 


    public class GermanPasswordOptions : AbstractPasswordOptions
    {

        public GermanPasswordOptions()
        {
            // PASSWORD_CHARS_LCASE = "aäbcdefgiïjklmnoöpqrsßtuüvwxyz";
            // PASSWORD_CHARS_UCASE = "AÄBCDEFGHIÏJKLMNOÖPQRSTUÜVWXYZ";

            PASSWORD_CHARS_LCASE = "aäbcdefghijklmnoöpqrsßtuüvwxyz";
            PASSWORD_CHARS_UCASE = "AÄBCDEFGHIJKLMNOÖPQRSTUÜVWXYZ";

            PASSWORD_CHARS_NUMERIC = "0123456789";
            PASSWORD_CHARS_SPECIAL = "*-+=_&!?$€£%{}()[]/\\|.:;";
            NumberOfLowerCaseCharacters = 5;
            NumberOfUpperCaseCharacters = 1;
            NumberOfNumericCharacters = 3;
            NumberOfSpecialCharacters = 1;
        } // End Constructor 

    } // End Class GermanPasswordOptions 


    public class SwissPasswordOptions : AbstractPasswordOptions
    {

        public SwissPasswordOptions()
        {
            PASSWORD_CHARS_LCASE = "abcdefghijklmnopqrstuvwxyz";
            PASSWORD_CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            PASSWORD_CHARS_NUMERIC = "0123456789";
            PASSWORD_CHARS_SPECIAL = "*-+=_&!?$€£%{}()[]/\\|.:;";
            NumberOfLowerCaseCharacters = 5;
            NumberOfUpperCaseCharacters = 1;
            NumberOfNumericCharacters = 3;
            NumberOfSpecialCharacters = 1;
        } // End Constructor 

    } // End Class SwissPasswordOptions 


    public class SessionUidOptions : AbstractPasswordOptions
    {

        public SessionUidOptions()
        {
            this.PASSWORD_CHARS_LCASE = "abcdefghijklmnopqrstuvwxyz";
            this.PASSWORD_CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.PASSWORD_CHARS_NUMERIC = "0123456789";
            this.PASSWORD_CHARS_SPECIAL = "";


            this.NumberOfNumericCharacters = this.RandomNumberGenerator.Next(0, 10);
            this.NumberOfLowerCaseCharacters = 24 - this.NumberOfNumericCharacters;
            this.NumberOfUpperCaseCharacters = 0;
            this.NumberOfSpecialCharacters = 0;
        } // End Constructor 

    } // End Class EnglishPasswordOptions 


} // End Namespace Captcha 
