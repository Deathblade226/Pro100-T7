using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Pro100_T7.Models
{
    public class ImageLayer
    {
        public Image Image { get; private set; } = new Image();
        public int ImageID { get; private set; } = -1;

        /// <summary>
        /// Creates a new ImageLayer which corresponds to the supplied indexID
        /// </summary>
        /// <param name="indexID">Corespondong ID this new ImageLayer will associate to</param>
        public ImageLayer(int indexID)
        {
            ImageID = indexID;
        }

        public void DrawBrush()
        {

        }

    }
}
