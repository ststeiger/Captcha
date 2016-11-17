
namespace CSharpFilters
{


	public class ConvMatrix
	{
		public int TopLeft = 0, TopMid = 0, TopRight = 0;
		public int MidLeft = 0, Pixel = 1, MidRight = 0;
		public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
		public int Factor = 1;
		public int Offset = 0;
		public void SetAll(int nVal)
		{
			TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
		}
	}

	public struct FloatPoint
	{
		public double X;
		public double Y;
	}

	public class BitmapFilter
	{
		public const short EDGE_DETECT_KIRSH		= 1;
		public const short EDGE_DETECT_PREWITT		= 2;
		public const short EDGE_DETECT_SOBEL		= 3;

		public static bool Invert(System.Drawing.Bitmap b)
		{
            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;
	
				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						p[0] = (byte)(255-p[0]);
						++p;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool GrayScale(System.Drawing.Bitmap b)
		{
            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				byte red, green, blue;
	
				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						blue = p[0];
						green = p[1];
						red = p[2];

						p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool Brightness(System.Drawing.Bitmap b, int nBrightness)
		{
			if (nBrightness < -255 || nBrightness > 255)
				return false;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			int nVal = 0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						nVal = (int) (p[0] + nBrightness);
		
						if (nVal < 0) nVal = 0;
						if (nVal > 255) nVal = 255;

						p[0] = (byte)nVal;

						++p;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool Contrast(System.Drawing.Bitmap b, sbyte nContrast)
		{
			if (nContrast < -100) return false;
			if (nContrast >  100) return false;

			double pixel = 0, contrast = (100.0+nContrast)/100.0;

			contrast *= contrast;

			int red, green, blue;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						blue = p[0];
						green = p[1];
						red = p[2];
				
						pixel = red/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[2] = (byte) pixel;

						pixel = green/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[1] = (byte) pixel;

						pixel = blue/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[0] = (byte) pixel;					

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}
	
		public static bool Gamma(System.Drawing.Bitmap b, double red, double green, double blue)
		{
			if (red < .2 || red > 5) return false;
			if (green < .2 || green > 5) return false;
			if (blue < .2 || blue > 5) return false;

			byte [] redGamma = new byte [256];
			byte [] greenGamma = new byte [256];
			byte [] blueGamma = new byte [256];

			for (int i = 0; i< 256; ++i)
			{
				redGamma[i] = (byte)System.Math.Min(255, (int)(( 255.0 * System.Math.Pow(i/255.0, 1.0/red)) + 0.5));
				greenGamma[i] = (byte)System.Math.Min(255, (int)(( 255.0 * System.Math.Pow(i/255.0, 1.0/green)) + 0.5));
				blueGamma[i] = (byte)System.Math.Min(255, (int)(( 255.0 * System.Math.Pow(i/255.0, 1.0/blue)) + 0.5));
			}

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						p[2] = redGamma[ p[2] ];
						p[1] = greenGamma[ p[1] ];
						p[0] = blueGamma[ p[0] ];

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool Color(System.Drawing.Bitmap b, int red, int green, int blue)
		{
			if (red < -255 || red > 255) return false;
			if (green < -255 || green > 255) return false;
			if (blue < -255 || blue > 255) return false;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;
				int nPixel;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						nPixel = p[2] + red;
						nPixel = System.Math.Max(nPixel, 0);
						p[2] = (byte)System.Math.Min(255, nPixel);

						nPixel = p[1] + green;
						nPixel = System.Math.Max(nPixel, 0);
						p[1] = (byte)System.Math.Min(255, nPixel);

						nPixel = p[0] + blue;
						nPixel = System.Math.Max(nPixel, 0);
						p[0] = (byte)System.Math.Min(255, nPixel);

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool Conv3x3(System.Drawing.Bitmap b, ConvMatrix m)
		{
			// Avoid divide by zero errors
			if (0 == m.Factor) return false;

            System.Drawing.Bitmap bSrc = (System.Drawing.Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmSrc = bSrc.LockBits(
                new System.Drawing.Rectangle(0, 0, bSrc.Width, bSrc.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			int stride2 = stride * 2;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr SrcScan0 = bmSrc.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * pSrc = (byte *)(void *)SrcScan0;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width - 2;
				int nHeight = b.Height - 2;

				int nPixel;

				for(int y=0;y < nHeight;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						nPixel = ( ( ( (pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
							(pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
							(pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						p[5 + stride]= (byte)nPixel;

						nPixel = ( ( ( (pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
							(pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
							(pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;
							
						p[4 + stride] = (byte)nPixel;

						nPixel = ( ( ( (pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
							(pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
							(pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						p[3 + stride] = (byte)nPixel;

						p += 3;
						pSrc += 3;
					}
					p += nOffset;
					pSrc += nOffset;
				}
			}

			b.UnlockBits(bmData);
			bSrc.UnlockBits(bmSrc);

			return true;
		}

		public static bool Smooth(System.Drawing.Bitmap b, int nWeight /* default to 1 */)
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(1);
			m.Pixel = nWeight;
			m.Factor = nWeight + 8;

			return  BitmapFilter.Conv3x3(b, m);
		}

		public static bool GaussianBlur(System.Drawing.Bitmap b, int nWeight /* default to 4*/)
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(1);
			m.Pixel = nWeight;
			m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
			m.Factor = nWeight + 12;

			return  BitmapFilter.Conv3x3(b, m);
		}
		public static bool MeanRemoval(System.Drawing.Bitmap b, int nWeight /* default to 9*/ )
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(-1);
			m.Pixel = nWeight;
			m.Factor = nWeight - 8;

			return BitmapFilter.Conv3x3(b, m);
		}
		public static bool Sharpen(System.Drawing.Bitmap b, int nWeight /* default to 11*/ )
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(0);
			m.Pixel = nWeight;
			m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -2;
			m.Factor = nWeight - 8;

			return  BitmapFilter.Conv3x3(b, m);
		}
		public static bool EmbossLaplacian(System.Drawing.Bitmap b)
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(-1);
			m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 0;
			m.Pixel = 4;
			m.Offset = 127;

			return  BitmapFilter.Conv3x3(b, m);
		}	
		public static bool EdgeDetectQuick(System.Drawing.Bitmap b)
		{
			ConvMatrix m = new ConvMatrix();
			m.TopLeft = m.TopMid = m.TopRight = -1;
			m.MidLeft = m.Pixel = m.MidRight = 0;
			m.BottomLeft = m.BottomMid = m.BottomRight = 1;
		
			m.Offset = 127;

			return  BitmapFilter.Conv3x3(b, m);
		}

		public static bool EdgeDetectConvolution(System.Drawing.Bitmap b, short nType, byte nThreshold)
		{
			ConvMatrix m = new ConvMatrix();

            // I need to make a copy of this bitmap BEFORE I alter it 80)
            System.Drawing.Bitmap bTemp = (System.Drawing.Bitmap)b.Clone();

			switch (nType)
			{
				case EDGE_DETECT_SOBEL:
					m.SetAll(0);
					m.TopLeft = m.BottomLeft = 1;
					m.TopRight = m.BottomRight = -1;
					m.MidLeft = 2;
					m.MidRight = -2;
					m.Offset = 0;
					break;
				case EDGE_DETECT_PREWITT:
					m.SetAll(0);
					m.TopLeft = m.MidLeft = m.BottomLeft = -1;
					m.TopRight = m.MidRight = m.BottomRight = 1;
					m.Offset = 0;
					break;
				case EDGE_DETECT_KIRSH:
					m.SetAll(-3);
					m.Pixel = 0;
					m.TopLeft = m.MidLeft = m.BottomLeft = 5;
					m.Offset = 0;
					break;
			}

			BitmapFilter.Conv3x3(b, m);

			switch (nType)
			{
				case EDGE_DETECT_SOBEL:
					m.SetAll(0);
					m.TopLeft = m.TopRight = 1;
					m.BottomLeft = m.BottomRight = -1;
					m.TopMid = 2;
					m.BottomMid = -2;
					m.Offset = 0;
					break;
				case EDGE_DETECT_PREWITT:
					m.SetAll(0);
					m.BottomLeft = m.BottomMid = m.BottomRight = -1;
					m.TopLeft = m.TopMid = m.TopRight = 1;
					m.Offset = 0;
					break;
				case EDGE_DETECT_KIRSH:
					m.SetAll(-3);
					m.Pixel = 0;
					m.BottomLeft = m.BottomMid = m.BottomRight = 5;
					m.Offset = 0;
					break;
			}

			BitmapFilter.Conv3x3(bTemp, m);

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmData2 = bTemp.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr Scan02 = bmData2.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * p2 = (byte *)(void *)Scan02;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				int nPixel = 0;
	
				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						nPixel = (int)System.Math.Sqrt((p[0]*p[0]) + (p2[0] * p2[0]));
						if (nPixel<nThreshold)nPixel = nThreshold;
						if (nPixel>255) nPixel = 255;
						p[0] = (byte) nPixel;
						++p;
						++p2;
					}
					p += nOffset;
					p2 += nOffset;
				}
			}

			b.UnlockBits(bmData);
			bTemp.UnlockBits(bmData2);

			return true;
		}
	
		public static bool EdgeDetectHorizontal(System.Drawing.Bitmap b)
		{
            System.Drawing.Bitmap bmTemp = (System.Drawing.Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmData2 = bmTemp.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr Scan02 = bmData2.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * p2 = (byte *)(void *)Scan02;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				int nPixel = 0;
	
				p += stride;
				p2 += stride;

				for(int y=1;y<b.Height-1;++y)
				{
					p += 9;
					p2 += 9;

					for(int x=9; x < nWidth-9; ++x )
					{
						nPixel = ((p2 + stride - 9)[0] +
							(p2 + stride - 6)[0] +
							(p2 + stride - 3)[0] +
							(p2 + stride)[0] +
							(p2 + stride + 3)[0] +
							(p2 + stride + 6)[0] +
							(p2 + stride + 9)[0] -
							(p2 - stride - 9)[0] -
							(p2 - stride - 6)[0] -
							(p2 - stride - 3)[0] -
							(p2 - stride)[0] -
							(p2 - stride + 3)[0] -
							(p2 - stride + 6)[0] -
							(p2 - stride + 9)[0]);

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						(p+stride)[0] = (byte) nPixel;
					
						++ p;
						++ p2;
					}

					p += 9 + nOffset;
					p2 += 9 + nOffset;
				}
			}

			b.UnlockBits(bmData);
			bmTemp.UnlockBits(bmData2);

			return true;
		}

		public static bool EdgeDetectVertical(System.Drawing.Bitmap b)
		{
            System.Drawing.Bitmap bmTemp = (System.Drawing.Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmData2 = bmTemp.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr Scan02 = bmData2.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * p2 = (byte *)(void *)Scan02;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				int nPixel = 0;

				int nStride2 = stride *2;
				int nStride3 = stride * 3;
	
				p += nStride3;
				p2 += nStride3;

				for(int y=3;y<b.Height-3;++y)
				{
					p += 3;
					p2 += 3;

					for(int x=3; x < nWidth-3; ++x )
					{
						nPixel = ((p2 + nStride3 + 3)[0] +
							(p2 + nStride2 + 3)[0] +
							(p2 + stride + 3)[0] +
							(p2 + 3)[0] +
							(p2 - stride + 3)[0] +
							(p2 - nStride2 + 3)[0] +
							(p2 - nStride3 + 3)[0] -
							(p2 + nStride3 - 3)[0] -
							(p2 + nStride2 - 3)[0] -
							(p2 + stride - 3)[0] -
							(p2 - 3)[0] -
							(p2 - stride - 3)[0] -
							(p2 - nStride2 - 3)[0] -
							(p2 - nStride3 - 3)[0]);

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						p[0] = (byte) nPixel;
					
						++ p;
						++ p2;
					}

					p += 3 + nOffset;
					p2 += 3 + nOffset;
				}
			}

			b.UnlockBits(bmData);
			bmTemp.UnlockBits(bmData2);

			return true;
		}

		public static bool EdgeDetectHomogenity(System.Drawing.Bitmap b, byte nThreshold)
		{
            // This one works by working out the greatest difference between a pixel and it's eight neighbours.
            // The threshold allows softer edges to be forced down to black, use 0 to negate it's effect.
            System.Drawing.Bitmap b2 = (System.Drawing.Bitmap) b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmData2 = b2.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr Scan02 = bmData2.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * p2 = (byte *)(void *)Scan02;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				int nPixel = 0, nPixelMax = 0;

				p += stride;
				p2 += stride;

				for(int y=1;y<b.Height-1;++y)
				{
					p += 3;
					p2 += 3;

					for(int x=3; x < nWidth-3; ++x )
					{
						nPixelMax = System.Math.Abs(p2[0] - (p2+stride-3)[0]);
						nPixel = System.Math.Abs(p2[0] - (p2 + stride)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs(p2[0] - (p2 + stride + 3)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs(p2[0] - (p2 - stride)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs(p2[0] - (p2 + stride)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs(p2[0] - (p2 - stride - 3)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs(p2[0] - (p2 - stride)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs(p2[0] - (p2 - stride + 3)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						if (nPixelMax < nThreshold) nPixelMax = 0;

						p[0] = (byte) nPixelMax;

						++ p;
						++ p2;
					}

					p += 3 + nOffset;
					p2 += 3 + nOffset;
				}
			}

			b.UnlockBits(bmData);
			b2.UnlockBits(bmData2);

			return true;
            
		}
		public static bool EdgeDetectDifference(System.Drawing.Bitmap b, byte nThreshold)
		{
            // This one works by working out the greatest difference between a pixel and it's eight neighbours.
            // The threshold allows softer edges to be forced down to black, use 0 to negate it's effect.
            System.Drawing.Bitmap b2 = (System.Drawing.Bitmap) b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmData2 = b2.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr Scan02 = bmData2.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * p2 = (byte *)(void *)Scan02;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				int nPixel = 0, nPixelMax = 0;

				p += stride;
				p2 += stride;

				for(int y=1;y<b.Height-1;++y)
				{
					p += 3;
					p2 += 3;

					for(int x=3; x < nWidth-3; ++x )
					{
						nPixelMax = System.Math.Abs((p2 - stride + 3)[0] - (p2+stride-3)[0]);
						nPixel = System.Math.Abs((p2 + stride + 3)[0] - (p2 - stride - 3)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs((p2 - stride)[0] - (p2 + stride)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs((p2+3)[0] - (p2 - 3)[0]);
						if (nPixel>nPixelMax) nPixelMax = nPixel;

						if (nPixelMax < nThreshold) nPixelMax = 0;

						p[0] = (byte) nPixelMax;

						++ p;
						++ p2;
					}

					p += 3 + nOffset;
					p2 += 3 + nOffset;
				}
			}

			b.UnlockBits(bmData);
			b2.UnlockBits(bmData2);

			return true;
            
		}

		public static bool EdgeEnhance(System.Drawing.Bitmap b, byte nThreshold)
		{
            // This one works by working out the greatest difference between a nPixel and it's eight neighbours.
            // The threshold allows softer edges to be forced down to black, use 0 to negate it's effect.
            System.Drawing.Bitmap b2 = (System.Drawing.Bitmap) b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmData2 = b2.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr Scan02 = bmData2.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * p2 = (byte *)(void *)Scan02;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				int nPixel = 0, nPixelMax = 0;

				p += stride;
				p2 += stride;

				for (int y = 1; y < b.Height-1; ++y)
				{
					p += 3;
					p2 += 3;

					for (int x = 3; x < nWidth-3; ++x)
					{
						nPixelMax = System.Math.Abs((p2 - stride + 3)[0] - (p2 + stride - 3)[0]);

						nPixel = System.Math.Abs((p2 + stride + 3)[0] - (p2 - stride - 3)[0]);

						if (nPixel > nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs((p2 - stride)[0] - (p2 + stride)[0]);

						if (nPixel > nPixelMax) nPixelMax = nPixel;

						nPixel = System.Math.Abs((p2 + 3)[0] - (p2 - 3)[0]);

						if (nPixel > nPixelMax) nPixelMax = nPixel;

						if (nPixelMax > nThreshold && nPixelMax > p[0])
							p[0] = (byte)System.Math.Max(p[0], nPixelMax);

						++ p;
						++ p2;			
					}

					p += nOffset + 3;
					p2 += nOffset + 3;
				}
			}	

			b.UnlockBits(bmData);
			b2.UnlockBits(bmData2);

			return true;
		}
		public static System.Drawing.Bitmap Resize(System.Drawing.Bitmap b, int nWidth, int nHeight, bool bBilinear)
		{
            System.Drawing.Bitmap bTemp = (System.Drawing.Bitmap)b.Clone();
			b = new System.Drawing.Bitmap(nWidth, nHeight, bTemp.PixelFormat);

			double nXFactor = (double)bTemp.Width/(double)nWidth;
			double nYFactor = (double)bTemp.Height/(double)nHeight;

			if (bBilinear)
			{
				double fraction_x, fraction_y, one_minus_x, one_minus_y;
				int ceil_x, ceil_y, floor_x, floor_y;
                System.Drawing.Color c1 = new System.Drawing.Color();
                System.Drawing.Color c2 = new System.Drawing.Color();
                System.Drawing.Color c3 = new System.Drawing.Color();
                System.Drawing.Color c4 = new System.Drawing.Color();
				byte red, green, blue;

				byte b1, b2;

				for (int x = 0; x < b.Width; ++x)
					for (int y = 0; y < b.Height; ++y)
					{
						// Setup

						floor_x = (int)System.Math.Floor(x * nXFactor);
						floor_y = (int)System.Math.Floor(y * nYFactor);
						ceil_x = floor_x + 1;
						if (ceil_x >= bTemp.Width) ceil_x = floor_x;
						ceil_y = floor_y + 1;
						if (ceil_y >= bTemp.Height) ceil_y = floor_y;
						fraction_x = x * nXFactor - floor_x;
						fraction_y = y * nYFactor - floor_y;
						one_minus_x = 1.0 - fraction_x;
						one_minus_y = 1.0 - fraction_y;

						c1 = bTemp.GetPixel(floor_x, floor_y);
						c2 = bTemp.GetPixel(ceil_x, floor_y);
						c3 = bTemp.GetPixel(floor_x, ceil_y);
						c4 = bTemp.GetPixel(ceil_x, ceil_y);

						// Blue
						b1 = (byte)(one_minus_x * c1.B + fraction_x * c2.B);

						b2 = (byte)(one_minus_x * c3.B + fraction_x * c4.B);
						
						blue = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						// Green
						b1 = (byte)(one_minus_x * c1.G + fraction_x * c2.G);

						b2 = (byte)(one_minus_x * c3.G + fraction_x * c4.G);
						
						green = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						// Red
						b1 = (byte)(one_minus_x * c1.R + fraction_x * c2.R);

						b2 = (byte)(one_minus_x * c3.R + fraction_x * c4.R);
						
						red = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						b.SetPixel(x,y, System.Drawing.Color.FromArgb(255, red, green, blue));
					}
			}
			else
			{
				for (int x = 0; x < b.Width; ++x)
					for (int y = 0; y < b.Height; ++y)
						b.SetPixel(x, y, bTemp.GetPixel((int)(System.Math.Floor(x * nXFactor))
                            ,(int)(System.Math.Floor(y * nYFactor))));
			}

			return b;
		}

		public static bool OffsetFilterAbs(System.Drawing.Bitmap b, System.Drawing.Point[,] offset )
		{
            System.Drawing.Bitmap bSrc = (System.Drawing.Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmSrc = bSrc.LockBits(
                new System.Drawing.Rectangle(0, 0, bSrc.Width, bSrc.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int scanline = bmData.Stride;

			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr SrcScan0 = bmSrc.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * pSrc = (byte *)(void *)SrcScan0;

				int nOffset = bmData.Stride - b.Width*3;
				int nWidth = b.Width;
				int nHeight = b.Height;

				int xOffset, yOffset;

				for(int y=0;y < nHeight;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{	
						xOffset = offset[x,y].X;
						yOffset = offset[x,y].Y;
				
						if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
						{
							p[0] = pSrc[(yOffset * scanline) + (xOffset * 3)];
							p[1] = pSrc[(yOffset * scanline) + (xOffset * 3) + 1];
							p[2] = pSrc[(yOffset * scanline) + (xOffset * 3) + 2];
						}

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);
			bSrc.UnlockBits(bmSrc);

			return true;
		}

		public static bool OffsetFilter(System.Drawing.Bitmap b, System.Drawing.Point[,] offset )
		{
            System.Drawing.Bitmap bSrc = (System.Drawing.Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmSrc = bSrc.LockBits(
                new System.Drawing.Rectangle(0, 0, bSrc.Width, bSrc.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int scanline = bmData.Stride;

			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr SrcScan0 = bmSrc.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * pSrc = (byte *)(void *)SrcScan0;

				int nOffset = bmData.Stride - b.Width*3;
				int nWidth = b.Width;
				int nHeight = b.Height;

				int xOffset, yOffset;

				for(int y=0;y < nHeight;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{	
						xOffset = offset[x,y].X;
						yOffset = offset[x,y].Y;

						if (y+yOffset >= 0 && y+yOffset < nHeight && x+xOffset >= 0 && x+xOffset < nWidth)
						{
							p[0] = pSrc[((y+yOffset) * scanline) + ((x+xOffset) * 3)];
							p[1] = pSrc[((y+yOffset) * scanline) + ((x+xOffset) * 3) + 1];
							p[2] = pSrc[((y+yOffset) * scanline) + ((x+xOffset) * 3) + 2];
						}

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);
			bSrc.UnlockBits(bmSrc);

			return true;
		}

		public static bool OffsetFilterAntiAlias(System.Drawing.Bitmap b, FloatPoint[,] fp)
		{
            System.Drawing.Bitmap bSrc = (System.Drawing.Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(
                new System.Drawing.Rectangle(0, 0, b.Width, b.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Imaging.BitmapData bmSrc = bSrc.LockBits(
                new System.Drawing.Rectangle(0, 0, bSrc.Width, bSrc.Height)
                , System.Drawing.Imaging.ImageLockMode.ReadWrite
                , System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			int scanline = bmData.Stride;

			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr SrcScan0 = bmSrc.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * pSrc = (byte *)(void *)SrcScan0;

				int nOffset = bmData.Stride - b.Width*3;
				int nWidth = b.Width;
				int nHeight = b.Height;

				double xOffset, yOffset;

				double fraction_x, fraction_y, one_minus_x, one_minus_y;
				int ceil_x, ceil_y, floor_x, floor_y;
				byte p1, p2;

				for(int y=0;y < nHeight;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{	
						xOffset = fp[x,y].X;
						yOffset = fp[x,y].Y;
				
						// Setup

						floor_x = (int)System.Math.Floor(xOffset);
						floor_y = (int)System.Math.Floor(yOffset);
						ceil_x = floor_x + 1;
						ceil_y = floor_y + 1;
						fraction_x = xOffset - floor_x;
						fraction_y = yOffset - floor_y;
						one_minus_x = 1.0 - fraction_x;
						one_minus_y = 1.0 - fraction_y;

						if (floor_y >= 0 && ceil_y < nHeight && floor_x >= 0 && ceil_x < nWidth)
						{
							// Blue

							p1 = (byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3]) +
								fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3]));

							p2 = (byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3]) +
								fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x]));

							p[x * 3 + y*scanline] = (byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));
							
							// Green

							p1 = (byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3 + 1]) +
								fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3 + 1]));

							p2 = (byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3 + 1]) +
								fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x + 1]));
			
							p[x * 3 + y*scanline + 1] = (byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));

							// Red

							p1 = (byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3 + 2]) +
								fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3 + 2]));

							p2 = (byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3 + 2]) +
								fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x + 2]));
			
							p[x * 3 + y*scanline + 2] = (byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));
						}
					}
				}
			}

			b.UnlockBits(bmData);
			bSrc.UnlockBits(bmSrc);

			return true;
		}

		public static bool Flip(System.Drawing.Bitmap b, bool bHorz, bool bVert)
		{
            System.Drawing.Point [,] ptFlip = new System.Drawing.Point[b.Width,b.Height]; 

			int nWidth = b.Width;
			int nHeight = b.Height;

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					ptFlip[x, y].X = (bHorz) ? nWidth - (x+1) : x;
					ptFlip[x,y].Y = (bVert) ? nHeight - (y + 1) : y;
				}
				
			OffsetFilterAbs(b, ptFlip);		

			return true;
		}

		public static bool RandomJitter(System.Drawing.Bitmap b, short nDegree)
		{
            System.Drawing.Point [,] ptRandJitter = new System.Drawing.Point[b.Width,b.Height]; 

			int nWidth = b.Width;
			int nHeight = b.Height;

			int newX, newY;

			short nHalf = (short)System.Math.Floor((double)nDegree/2);
            System.Random rnd = new System.Random();

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					newX = rnd.Next(nDegree) - nHalf;

					if (x + newX > 0 && x + newX < nWidth)
						ptRandJitter[x, y].X = newX;
					else
						ptRandJitter[x, y].X = 0;

					newY = rnd.Next(nDegree) - nHalf;

					if (y + newY > 0 && y + newY < nWidth)
						ptRandJitter[x, y].Y = newY;
					else
						ptRandJitter[x, y].Y = 0;
				}
				
			OffsetFilter(b, ptRandJitter);		

			return true;
		}
		public static bool Swirl(System.Drawing.Bitmap b, double fDegree, bool bSmoothing /* default fDegree to .05 */)
		{
			int nWidth = b.Width;
			int nHeight = b.Height;

			FloatPoint [,] fp = new FloatPoint[nWidth, nHeight];
            System.Drawing.Point [,] pt = new System.Drawing.Point[nWidth, nHeight];

            System.Drawing.Point mid = new System.Drawing.Point();
			mid.X = nWidth/2;
			mid.Y = nHeight/2;

			double theta, radius;
			double newX, newY;

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					int trueX = x - mid.X;
					int trueY = y - mid.Y;
					theta = System.Math.Atan2((trueY),(trueX));

					radius = System.Math.Sqrt(trueX*trueX + trueY*trueY);

					newX = mid.X + (radius * System.Math.Cos(theta + fDegree * radius));
					if (newX > 0 && newX < nWidth)
					{
						fp[x, y].X = newX;
						pt[x, y].X = (int)newX;
					}
					else
						fp[x, y].X = pt[x, y].X = x;

					newY = mid.Y + (radius * System.Math.Sin(theta + fDegree * radius));
					if (newY > 0 && newY < nHeight)
					{
						fp[x, y].Y = newY;
						pt[x, y].Y = (int)newY;
					}
					else
						fp[x, y].Y = pt[x, y].Y = y;
				}

			if(bSmoothing)
				OffsetFilterAntiAlias(b, fp);
			else
				OffsetFilterAbs(b, pt);		

			return true;
		}

		public static bool Sphere(System.Drawing.Bitmap b, bool bSmoothing)
		{
			int nWidth = b.Width;
			int nHeight = b.Height;

			FloatPoint [,] fp = new FloatPoint[nWidth, nHeight];
            System.Drawing.Point [,] pt = new System.Drawing.Point[nWidth, nHeight];

            System.Drawing.Point mid = new System.Drawing.Point();
			mid.X = nWidth/2;
			mid.Y = nHeight/2;

			double theta, radius;
			double newX, newY;

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					int trueX = x - mid.X;
					int trueY = y - mid.Y;
					theta = System.Math.Atan2((trueY),(trueX));

					radius = System.Math.Sqrt(trueX*trueX + trueY*trueY);

					double newRadius = radius * radius/(System.Math.Max(mid.X, mid.Y));

					newX = mid.X + (newRadius * System.Math.Cos(theta));

					if (newX > 0 && newX < nWidth)
					{
						fp[x, y].X = newX;
						pt[x, y].X = (int) newX;
					}
					else
					{
						fp[x, y].X = fp[x,y].Y = 0.0;
						pt[x, y].X = pt[x,y].Y = 0;
					}

					newY = mid.Y + (newRadius * System.Math.Sin(theta));

					if (newY > 0 && newY < nHeight && newX > 0 && newX < nWidth)
					{
						fp[x, y].Y = newY;
						pt[x, y].Y = (int) newY;
					}
					else
					{
						fp[x, y].X = fp[x,y].Y = 0.0;
						pt[x, y].X = pt[x,y].Y = 0;
					}
				}

			if(bSmoothing)
				OffsetFilterAbs(b, pt);
			else
				OffsetFilterAntiAlias(b, fp);
	
			return true;
		}

		public static bool TimeWarp(System.Drawing.Bitmap b, byte factor, bool bSmoothing)
		{
			int nWidth = b.Width;
			int nHeight = b.Height;

			FloatPoint [,] fp = new FloatPoint[nWidth, nHeight];
            System.Drawing.Point [,] pt = new System.Drawing.Point[nWidth, nHeight];

            System.Drawing.Point mid = new System.Drawing.Point();
			mid.X = nWidth/2;
			mid.Y = nHeight/2;

			double theta, radius;
			double newX, newY;

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					int trueX = x - mid.X;
					int trueY = y - mid.Y;
					theta = System.Math.Atan2((trueY),(trueX));

					radius = System.Math.Sqrt(trueX*trueX + trueY*trueY);

					double newRadius = System.Math.Sqrt(radius) * factor;

					newX = mid.X + (newRadius * System.Math.Cos(theta));
					if (newX > 0 && newX < nWidth)
					{
						fp[x, y].X = newX;
						pt[x, y].X = (int) newX;
					}
					else
					{
						fp[x, y].X = 0.0;
						pt[x, y].X = 0;
					}

					newY = mid.Y + (newRadius * System.Math.Sin(theta));
					if (newY > 0 && newY < nHeight)
					{
						fp[x, y].Y = newY;
						pt[x, y].Y = (int) newY;
					}
					else
					{
						fp[x, y].Y = 0.0;
						pt[x, y].Y = 0;
					}
				}

			if(bSmoothing)
				OffsetFilterAbs(b, pt);
			else
				OffsetFilterAntiAlias(b, fp);
	
			return true;
		}

		public static bool Moire(System.Drawing.Bitmap b, double fDegree)
		{
			int nWidth = b.Width;
			int nHeight = b.Height;

            System.Drawing.Point [,] pt = new System.Drawing.Point[nWidth, nHeight];

            System.Drawing.Point mid = new System.Drawing.Point();
			mid.X = nWidth/2;
			mid.Y = nHeight/2;

			double theta, radius;
			int newX, newY;

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					int trueX = x - mid.X;
					int trueY = y - mid.Y;
					theta = System.Math.Atan2((trueX),(trueY));

					radius = System.Math.Sqrt(trueX*trueX + trueY*trueY);

					newX = (int)(radius * System.Math.Sin(theta + fDegree * radius));
					if (newX > 0 && newX < nWidth)
					{
						pt[x, y].X = (int) newX;
					}
					else
					{
						pt[x, y].X = 0;
					}

					newY = (int)(radius * System.Math.Sin(theta + fDegree * radius));
					if (newY > 0 && newY < nHeight)
					{
						pt[x, y].Y = (int) newY;
					}
					else
					{
						pt[x, y].Y = 0;
					}
				}

			OffsetFilterAbs(b, pt);

			return true;
		}

		public static bool Water(System.Drawing.Bitmap b, short nWave, bool bSmoothing)
		{
			int nWidth = b.Width;
			int nHeight = b.Height;

			FloatPoint [,] fp = new FloatPoint[nWidth, nHeight];
            System.Drawing.Point [,] pt = new System.Drawing.Point[nWidth, nHeight];

            System.Drawing.Point mid = new System.Drawing.Point();
			mid.X = nWidth/2;
			mid.Y = nHeight/2;

			double newX, newY;
			double xo, yo;

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					xo = ((double)nWave * System.Math.Sin(2.0 * 3.1415 * (float)y / 128.0));
					yo = ((double)nWave * System.Math.Cos(2.0 * 3.1415 * (float)x / 128.0));

					newX = (x + xo);
					newY = (y + yo);

					if (newX > 0 && newX < nWidth)
					{
						fp[x, y].X = newX;
						pt[x, y].X = (int) newX;
					}
					else
					{
						fp[x, y].X = 0.0;
						pt[x, y].X = 0;
					}


					if (newY > 0 && newY < nHeight)
					{
						fp[x, y].Y = newY;
						pt[x, y].Y = (int) newY;
					}
					else
					{
						fp[x, y].Y = 0.0;
						pt[x, y].Y = 0;
					}
				}

			if(bSmoothing)
				OffsetFilterAbs(b, pt);
			else
				OffsetFilterAntiAlias(b, fp);
	
			return true;
		}

		public static bool Pixelate(System.Drawing.Bitmap b, short pixel, bool bGrid)
		{
			int nWidth = b.Width;
			int nHeight = b.Height;

            System.Drawing.Point [,] pt = new System.Drawing.Point[nWidth, nHeight];

			int newX, newY;

			for (int x = 0; x < nWidth; ++x)
				for (int y = 0; y < nHeight; ++y)
				{
					newX = pixel - x%pixel;

					if (bGrid && newX == pixel)
						pt[x, y].X = -x;
					else if (x + newX > 0 && x +newX < nWidth)
						pt[x, y].X = newX;
					else
						pt[x, y].X = 0;

					newY = pixel - y%pixel;

					if (bGrid && newY == pixel)
						pt[x, y].Y = -y;
					else if (y + newY > 0 && y + newY < nHeight)
						pt[x, y].Y = newY;
					else
						pt[x, y].Y = 0;
				}

			OffsetFilter(b, pt);

			return true;
		}
	}
}
