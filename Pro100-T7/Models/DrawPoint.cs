using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Pro100_T7.Models
{
    public class DrawPoint
    {
        public Point? OldPoint { get; set; } = null;
        public Point? CurrentPoint { get; set; } = null;
        
        /// <summary>
        /// Create an empty DrawPoint object ready to store two points - an old location and a new location
        /// </summary>
        public DrawPoint() { }

        /// <summary>
        /// Create a DrawPoint object with already existing Points.
        /// </summary>
        /// <param name="oldPoint">Preexisting Point defining the old Point</param>
        /// <param name="newPoint">Preexisting Point defining the new Point</param>
        public DrawPoint(Point oldPoint, Point currentPoint)
        {
            OldPoint = oldPoint; CurrentPoint = currentPoint;
        }

        public int OldX() => (int)OldPoint?.X;
        public int OldY() => (int)OldPoint?.Y;
        public int CurX() => (int)CurrentPoint?.X;
        public int CurY() => (int)CurrentPoint?.Y;
    }
}
