﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{

	public static class StraightLineTool
	{
		private static WriteableBitmap bmp;
		private static bool isSelecting = false;
		//private static bool lineDrawn = false;

		private static P startPoint;
		private static P currentPoint;

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

		public static void StraightLine(WriteableBitmap b, int x, int y, int width, Color color)
		{
			byte[] oldBytes = null;

			bmp = b;

			if (!isSelecting)
			{
				startPoint = new P(x, y);

			}

			currentPoint = new P(x, y);


			if (isSelecting)
			{
				oldBytes = History.Undo().bmp;
				bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
			}

			isSelecting = true;

			bmp.DrawLineCustom(startPoint.x, startPoint.y, currentPoint.x, currentPoint.y, color, width);

			oldBytes = bmp.PixelBuffer.ToArray();
			bmp.PixelBuffer.ToArray().CopyTo(oldBytes, 0);



			History.EndAction(new Action(oldBytes));
		}

		public static void StraightLineRelease()
		{
			if (isSelecting)
			{
				isSelecting = false;
				//byte[] oldBytes = History.Undo().bmp;
				//bmp.PixelBuffer.AsStream().Write(oldBytes, 0, oldBytes.Length);
				//bmp.Invalidate();

			}
		}

	}
}
