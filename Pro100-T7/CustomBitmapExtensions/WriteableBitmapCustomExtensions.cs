﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Windows.UI.Xaml.Media.Imaging
{
	public static class WriteableBitmapCustomExtensions
	{
		private static readonly int[] leftEdgeX = new int[8192];
		private static readonly int[] rightEdgeX = new int[8192];
		/// <summary> 
		/// Draws a normal line with a desired stroke thickness
		/// <param name="context">The context containing the pixels as int RGBA value.</param>
		/// <param name="x1">The x-coordinate of the start point.</param>
		/// <param name="y1">The y-coordinate of the start point.</param>
		/// <param name="x2">The x-coordinate of the end point.</param>
		/// <param name="y2">The y-coordinate of the end point.</param>
		/// <param name="color">The color for the line.</param>
		/// <param name="strokeThickness">The stroke thickness of the line.</param>
		/// </summary>
		public static void DrawLineCustom(BitmapContext context, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color, int strokeThickness, Rect? clipRect = null)
		{
			CustomWidthLine(pixelWidth, pixelHeight, context, x1, y1, x2, y2, strokeThickness, color, clipRect);
		}

		/// <summary> 
		/// Draws a normal line with a desired stroke thickness
		/// <param name="bmp">The WriteableBitmap.</param>
		/// <param name="x1">The x-coordinate of the start point.</param>
		/// <param name="y1">The y-coordinate of the start point.</param>
		/// <param name="x2">The x-coordinate of the end point.</param>
		/// <param name="y2">The y-coordinate of the end point.</param>
		/// <param name="color">The color for the line.</param>
		/// <param name="strokeThickness">The stroke thickness of the line.</param>
		/// </summary>
		public static void DrawLineCustom(this WriteableBitmap bmp, int x1, int y1, int x2, int y2, int color, int strokeThickness, Rect? clipRect = null)
		{
			using (var context = bmp.GetBitmapContext())
			{
				CustomWidthLine(bmp.PixelWidth, bmp.PixelHeight, context, x1, y1, x2, y2, strokeThickness, color, clipRect);
			}
		}

		/// <summary> 
		/// Draws a normal line with a desired stroke thickness
		/// <param name="context">The context containing the pixels as int RGBA value.</param>
		/// <param name="x1">The x-coordinate of the start point.</param>
		/// <param name="y1">The y-coordinate of the start point.</param>
		/// <param name="x2">The x-coordinate of the end point.</param>
		/// <param name="y2">The y-coordinate of the end point.</param>
		/// <param name="color">The color for the line.</param>
		/// <param name="strokeThickness">The stroke thickness of the line.</param>
		/// </summary>
		public static void DrawLineCustom(BitmapContext context, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, Color color, int strokeThickness, Rect? clipRect = null)
		{
			var col = ((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);

			CustomWidthLine(pixelWidth, pixelHeight, context, x1, y1, x2, y2, strokeThickness, col, clipRect);
		}

		/// <summary> 
		/// Draws a normal line with a desired stroke thickness
		/// <param name="bmp">The WriteableBitmap.</param>
		/// <param name="x1">The x-coordinate of the start point.</param>
		/// <param name="y1">The y-coordinate of the start point.</param>
		/// <param name="x2">The x-coordinate of the end point.</param>
		/// <param name="y2">The y-coordinate of the end point.</param>
		/// <param name="color">The color for the line.</param>
		/// <param name="strokeThickness">The stroke thickness of the line.</param>
		/// </summary>
		public static void DrawLineCustom(this WriteableBitmap bmp, int x1, int y1, int x2, int y2, Color color, int strokeThickness, Rect? clipRect = null)
		{
			int intColor = ((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
			using (var context = bmp.GetBitmapContext())
			{
				CustomWidthLine(bmp.PixelWidth, bmp.PixelHeight, context, x1, y1, x2, y2, strokeThickness, intColor, clipRect);

			}

		}


		private static void CustomWidthLine(int width, int height, BitmapContext context, float x1, float y1, float x2, float y2, float lineWidth, Int32 color, Rect? clipRect = null)
		{
			// Perform cohen-sutherland clipping if either point is out of the viewport
			//if (!CohenSutherlandLineClip(clipRect ?? new Rect(0, 0, width, height), ref x1, ref y1, ref x2, ref y2)) return;

			if (lineWidth <= 0) return;

			var buffer = context.Pixels;

			if (y1 > y2)
			{
				Swap(ref x1, ref x2);
				Swap(ref y1, ref y2);
			}

			if (x1 == x2)
			{
				x1 -= (int)lineWidth / 2;
				x2 += (int)lineWidth / 2;

				if (x1 < 0)
					x1 = 0;
				if (x2 < 0)
					return;

				if (x1 >= width)
					return;
				if (x2 >= width)
					x2 = width - 1;

				if (y1 >= height || y2 < 0)
					return;

				if (y1 < 0)
					y1 = 0;
				if (y2 >= height)
					y2 = height - 1;

				for (var x = (int)x1; x <= x2; x++)
				{
					for (var y = (int)y1; y <= y2; y++)
					{
						var a = (byte)((color & 0xff000000) >> 24);
						var r = (byte)((color & 0x00ff0000) >> 16);
						var g = (byte)((color & 0x0000ff00) >> 8);
						var b = (byte)((color & 0x000000ff) >> 0);

						buffer[y * width + x] = (a << 24) | (r << 16) | (g << 8) | (b << 0);
					}
				}

				return;
			}
			if (y1 == y2)
			{
				if (x1 > x2) Swap(ref x1, ref x2);

				y1 -= (int)lineWidth / 2;
				y2 += (int)lineWidth / 2;

				if (y1 < 0) y1 = 0;
				if (y2 < 0) return;

				if (y1 >= height) return;
				if (y2 >= height) y2 = height - 1;

				if (x1 >= width || y2 < 0) return;

				if (x1 < 0) x1 = 0;
				if (x2 >= width) x2 = width - 1;

				for (var x = (int)x1; x <= x2; x++)
				{
					for (var y = (int)y1; y <= y2; y++)
					{
						var a = (byte)((color & 0xff000000) >> 24);
						var r = (byte)((color & 0x00ff0000) >> 16);
						var g = (byte)((color & 0x0000ff00) >> 8);
						var b = (byte)((color & 0x000000ff) >> 0);


						buffer[y * width + x] = (a << 24) | (r << 16) | (g << 8) | (b << 0);
					}
				}

				return;
			}

			y1 += 1;
			y2 += 1;

			float slope = (y2 - y1) / (x2 - x1);
			float islope = (x2 - x1) / (y2 - y1);

			float m = slope;
			float w = lineWidth;

			float dx = x2 - x1;
			float dy = y2 - y1;

			var xtot = (float)(w * dy / Math.Sqrt(dx * dx + dy * dy));
			var ytot = (float)(w * dx / Math.Sqrt(dx * dx + dy * dy));

			float sm = dx * dy / (dx * dx + dy * dy);

			// Center it.

			x1 += xtot / 2;
			y1 -= ytot / 2;
			x2 += xtot / 2;
			y2 -= ytot / 2;

			//
			//

			float sx = -xtot;
			float sy = +ytot;

			var ix1 = (int)x1;
			var iy1 = (int)y1;

			var ix2 = (int)x2;
			var iy2 = (int)y2;

			var ix3 = (int)(x1 + sx);
			var iy3 = (int)(y1 + sy);

			var ix4 = (int)(x2 + sx);
			var iy4 = (int)(y2 + sy);

			if (ix1 == ix2)
			{
				ix2++;
			}
			if (ix3 == ix4)
			{
				ix4++;
			}

			if (lineWidth == 2)
			{
				if (Math.Abs(dy) < Math.Abs(dx))
				{
					if (x1 < x2)
					{
						iy3 = iy1 + 2;
						iy4 = iy2 + 2;
					}
					else
					{
						iy1 = iy3 + 2;
						iy2 = iy4 + 2;
					}
				}
				else
				{
					ix1 = ix3 + 2;
					ix2 = ix4 + 2;
				}
			}

			int starty = Math.Min(Math.Min(iy1, iy2), Math.Min(iy3, iy4));
			int endy = Math.Max(Math.Max(iy1, iy2), Math.Max(iy3, iy4));

			if (starty < 0) starty = -1;
			if (endy >= height) endy = height + 1;

			for (int y = starty + 1; y < endy - 1; y++)
			{
				leftEdgeX[y] = -1 << 16;
				rightEdgeX[y] = 1 << 16 - 1;
			}


			AALineQ1(width, height, context, ix1, iy1, ix2, iy2, color, sy > 0, false);
			AALineQ1(width, height, context, ix3, iy3, ix4, iy4, color, sy < 0, true);

			if (lineWidth > 1)
			{
				AALineQ1(width, height, context, ix1, iy1, ix3, iy3, color, true, sy > 0);
				AALineQ1(width, height, context, ix2, iy2, ix4, iy4, color, false, sy < 0);
			}

			if (x1 < x2)
			{
				if (iy2 >= 0 && iy2 < height) rightEdgeX[iy2] = Math.Min(ix2, rightEdgeX[iy2]);
				if (iy3 >= 0 && iy3 < height) leftEdgeX[iy3] = Math.Max(ix3, leftEdgeX[iy3]);
			}
			else
			{
				if (iy1 >= 0 && iy1 < height) rightEdgeX[iy1] = Math.Min(ix1, rightEdgeX[iy1]);
				if (iy4 >= 0 && iy4 < height) leftEdgeX[iy4] = Math.Max(ix4, leftEdgeX[iy4]);
			}

			//return;

			for (int y = starty + 1; y < endy - 1; y++)
			{
				leftEdgeX[y] = Math.Max(leftEdgeX[y], 0);
				rightEdgeX[y] = Math.Min(rightEdgeX[y], width - 1);

				for (int x = leftEdgeX[y]; x <= rightEdgeX[y]; x++)
				{
					var a = (byte)((color & 0xff000000) >> 24);
					var r = (byte)((color & 0x00ff0000) >> 16);
					var g = (byte)((color & 0x0000ff00) >> 8);
					var b = (byte)((color & 0x000000ff) >> 0);

					buffer[y * width + x] = (a << 24) | (r << 16) | (g << 8) | (b << 0);
				}
			}
		}

		private static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		private static void AALineQ1(int width, int height, BitmapContext context, int x1, int y1, int x2, int y2, Int32 color, bool minEdge, bool leftEdge)
		{
			Byte off = 0;

			if (minEdge) off = 0xff;

			if (x1 == x2) return;
			if (y1 == y2) return;

			var buffer = context.Pixels;

			if (y1 > y2)
			{
				Swap(ref x1, ref x2);
				Swap(ref y1, ref y2);
			}

			int deltax = (x2 - x1);
			int deltay = (y2 - y1);

			if (x1 > x2) deltax = (x1 - x2);

			int x = x1;
			int y = y1;

			UInt16 m = 0;

			if (deltax > deltay) m = (ushort)(((deltay << 16) / deltax));
			else m = (ushort)(((deltax << 16) / deltay));

			UInt16 e = 0;

			var a = (byte)((color & 0xff000000) >> 24);
			var r = (byte)((color & 0x00ff0000) >> 16);
			var g = (byte)((color & 0x0000ff00) >> 8);
			var b = (byte)((color & 0x000000ff) >> 0);


			e = 0;

			if (deltax >= deltay)
			{
				while (deltax-- != 0)
				{
					if ((UInt16)(e + m) <= e) // Roll
					{
						y++;
					}

					e += m;

					if (x1 < x2) x++;
					else x--;

					if (y < 0 || y >= height) continue;

					if (leftEdge) leftEdgeX[y] = Math.Max(x + 1, leftEdgeX[y]);
					else rightEdgeX[y] = Math.Min(x - 1, rightEdgeX[y]);

					if (x < 0 || x >= width) continue;

					//

					buffer[y * width + x] = (a << 24) | (r << 16) | (g << 8) | (b << 0);

					//
				}
			}
			else
			{
				off ^= 0xff;

				while (--deltay != 0)
				{
					if ((UInt16)(e + m) <= e) // Roll
					{
						if (x1 < x2) x++;
						else x--;
					}

					e += m;

					y++;

					if (x < 0 || x >= width) continue;
					if (y < 0 || y >= height) continue;

					//

					buffer[y * width + x] = (a << 24) | (r << 16) | (g << 8) | (b << 0);

					if (leftEdge) leftEdgeX[y] = x + 1;
					else rightEdgeX[y] = x - 1;
				}
			}
		}

	}
}
