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
	public static class SelectionTool
	{


		private static bool isSelecting = false;
		private static bool selectionMade = false;

		private static P startPoint;
		private static P currentPoint;

		private static Rect selection = null;

		private static WriteableBitmap bmp;

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

		private class Rect
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


		public static void Selection(WriteableBitmap b, int x, int y)
		{
			byte[] oldBytes = null;

			bmp = b;

			if (!isSelecting)
			{
				startPoint = new P(x, y);

				if(selectionMade)
				{
					selectionMade = false;
					oldBytes = History.Undo().bmp;
					bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
					bmp.Invalidate();
				}
			}

			currentPoint = new P(x, y);

			//make a rectangle to represent the selected area (probably store it statically)


			if (startPoint.x > currentPoint.x)
			{
				if (startPoint.y > currentPoint.y)
				{
					selection = new Rect(currentPoint.x, currentPoint.y, startPoint.x, startPoint.y);

				}
				else
				{
					selection = new Rect(currentPoint.x, startPoint.y, startPoint.x, currentPoint.y);

				}
			}
			else
			{
				if(startPoint.y < currentPoint.y)
				{
					selection = new Rect(startPoint.x, startPoint.y, currentPoint.x, currentPoint.y);
				}
				else
				{
					selection = new Rect(startPoint.x, currentPoint.y, currentPoint.x, startPoint.y);
				}
			}

			if (isSelecting)
			{
				oldBytes = History.Undo().bmp;
				bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
			}

			isSelecting = true;

			bmp.DrawRectangle(selection.topLeft.x, selection.topLeft.y, selection.bottomRight.x, selection.bottomRight.y, Colors.Black);

			oldBytes = bmp.PixelBuffer.ToArray();
			bmp.PixelBuffer.ToArray().CopyTo(oldBytes, 0);

			

			History.EndAction(new Action(oldBytes));
		}

		public static void ClearSelection()
		{
			//use the rectangle to clear the bytes within the selected area
			if(selectionMade)
			{
				byte[] oldBytes = History.Undo().bmp;
				bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);

				bmp.FillRectangle(selection.topLeft.x, selection.topLeft.y, selection.bottomRight.x + 1, selection.bottomRight.y + 1, Color.FromArgb(0, 0, 0, 0));
				selectionMade = false;

				oldBytes = bmp.PixelBuffer.ToArray();
				bmp.PixelBuffer.ToArray().CopyTo(oldBytes, 0);

				History.EndAction(new Action(oldBytes));
				bmp.Invalidate();
			}
			
		}


		public static void SelectRelease()
		{
			if (isSelecting)
			{
				isSelecting = false;
				selectionMade = true;
				byte[] oldBytes = History.Undo().bmp;
				bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
				bmp.Invalidate();

			}

		}

		public static void SelectionUndo()
		{
			selectionMade = false;
		}

		public static void ToolChanged()
		{
			if(selectionMade)
			{
				byte[] oldBytes = History.Undo().bmp;
				bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
				bmp.Invalidate();
			}

			selectionMade = false;
			selection = null;
			isSelecting = false;
			
		}

	}
}
