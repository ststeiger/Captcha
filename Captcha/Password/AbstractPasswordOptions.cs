
namespace Captcha
{


    public class AbstractPasswordOptions
    {

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
            // this.PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
            this.PASSWORD_CHARS_LCASE = "abcdefgijklmnopqrstuvwxyz";

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
            PASSWORD_CHARS_LCASE = "aäbcdefgiïjklmnoöpqrsßtuüvwxyz";
            PASSWORD_CHARS_UCASE = "AÄBCDEFGHIÏJKLMNOÖPQRSTUÜVWXYZ";
            PASSWORD_CHARS_NUMERIC = "0123456789";
            PASSWORD_CHARS_SPECIAL = "*-+=_&!?$€£%{}()[]/\\|.:;";
            NumberOfLowerCaseCharacters = 5;
            NumberOfUpperCaseCharacters = 1;
            NumberOfNumericCharacters = 3;
            NumberOfSpecialCharacters = 1;
        } // End Constructor 

    } // End Class GermanPasswordOptions 


} // End Namespace Captcha 
