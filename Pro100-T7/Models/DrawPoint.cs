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
        public Point OldPoint { get; set; }
        public Point NewPoint { get; set; }
        
        /// <summary>
        /// Create an empty DrawPoint object ready to store two points which will be drawn between
        /// </summary>
        public DrawPoint() { }

        /// <summary>
        /// Create a DrawPoint object with already existing Points.
        /// </summary>
        /// <param name="oldPoint">Preexisting Point defining the old Point</param>
        /// <param name="newPoint">Preexisting Point defining the new Point</param>
        public DrawPoint(Point oldPoint, Point newPoint)
        {
            OldPoint = oldPoint; NewPoint = NewPoint;
        }
    }
}
