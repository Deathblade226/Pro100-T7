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
		public static bool selectionMade = false;
		private static bool selectionDisplaying = false;

		private static P startPoint;
		private static P currentPoint;

		private static Rect selection = null;

		private static WriteableBitmap bmp;

		private static byte[] beforeSelection;

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

				if (selectionMade)
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
				if (startPoint.y < currentPoint.y)
				{
					selection = new Rect(startPoint.x, startPoint.y, currentPoint.x, currentPoint.y);
				}
				else if(startPoint.y > currentPoint.y)
				{
					selection = new Rect(startPoint.x, currentPoint.y, currentPoint.x, startPoint.y);
				}
			}
			

			isSelecting = true;
			if (selection == null)
			{

				return;
			}

			bmp.DrawRectangle(selection.topLeft.x, selection.topLeft.y, selection.bottomRight.x, selection.bottomRight.y, Colors.Black);
			selectionDisplaying = true;
			if (isSelecting && selection != null)
			{
				oldBytes = History.Undo().bmp;
				bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
			}

			oldBytes = bmp.PixelBuffer.ToArray();
			bmp.PixelBuffer.ToArray().CopyTo(oldBytes, 0);



			History.EndAction(new Action(oldBytes));
		}

		public static void ClearSelection()
		{
			//use the rectangle to clear the bytes within the selected area
			if (selectionMade && selectionDisplaying)
			{
				byte[] oldBytes = History.Undo().bmp;
				bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);

				bmp.FillRectangle(selection.topLeft.x, selection.topLeft.y, selection.bottomRight.x + 1, selection.bottomRight.y + 1, Color.FromArgb(0, 0, 0, 0));
				selectionMade = false;
				selectionDisplaying = false;

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
				if(selection != null)
				{
					selectionMade = true;
				}
				//byte[] oldBytes = History.Undo().bmp;
				//bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
				//bmp.Invalidate();

			}

		}


		public static void ToolChanged(int type)
		{
			if (type == 9)//bucket fill tool
			{
				isSelecting = false;
				if (selectionMade && !selectionDisplaying)
				{
					selection = null;
					selectionMade = false;
				}
			}
			else if (type != 10)//selection tool
			{
				if (selectionMade && selectionDisplaying)
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
		public static void RedrawSelection()
		{
			if (selectionMade && !selectionDisplaying)
			{
				//first do the draw logic for the rectangle
				WriteableBitmap oldbmp = new WriteableBitmap(bmp.PixelWidth, bmp.PixelHeight);
				oldbmp.PixelBuffer.AsStream().Write(beforeSelection, 0, beforeSelection.Length);
				using (BitmapReader oldReader = new BitmapReader(oldbmp))
				{
					using (BitmapReader newReader = new BitmapReader(bmp))
					{
						//top line
						for (int i = selection.topLeft.x; i <= selection.bottomRight.x; i++)
						{
							int color = oldReader.GetPixeli(i, selection.topLeft.y);
							newReader.SetPixel(i, selection.topLeft.y, color);
						}
						//bottom line
						for (int i = selection.topLeft.x; i <= selection.bottomRight.x; i++)
						{
							int color = oldReader.GetPixeli(i, selection.bottomRight.y);
							newReader.SetPixel(i, selection.bottomRight.y, color);
						}
						//left line
						for (int i = selection.topLeft.y; i <= selection.bottomRight.y; i++)
						{
							int color = oldReader.GetPixeli(selection.topLeft.x, i);
							newReader.SetPixel(selection.topLeft.x, i, color);
						}
						//right
						for (int i = selection.topLeft.y; i <= selection.bottomRight.y; i++)
						{
							int color = oldReader.GetPixeli(selection.bottomRight.x, i);
							newReader.SetPixel(selection.bottomRight.x, i, color);
						}
					}
				}

				byte[] b = new byte[beforeSelection.Length];
				bmp.PixelBuffer.ToArray().CopyTo(b, 0);

				History.EndAction(new Action(b));

				bmp.DrawRectangle(selection.topLeft.x, selection.topLeft.y, selection.bottomRight.x, selection.bottomRight.y, Colors.Black);
				bmp.Invalidate();
			}

		}
		public static void StoreSelection()
		{
			if (selectionMade)
			{
				Action a = History.Undo();
				byte[] b = History.PeekHistory().bmp;
				beforeSelection = new byte[b.Length];
				b.CopyTo(beforeSelection, 0);
				History.EndAction(a);
			}
		}

		public static bool IsInSelection(int x, int y)
		{
			if (selection != null && selectionMade && x >= selection.topLeft.x && x <= selection.bottomRight.x && y >= selection.topLeft.y && y <= selection.bottomRight.y)
			{
				return true;
			}
			return false;
		}

		public static void UndoSelection()
		{
			if(selectionMade)
			{
				selectionMade = false;
			}
		}

		public static void RedoSelection()
		{
			if(!selectionMade && selection != null)
			{
				selectionMade = true;
			}
		}

	}
}
