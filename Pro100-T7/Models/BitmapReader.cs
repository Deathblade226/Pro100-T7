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
	/// <summary>
	/// Encapsulates a WriteableBitmap to allow for fast GetPixel and SetPixel methods. Implements IDisposable, and changes are not saved to the bitmap until after the reader is disposed of.
	/// </summary>
	public class BitmapReader : IDisposable
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		private WriteableBitmap bmp;
		private byte[] bytes;

		public BitmapReader(WriteableBitmap bmp)
		{
			this.bmp = bmp;
			this.Width = bmp.PixelWidth;
			this.Height = bmp.PixelHeight;
			this.bytes = bmp.PixelBuffer.ToArray();
		}

		/// <summary>
		/// Returns the color at the specified coordinates from the bitmap used to initialize this class.
		/// </summary>
		/// <param name="x">The x coordinate of the pixel</param>
		/// <param name="y">The y coordinate of the pixel</param>
		/// <returns>The color at the specified location as a Color object</returns>
		public Color GetPixel(int x, int y)
		{
			int i = ((y * Width) + x) * sizeof(int);
			byte b = bytes[i];
			i++;
			byte g = bytes[i];
			i++;
			byte r = bytes[i];
			i++;
			byte a = bytes[i];

			return Color.FromArgb(a, r, g, b);
		}

		/// <summary>
		/// Returns the color at the specified coordinates from the bitmap used to initialize this class.
		/// </summary>
		/// <param name="x">The x coordinate of the pixel</param>
		/// <param name="y">The y coordinate of the pixel</param>
		/// <returns>An integer representation of the color (stored as ARGB)</returns>
		public int GetPixeli(int x, int y)
		{
			int i = ((y * Width) + x) * sizeof(int);
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

		/// <summary>
		/// Sets the pixel at the specified location of the bitmap used to initialize this class.
		/// </summary>
		/// <param name="x">The x coordinate of the pointer</param>
		/// <param name="y">The y coordinate of the pointer</param>
		/// <param name="color">The desired color as a Color object</param>
		public void SetPixel(int x, int y, Color color)
		{
			int i = ((y * Width) + x) * sizeof(int);
			bytes[i] = (byte)(color.B);
			i++;
			bytes[i] = (byte)(color.G);
			i++;
			bytes[i] = (byte)(color.R);
			i++;
			bytes[i] = (byte)(color.A);

		}

		/// <summary>
		/// Sets the pixel at the specified location of the bitmap used to initialize this class.
		/// </summary>
		/// <param name="x">The x coordinate of the pointer</param>
		/// <param name="y">The y coordinate of the pointer</param>
		/// <param name="color">The desired color as an integer (stored as ARGB)</param>
		public void SetPixel(int x, int y, int color)
		{
			int i = ((y * Width) + x) * sizeof(int);
			bytes[i] = (byte)(color >> 0);
			i++;
			bytes[i] = (byte)(color >> 8);
			i++;
			bytes[i] = (byte)(color >> 16);
			i++;
			bytes[i] = (byte)(color >> 24);
		}


		/// <summary>
		/// This method should be automatically called if the instance is created with the using keyword. Call this method to save the changes to the bitmap passed it.
		/// </summary>
		public void Dispose()
		{
			bmp.PixelBuffer.AsStream().Write(bytes, 0, bytes.Length);
		}
	}
}
