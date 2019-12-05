using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
	public static class FillTool
	{
		private static int toFillWith;
		private static int toBeFilled;

		/// <summary>
		/// Simple struct to represent a point, only used in this class.
		/// </summary>
		private struct P
		{
			public int x;
			public int y;

			public P(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		/// <summary>
		/// Edits the passed in bitmap and fills the area surrounding the point with the color passed in
		/// </summary>
		/// <param name="bitmap">The bitmap to be edited</param>
		/// <param name="x">The x value of the pointer location</param>
		/// <param name="y">The y value of the pointer location</param>
		/// <param name="color">The color to fill the area with</param>
		public static void Fill(WriteableBitmap bitmap, int x, int y, Color color)
		{
			int intColor = ((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
			Fill(bitmap, x, y, intColor);
		}

		/// <summary>
		/// Fills the area surrounding the point with the color passed in
		/// </summary>
		/// <param name="bitmap">The bitmap to be edited</param>
		/// <param name="x">The x value of the pointer location</param>
		/// <param name="y">The y value of the pointer location</param>
		/// <param name="color">The color to fill the area with</param>
		public static void Fill(WriteableBitmap bitmap, int x, int y, int color)
		{
			toFillWith = color;
			//use the using keyword to ensure the BitmapReader is disposed of before the method returns.
			using (BitmapReader bmpReader = new BitmapReader(bitmap))
			{
				toBeFilled = bmpReader.GetPixeli(x, y);
				if (toBeFilled != toFillWith)
				{
					FloodFillScanline(bmpReader, x, y);
				}

			}

		}

		

		/// <summary>
		/// Uses the scanline algorithm to flood fill a selected area with the color given, starting from the point given.
		/// </summary>
		/// <param name="x">The X position to start the fill from</param>
		/// <param name="y">The Y position to start the fill from</param>
		private static void FloodFillScanline(BitmapReader bmpReader, int x, int y)
		{
			Queue<P> points = new Queue<P>();

			points.Enqueue(new P(x, y));

			while (points.Count > 0)
			{
				P temp = points.Dequeue();
				int y1 = temp.y;


				while (y1 >= 0 && bmpReader.GetPixeli(temp.x, y1) == toBeFilled)
				{
					y1--;
				}
				y1++;
				bool toBeFilledLeft = false;
				bool toBeFilledRight = false;
				while (y1 < bmpReader.Height && bmpReader.GetPixeli(temp.x, y1) == toBeFilled)
				{
					bmpReader.SetPixel(temp.x, y1, toFillWith);

					if (!toBeFilledLeft && (temp.x > 0 && bmpReader.GetPixeli(temp.x - 1, y1) == toBeFilled))
					{
						points.Enqueue(new P(temp.x - 1, y1));
						toBeFilledLeft = true;
					}
					else if (toBeFilledLeft && (temp.x > 0 && bmpReader.GetPixeli(temp.x - 1, y1) != toBeFilled))
					{
						toBeFilledLeft = false;
					}
					if (!toBeFilledRight && (temp.x < bmpReader.Width - 1 && bmpReader.GetPixeli(temp.x + 1, y1) == toBeFilled))
					{
						points.Enqueue(new P(temp.x + 1, y1));
						toBeFilledRight = true;
					}
					else if (toBeFilledRight && (temp.x < bmpReader.Width - 1 && bmpReader.GetPixeli(temp.x + 1, y1) != toBeFilled))
					{
						toBeFilledRight = false;
					}
					y1++;
				}
			}
		}


	}
}
