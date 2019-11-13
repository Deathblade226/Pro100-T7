using Windows.System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;
using Pro100_T7.Models;
using Windows.UI.Input;
using System;
using System.Threading;
using Pro100_T7.Models;
using Windows.Storage.Pickers;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pro100_T7
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CanvasMaster canvas = new CanvasMaster(1000, 800);
        DrawPoint drawPoint = new DrawPoint();

        Stroke defaultStroke = new Stroke() { StrokeColor = Colors.OrangeRed, StrokeRadius = 15 };

        public MainPage()
        {
            this.InitializeComponent();

            DrawArea.Children.Add(canvas.ImageData);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PointerMoved += MainPage_PointerMoved;
        }

        private void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Point current = Window.Current.CoreWindow.PointerPosition;
            current.X += Window.Current.Bounds.X - 44;
            current.Y += Window.Current.Bounds.Y - 164;
            drawPoint.CurrentPoint = current;            

            if (drawPoint.OldPoint == null) drawPoint.OldPoint = drawPoint.CurrentPoint;

            PointerPoint ptrPt = e.GetCurrentPoint(null);
            if (ptrPt.Properties.IsLeftButtonPressed)
            {
                canvas.ImageDataLayer.DrawBrush(defaultStroke, drawPoint);
            }
            else if (ptrPt.Properties.IsRightButtonPressed)
            {
                canvas.ImageDataLayer.BitmapDrawingData.Clear();
            }
            drawPoint.OldPoint = drawPoint.CurrentPoint;
        }
    }
}