
using System.Windows.Forms;


namespace Captcha
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            PasswordGenerator.Test();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        } // End Sub Main 


    } // End Class Program 


} // End Namespace Captcha 
