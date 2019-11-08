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
        public Image Image { get; private set; } = new Image();
        public uint ImageID { get; private set; } = uint.MaxValue;

        /// <summary>
        /// Creates a new ImageLayer which corresponds to the supplied indexID
        /// </summary>
        /// <param name="indexID">Corespondong ID this new ImageLayer will associate to</param>
        public ImageLayer(uint indexID)
        {
            ImageID = indexID;
        }

        public void DrawBrush(Stroke stroke, DrawPoint drawPoint)
        {
            //draw code will go here and modify the image accordingly
        }

    }
}
