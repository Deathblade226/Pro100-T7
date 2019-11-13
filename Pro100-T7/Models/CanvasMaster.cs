using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
    public delegate void ImageDataLayerModified();

    public class CanvasMaster
    {
        public Canvas UICanvasObject { get; private set; }

        public Image ImageData { get; set; }
        public ImageLayer ImageDataLayer { get; private set; }

        /// <summary>
        /// Creates a boiler-plate WritableBitMap Canvas element with drawing capability using the supplied width and heights.
        /// </summary>
        /// <param name="pixelWidth">Drawing space width</param>
        /// <param name="pixelHeight">Drawing space height</param>
        public CanvasMaster(int pixelWidth, int pixelHeight)
        {
            ImageDataLayer = new ImageLayer(pixelWidth, pixelHeight);
            ImageData = new Image();

            ImageDataLayer.ImageDataLayerModifiedEvent += UpdateImageData;
        }

        /// <summary>
        /// Creates a boiler-plate WritableBitMap Canvas element with drawing capability using the supplied width and heights.
        /// Initializes the WritableBitMap with a supplied array of ImageLayer to initialize the canvas with preexisting drawing data.
        /// </summary>
        /// <param name="pixelWidth">Drawing space width</param>
        /// <param name="pixelHeight">Drawing space height</param>
        /// <param name="imageLayers">Supplied ImageLayer layers containing preexisting drawing data</param>
        public CanvasMaster(ImageLayer imageLayer)
        {
            ImageDataLayer = imageLayer;
            ImageData = new Image() { Source = ImageDataLayer.BitmapDrawingData };

            ImageDataLayer.ImageDataLayerModifiedEvent += UpdateImageData;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawLayerToCanvas()
        {
            DrawLayerToWritableBitmap();
            DrawWritableBitmapToUICanvasElement();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawLayerToWritableBitmap()
        {
            //combine all layers into one WritableBitmap object
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawWritableBitmapToUICanvasElement()
        {
            //apply the complete writable bitmap to the visible canvas element
        }

        private void UpdateImageData()
        {
            ImageData.Source = ImageDataLayer.BitmapDrawingData;
        }
    }
}
