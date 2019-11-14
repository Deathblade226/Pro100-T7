using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace Pro100_T7.Models
{
    class Brush
    {
        public int OldX { get; set; }
        public int OldY { get; set; }
        int CurrentX { get; set; }
        int CurrentY { get; set; }
        Windows.UI.Color BrushColor { get; set; }
        WriteableBitmap Bmp { get; set; }
        BitmapContext BC { get; set; }
        int BrushSize { get; set; }

        public Brush(int oldX, int oldY, int currentX, int currentY, Windows.UI.Color brushColor, int brushSize, WriteableBitmap bmp)
        {
            OldX = oldX;
            OldY = oldY;
            CurrentX = currentX;
            CurrentY = currentY;
            BrushColor = brushColor;
            BrushSize = brushSize;
            Bmp = bmp;
        }

        public void Wavy()
        {
            //Wavy Kinda lines
            for (int i = 1; i <= BrushSize; i++)
            {
                Bmp.SetPixel(CurrentX + i, CurrentY + i, 0, 0, 0);
            }
        }

        public void Regular() => Bmp.FillEllipseCentered(CurrentX, CurrentY, BrushSize, BrushSize, BrushColor);

        public void Double()
        {
            //Brush that draws two lines at once
            Bmp.FillEllipseCentered(CurrentX, CurrentY, BrushSize, BrushSize, BrushColor);
            Bmp.FillEllipseCentered(CurrentX + BrushSize + 20, CurrentY + BrushSize + 20, BrushSize, BrushSize, BrushColor);
        }

        //Brush that draws like a pen
        public void Pen() => Bmp.DrawLineDDA(OldX, OldY, CurrentX, CurrentY, BrushColor);
    }
}
