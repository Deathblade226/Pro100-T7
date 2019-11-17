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
	public class FillTool
	{
		public static int toFillWith = (((byte)255) << 24 ) | (((byte)255) << 16);
		public static int toBeFilled;

		public static Queue<P> points = new Queue<P>();

		public static BitmapReader bmpReader;

		public struct P
		{
			public int x;
			public int y;

			public P(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}


		public static void Fill(WriteableBitmap bitmap, int x, int y)
		{

			using (bmpReader = new BitmapReader(bitmap))
			{
				toBeFilled = bmpReader.GetPixeli(x, y);
				if (toBeFilled != toFillWith)
				{
					FloodFillScanlineV2(x, y);
				}

			}

		}

		

		/// <summary>
		/// Uses the scanline algorithm to flood fill a selected area, starting from the point given.
		/// </summary>
		/// <param name="x">The X position to start the fill from</param>
		/// <param name="y">The Y position to start the fill from</param>
		private static void FloodFillScanlineV2(int x, int y)
		{
			points.Enqueue(new P(x, y));

			while (points.Count != 0)
			{
				P temp = points.Dequeue();
				int y1 = temp.y;


				while (y1 >= 0 && bmpReader.GetPixeli(temp.x, y1) == toBeFilled)
				{
					y1--;
				}
				y1++;
				bool spanLeft = false;
				bool spanRight = false;
				while (y1 < bmpReader.height && bmpReader.GetPixeli(temp.x, y1) == toBeFilled)
				{
					bmpReader.SetPixel(temp.x, y1, toFillWith);

					if (!spanLeft && temp.x > 0 && bmpReader.GetPixeli(temp.x - 1, y1) == toBeFilled)
					{
						points.Enqueue(new P(temp.x - 1, y1));
						spanLeft = true;
					}
					else if (spanLeft || temp.x - 1 == 0 && bmpReader.GetPixeli(temp.x - 1, y1) != toBeFilled)
					{
						spanLeft = false;
					}
					if (!spanRight && temp.x < bmpReader.width - 1 && bmpReader.GetPixeli(temp.x + 1, y1) == toBeFilled)
					{
						points.Enqueue(new P(temp.x + 1, y1));
						spanRight = true;
					}
					else if (spanRight && temp.x < bmpReader.width - 1 && bmpReader.GetPixeli(temp.x + 1, y1) != toBeFilled)
					{
						spanRight = false;
					}
					y1++;
				}
			}
		}


	}
}
