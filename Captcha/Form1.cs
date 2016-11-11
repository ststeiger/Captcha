using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Captcha
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public void Variant1()
        {
            string sCaptchaText = "abc123";
            byte[] captchaBytes = Captcha.Variant2.VisualCaptcha("someTest");
            this.pictureBox1.Image = Helpers.ByteArrayToImage(captchaBytes);
        }

        public void Variant2()
        {
            string sCaptchaText = "abc123";
            byte[] captchaBytes = Captcha.Variant1.GetCaptchaImage(sCaptchaText, System.Drawing.Imaging.ImageFormat.Png);
            this.pictureBox1.Image = Helpers.ByteArrayToImage(captchaBytes);
        }

        public void Variant3()
        {
            string CaptchaImageText = Helpers.GenerateRandomCode();
            // Create a CAPTCHA image using the text stored in the Session object.
            RandomImage ci = new RandomImage(CaptchaImageText, 300, 75);
            this.pictureBox1.Image = ci.Image;
        }

        public void Variant4()
        {
            string sCaptchaText = "abc123";
            Variant4 ci = new Variant4(sCaptchaText, 200, 50, "Century Schoolbook");
            this.pictureBox1.Image = ci.Image;
        }


        public void Variant5()
        {
            byte[] imageBytes = null;

            int nWidth = 500;
            int nHeight = 100;
            string sKeyword ="HELLO";
            // string sFontName
            // float fFontSize 

            Captcha.Variant5 cc = new Captcha.Variant5();

            cc.Width = nWidth;
            cc.Height = nHeight;

            // cc.FontName = sFontName;
            // cc.FontSize = fFontSize;
            cc.Keyword = sKeyword;

            using (Bitmap bmp = cc.makeCaptcha())
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageBytes = ms.ToArray();
                }
            
            }

            this.pictureBox1.Image = Helpers.ByteArrayToImage(imageBytes);
        }


        public void Variant6()
        {
            Variant6 var6 = new Variant6();
            byte[] captchaBytes = var6.CreateImage();
            this.pictureBox1.Image = Helpers.ByteArrayToImage(captchaBytes);
        }


        public void Variant7()
        {
            string sCaptchaText = "abc123";
            byte[] captchaBytes = Captcha.Variant7.CreateImage(sCaptchaText);
            this.pictureBox1.Image = Helpers.ByteArrayToImage(captchaBytes);
        }


        public void Variant8()
        {
            CaptchaImage ci = new CaptchaImage();
            ci.Width = 500;
            ci.Height = 100;
            this.pictureBox1.Image = ci.RenderImage();
        }


        public void Variant9()
        {
            Variant9 ci = new Variant9();
            ci.Width = 500;
            ci.Height = 100;
            this.pictureBox1.Image = ci.RenderImage();
        }


        public void Variant10()
        {
            string sCaptchaText = "abc123";
            RNCaptcha rn = new RNCaptcha(sCaptchaText);
            byte[] captchaBytes = System.Convert.FromBase64String(rn.GetCaptcha());
            this.pictureBox1.Image = Helpers.ByteArrayToImage(captchaBytes);
        }


        public void Variant11()
        {
            byte[] captchaBytes = Warping.GetWarpCaptcha();
            this.pictureBox1.Image = Helpers.ByteArrayToImage(captchaBytes);
        }


        private void btnVariant1_Click(object sender, EventArgs e)
        {
            // Variant1();
            // Variant2();
            // Variant3();
            // Variant4();
            // Variant5();
            // Variant6();
            // Variant7();
            // Variant8();
            // Variant9();
            // Variant10();
            Variant11();

            return;

            string sCaptchaText = "abc123";
            byte[] captchaBytes = Captcha.Variant7.CreateImage(sCaptchaText);
            this.pictureBox1.Image = Helpers.ByteArrayToImage(captchaBytes);

        }
    }
}
