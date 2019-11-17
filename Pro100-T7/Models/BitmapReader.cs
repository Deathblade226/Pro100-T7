using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
	public class BitmapReader : IDisposable
	{
		public int width;
		public int height;
		WriteableBitmap bmp;
		public byte[] bytes;

		public BitmapReader(WriteableBitmap bmp)
		{
			this.bmp = bmp;
			this.width = bmp.PixelWidth;
			this.height = bmp.PixelHeight;
			this.bytes = bmp.PixelBuffer.ToArray();
		}

		public Color GetPixel(int x, int y)
		{
			int i = ((y * width) + x) * sizeof(int);
			byte b = bytes[i];
			i++;
			byte g = bytes[i];
			i++;
			byte r = bytes[i];
			i++;
			byte a = bytes[i];

			return Color.FromArgb(a, r, g, b);
		}

		public int GetPixeli(int x, int y)
		{
			int i = ((y * width) + x) * sizeof(int);
			byte b = bytes[i];
			i++;
			byte g = bytes[i];
			i++;
			byte r = bytes[i];
			i++;
			byte a = bytes[i];


			int color = ((a << 24) | (r << 16) | (g << 8) | b);
			return color;
		}

		public void SetPixel(int x, int y, int color)
		{
			int i = ((y * width) + x) * sizeof(int);
			bytes[i] = (byte)(color >> 0);
			i++;
			bytes[i] = (byte)(color >> 8);
			i++;
			bytes[i] = (byte)(color >> 16);
			i++;
			bytes[i] = (byte)(color >> 24);

			bmp.Invalidate();
		}


		public void Dispose()
		{
			bmp.PixelBuffer.AsStream().Write(bytes, 0, bytes.Length);
		}
	}
}
