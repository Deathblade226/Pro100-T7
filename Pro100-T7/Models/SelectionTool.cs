using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
	public static class SelectionTool
	{

		
		private static bool isSelecting = false;

		private static P startPoint;
		private static P currentPoint;

		private static Rect selection;

		private static byte[] copiedBytes;

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
		
		private struct Rect
		{
			public P topLeft;
			public P bottomRight;

			public Rect(P topLeft, P bottomRight)
			{
				this.topLeft = topLeft;
				this.bottomRight = bottomRight;
			}

			public Rect(int startX, int startY, int currentX, int currentY)
			{
				topLeft = new P(startX, startY);
				bottomRight = new P(currentX, currentY);
			}
		}


		public static void Selection(WriteableBitmap bmp, int x, int y)
		{
			if(!isSelecting)
			{
				isSelecting = true;
				startPoint = new P(x, y);
			}

			currentPoint = new P(x, y);

			//make a rectangle to represent the selected area (probably store it statically)
			selection = new Rect(startPoint, currentPoint);



			//display the rectangle?
			bmp.DrawRectangle(startPoint.x, startPoint.y, currentPoint.x, currentPoint.y, Colors.Black);



		}

		public static void ClearSelection()
		{
			//use the rectangle to clear the bytes within the selected area

		}

		public static void CopySelection()
		{
			//use the rectangle to copy the bytes from that area to a new array with the same dimensions as the rectangle

			//probably store the copied area as a byte area in this class.

		}

		public static void PasteSelection()
		{
			//use the rectangle to paste the bytes from that area to a new array with the same dimensions as the rectangle

		}
	}
}
