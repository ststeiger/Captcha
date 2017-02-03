
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
            // PasswordGenerator.Test();

            string fileName = @"D:\username\Downloads\SNB-Logo-positiv-hochaufloesend.png";
            // fileName = @"D:\username\Documents\Visual Studio 2008\Projects\SNB_Berichte\SNB_Berichte\logo_text_schwarz_klein.jpg";
            // fileName = @"D:\username\Documents\Visual Studio 2008\Projects\SNB_Berichte\SNB_Berichte\SNB-Logo-positiv-hochaufloesend.png";
            fileName = @"D:\username\Documents\Visual Studio 2008\Projects\SNB_Berichte\SNB_Berichte\snb_bns.png";
            fileName = @"D:\username\Downloads\snb_bns_hoch.png";
            fileName = @"D:\username\Documents\Visual Studio 2008\Projects\SNB_Berichte\SNB_Berichte\snb_bns_hoch.png";

            using (System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(fileName))
            {
                using (System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(bitmap))
                {
                    //newBitmap.SetResolution(300, 300);
                    newBitmap.SetResolution(96, 96);
                    //newBitmap.SetResolution(1200, 1200);
                    newBitmap.Save("snb_bns_96.png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        } // End Sub Main 


    } // End Class Program 


} // End Namespace Captcha 
