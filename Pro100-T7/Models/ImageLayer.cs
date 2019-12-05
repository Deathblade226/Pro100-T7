using Pro100_T7.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
    public class ImageLayer
    {
        public event ImageDataLayerModified ImageDataLayerModifiedEvent;
        public WriteableBitmap BitmapDrawingData { get; set; }
        public BrushModifierPanel BrushMod { get; set; }
        public ImageLayer(int pixelWidth, int pixelHeight) 
        {
            BitmapDrawingData = BitmapFactory.New(pixelWidth, pixelHeight);
        }

        public void DrawBrush(Stroke stroke, DrawPoint drawPoint, int type = 0, int click = 1)
        {
        Brush brush = new Brush(drawPoint.OldX(), drawPoint.OldY(), drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeColor, stroke.StrokeRadius, BitmapDrawingData, BrushMod);

        switch(type){
        case 0: brush.Regular(); break;
        case 1: brush.Wavy();  break;
        case 2: brush.Double();  break;
        case 3: brush.Pen();  break;
        case 4: brush.Clear(); break;
        case 5: brush.Triangle(); break;
        case 6: brush.Hourglass(); break;
        case 7: brush.Erase(); break;
        case 8: brush.EyeDropper(); break;
        case 9: brush.Fill(); break;
		case 10: brush.Selection(); break;

        }

            //draw code will go here and modify the image accordingly
            //BitmapDrawingData.FillEllipseCentered(drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeRadius, stroke.StrokeRadius, stroke.StrokeColor);
            //BitmapDrawingData.DrawLineAa(drawPoint.OldX(), drawPoint.OldY(), drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeColor, stroke.StrokeRadius * 2);

            ImageDataLayerModifiedEvent.Invoke();
        }
        public void DrawEllipse(Stroke stroke, DrawPoint drawPoint) {
            Brush brush = new Brush(drawPoint.CurX(), drawPoint.CurY(), drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeColor, stroke.StrokeRadius, BitmapDrawingData, BrushMod);
            ImageDataLayerModifiedEvent.Invoke();
        }
    }
}
