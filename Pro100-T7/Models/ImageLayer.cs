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

        public void DrawBrush(Stroke stroke, DrawPoint drawPoint)
        {
            using (BitmapDrawingData.GetBitmapContext())
            {
                //draw code will go here and modify the image accordingly
                BitmapDrawingData.DrawLineAa(drawPoint.OldX(), drawPoint.OldY(), drawPoint.NewX(), drawPoint.NewY(), stroke.StrokeColor, stroke.StrokeRadius);
            }

            ImageDataLayerModifiedEvent.Invoke();
        }
    }
}
