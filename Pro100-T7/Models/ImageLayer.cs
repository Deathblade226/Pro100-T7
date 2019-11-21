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

        public ImageLayer(int pixelWidth, int pixelHeight) 
        {
            BitmapDrawingData = BitmapFactory.New(pixelWidth, pixelHeight);
        }

        public void DrawBrush(Stroke stroke, DrawPoint drawPoint, int type = 0)
        {
        Brush brush = new Brush(drawPoint.OldX(), drawPoint.OldY(), drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeColor, stroke.StrokeRadius,BitmapDrawingData);
        
        switch(type){
        case 0: brush.Regular(); break;
        case 1: brush.Wavy();  break;
        case 2: brush.Double();  break;
        case 3: brush.Pen();  break;
        }

            //draw code will go here and modify the image accordingly
            //BitmapDrawingData.FillEllipseCentered(drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeRadius, stroke.StrokeRadius, stroke.StrokeColor);
            //BitmapDrawingData.DrawLineAa(drawPoint.OldX(), drawPoint.OldY(), drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeColor, stroke.StrokeRadius * 2);

            ImageDataLayerModifiedEvent.Invoke();
        }
        public void DrawEllipse(Stroke stroke, DrawPoint drawPoint) {
            Brush brush = new Brush(drawPoint.CurX(), drawPoint.CurY(), drawPoint.CurX(), drawPoint.CurY(), stroke.StrokeColor, stroke.StrokeRadius, BitmapDrawingData);
            brush.Regular();
            ImageDataLayerModifiedEvent.Invoke();
        }
    }
}
