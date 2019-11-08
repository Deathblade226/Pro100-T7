﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
    public class CanvasMaster
    {
        public Canvas UICanvasObject { get; private set; }

        public WriteableBitmap CanvasData { get; private set; }
        public List<ImageLayer> ImageLayers { get; private set; }

        /// <summary>
        /// Creates a boiler-plate WritableBitMap Canvas element with drawing capability using the supplied width and heights.
        /// </summary>
        /// <param name="pixelWidth">Drawing space width</param>
        /// <param name="pixelHeight">Drawing space height</param>
        public CanvasMaster(int pixelWidth, int pixelHeight)
        {
            CanvasData = BitmapFactory.New(pixelWidth, pixelHeight);
            ImageLayers.Add(new ImageLayer(0));
        }

        /// <summary>
        /// Creates a boiler-plate WritableBitMap Canvas element with drawing capability using the supplied width and heights.
        /// Initializes the WritableBitMap with a supplied array of ImageLayer to initialize the canvas with preexisting drawing data.
        /// </summary>
        /// <param name="pixelWidth">Drawing space width</param>
        /// <param name="pixelHeight">Drawing space height</param>
        /// <param name="imageLayers">Supplied ImageLayer layers containing preexisting drawing data</param>
        public CanvasMaster(int pixelWidth, int pixelHeight, ImageLayer[] imageLayers)
        {
            CanvasData = BitmapFactory.New(pixelWidth, pixelHeight);
            ImageLayers = imageLayers.ToList();
        }
    }
}
