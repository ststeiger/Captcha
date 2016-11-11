
//************************************************************************************
// WSCaptcha web service
// Copyright (C) 2004, Massimo Beatini
//
// This software is provided "as-is", without any express or implied warranty. In 
// no event will the authors be held liable for any damages arising from the use 
// of this software.
//
// Permission is granted to anyone to use this software for any purpose, including 
// commercial applications, and to alter it and redistribute it freely, subject to 
// the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim 
//    that you wrote the original software. If you use this software in a product, 
//    an acknowledgment in the product documentation would be appreciated but is 
//    not required.
//
// 2. Altered source versions must be plainly marked as such, and must not be 
//    misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.
//
//************************************************************************************


using CSharpFilters;


// http://www.codeproject.com/Articles/9178/The-Captcha-Web-Service
namespace Captcha
{


	/// <summary>
	/// Descrizione di riepilogo per Captcha.
	/// </summary>
	public class Variant5
	{

		private int img_width = 450;
		private int img_height = 200;

		private System.Drawing.Font img_font;
		private string img_fontname = "arial black";
		private float img_fontsize = 24;
		private System.Drawing.FontStyle img_fontstyle = System.Drawing.FontStyle.Italic;
		private System.Drawing.Brush img_fontcolor = System.Drawing.Brushes.Black;

		private short nWave = 15;
		private double fDegree = 0;
		private int nPoints = 8;

		private string sKeyword = "";


		#region Properties
		// img size
		public int Width
		{
			set 
			{
				img_width = value;
			}
		}

		public int Height
		{
			set 
			{
				img_height = value;
			}
		}

		// img font
		public string FontName
		{
			set
			{
				img_fontname = value;
			}
		}

		public float FontSize
		{
			set
			{
				img_fontsize = value;
			}
		}

		public System.Drawing.FontStyle FontStyle
		{
			set
			{
				img_fontstyle = value;
			}
		}

		public System.Drawing.Brush FontColor
		{
			set
			{
				img_fontcolor = value;
			}
		}


		//
		public short Wave
		{
			set
			{
				nWave = value;
			}
		}

		public double Degree
		{
			set
			{
				fDegree = value;
			}
		}

		// 
		public int Points
		{
			set
			{
				nPoints = value;
			}
		}

		//
		public string Keyword
		{
			set
			{
				sKeyword = value;
			}
		}

		#endregion


		///
		/// makeCaptcha
		///
		public System.Drawing.Bitmap makeCaptcha()
		{


			System.Random randomGenerator = new System.Random();

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img_width, img_height
                , System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, img_width, img_height);


            System.Drawing.StringFormat sFormat = new System.Drawing.StringFormat();
			sFormat.Alignment = System.Drawing.StringAlignment.Center;
			sFormat.LineAlignment = System.Drawing.StringAlignment.Center;

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);


            // Set up the text font.
            System.Drawing.SizeF size;
			float fontSize = img_fontsize + 1;
            System.Drawing.Font font;
		
			// try to use requested font, but
			// If the named font is not installed, default to a system font.
			try
			{
				font = new System.Drawing.Font(img_fontname, img_fontsize);
				font.Dispose();
			}
			catch (System.Exception ex)
			{
				img_fontname = System.Drawing.FontFamily.GenericSerif.Name;
			}

			
			// build a new string with space through the chars
			// i.e. keyword 'hello' become 'h e l l o '
			string tempKey = "";			

			for(int ii=0; ii< sKeyword.Length; ii++)
			{
				tempKey = string.Concat(tempKey, sKeyword[ii].ToString());
				tempKey = string.Concat(tempKey, " ");
			}

			// Adjust the font size until the text fits within the image.
			do
			{
				fontSize--;
				font = new System.Drawing.Font(img_fontname, fontSize,	img_fontstyle);
				size = g.MeasureString(tempKey, font);
			} while (size.Width > (0.8*bmp.Width));

			img_font = font;


			g.Clear(System.Drawing.Color.Silver); // blank the image
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // antialias objects

			// fill with a liner gradient
			// random colors
			g.FillRectangle(	
				new System.Drawing.Drawing2D.LinearGradientBrush(
					new System.Drawing.Point(0,0), 
					new System.Drawing.Point(bmp.Width,bmp.Height),
                    System.Drawing.Color.FromArgb(
						255, //randomGenerator.Next(255),
						randomGenerator.Next(255),
						randomGenerator.Next(255),
						randomGenerator.Next(255)
					),
                    System.Drawing.Color.FromArgb(
						randomGenerator.Next(100),
						randomGenerator.Next(255),
						randomGenerator.Next(255),
						randomGenerator.Next(255)
					) ), 
				rect);

			// apply swirl filter
			if ( fDegree == 0)
			{
				BitmapFilter.Swirl(bmp, randomGenerator.NextDouble(), true);
			}
			else
			{
				BitmapFilter.Swirl(bmp, fDegree , true);
			}

            // draw a first line crossing the image
            //			int y1, y2;
            //			y1 = randomGenerator.Next(bmp.Height/3) + (bmp.Height/3);
            //			y2 = bmp.Height - randomGenerator.Next(bmp.Height/3);
            //
            //			g.DrawLine(new Pen( img_fontcolor, 2),
            //				new Point(0,y1), new Point(bmp.Width,y2));


            // Add some random noise.
            System.Drawing.Drawing2D.HatchBrush hatchBrush = new System.Drawing.Drawing2D.HatchBrush(
                System.Drawing.Drawing2D.HatchStyle.LargeConfetti,
                System.Drawing.Color.LightGray,
                System.Drawing.Color.DarkGray);

			int m = System.Math.Max(rect.Width, rect.Height);
			for (int i = 0; i < (int) (rect.Width * rect.Height / 30F); i++)
			{
				int x = randomGenerator.Next(rect.Width);
				int y = randomGenerator.Next(rect.Height);
				int w = randomGenerator.Next(m / 50);
				int h = randomGenerator.Next(m / 50);
				g.FillEllipse(hatchBrush, x, y, w, h);
			}


			// write keyword

			// keyword positioning
			// to space equally
			int posx;
			int posy;
			int deltax;


			deltax = System.Convert.ToInt32(size.Width/tempKey.Length);
			posx = System.Convert.ToInt32((img_width - size.Width)/2);

			// write each keyword char
			for (int l=0; l < tempKey.Length; l++ )
			{
				posy = ((int)(2.5 * (bmp.Height/5))) + (((l%2)==0)?-2:2) * ((int)(size.Height/3));
				posy = (int)((bmp.Height/2)+ (size.Height/2));
				posy += (int) ((((l%2)==0)?-2:2) * ((size.Height/3)));
				posx += deltax;
				g.DrawString(tempKey[l].ToString(),
					img_font,
					img_fontcolor,
					posx, 
					posy,
					sFormat);
			}


            // draw a curve 
            System.Drawing.Point[] ps = new System.Drawing.Point[nPoints];
			
			for (int ii=0; ii < nPoints; ii++)
			{
				int x,y;
				x = randomGenerator.Next(bmp.Width);
				y = randomGenerator.Next(bmp.Height);
				ps[ii] = new System.Drawing.Point(x,y);
			}
			g.DrawCurve(new System.Drawing.Pen( img_fontcolor, 2), ps
                , System.Convert.ToSingle(randomGenerator.NextDouble()));

			// apply water filter
			BitmapFilter.Water(bmp, nWave, false);
			
			// Clean up.
			font.Dispose();
			hatchBrush.Dispose();
			g.Dispose();

			return bmp;
		}


	}


}
