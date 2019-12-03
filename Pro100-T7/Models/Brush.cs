using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Pro100_T7.UserControls;

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
        BrushModifierPanel Mod { get; set; }


        public Brush(int oldX, int oldY, int currentX, int currentY, Windows.UI.Color brushColor, int brushSize, WriteableBitmap bmp, BrushModifierPanel mod)
        {
            OldX = oldX;
            OldY = oldY;
            CurrentX = currentX;
            CurrentY = currentY;
            BrushColor = brushColor;
            BrushSize = brushSize;
            Bmp = bmp;
            Mod = mod;
        }

        public void Wavy() //Crash on going to top/bottom of image(Fixed) New issue very slow
        {
            //Wavy Kinda lines
            Bmp.DrawTriangle(OldX, OldY, OldX + BrushSize, OldY + BrushSize, OldX - BrushSize, OldY - BrushSize, BrushColor);
        }

        public void Regular() 
        { 
            Bmp.FillEllipseCentered(CurrentX, CurrentY, BrushSize, BrushSize, BrushColor);
            Bmp.DrawLineCustom(OldX, OldY, CurrentX, CurrentY, BrushColor, BrushSize * 2);
            //bmp.FillEllipseCentered((int)current.X, (int)current.Y, (int)brushSize.Value, (int)brushSize.Value, colorPicker.Color);
            //bmp.DrawLineAa((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color, (int)brushSize.Value * 2);

        }

        public void Double()
        {
            //Brush that draws two lines at once
            Bmp.FillEllipseCentered(CurrentX, CurrentY, BrushSize, BrushSize, BrushColor);
            Bmp.DrawLineCustom(OldX, OldY, CurrentX, CurrentY, BrushColor, BrushSize * 2);
            Bmp.FillEllipseCentered(CurrentX + BrushSize + 100, CurrentY + BrushSize + 100, BrushSize, BrushSize, BrushColor);
            Bmp.DrawLineCustom(OldX + BrushSize + 100, OldY + BrushSize + 100, CurrentX + BrushSize + 100, CurrentY + BrushSize + 100, BrushColor, BrushSize * 2);
        }

        //Brush that draws like a pen
        public void Pen() => Bmp.DrawLineDDA(OldX, OldY, CurrentX, CurrentY, BrushColor);

        public void Triangle() => Bmp.DrawTriangle(OldX, OldY, CurrentX + BrushSize, CurrentY + BrushSize, CurrentX - BrushSize, CurrentY - BrushSize, BrushColor);
        public void Hourglass() => Bmp.DrawQuad(OldX, OldY, OldX, OldY + BrushSize, CurrentX, CurrentY, CurrentX, CurrentY + BrushSize, BrushColor);

        public void Erase()
        { 
            Bmp.FillEllipseCentered(CurrentX, CurrentY, BrushSize, BrushSize, Color.FromArgb(0,0,0,0));
        }

        public void EyeDropper() { //Left Click Color
            if (Bmp.GetPixel(CurrentX, CurrentY) != Color.FromArgb(0,0,0,0)) Mod.GetColorPickerUIElement().Color = Bmp.GetPixel(CurrentX, CurrentY);
        }

        public void Clear()
        {
            Bmp.Clear();
        }

		public void Fill()
		{
			FillTool.Fill(Bmp, CurrentX, CurrentY, BrushColor);
			Bmp.Invalidate();
		}

		public void Selection()
		{
			SelectionTool.Selection(Bmp, CurrentX, CurrentY);
			Bmp.Invalidate();
			
		}
    }
}
