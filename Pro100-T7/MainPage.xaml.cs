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
using System;
using System.Threading;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pro100_T7 {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
public sealed partial class MainPage : Page {

WriteableBitmap bmp;
Point current = new Point();
Point betweem = new Point();
Point old = new Point();

public MainPage() {
    this.InitializeComponent();
    bmp = BitmapFactory.New((int)DrawArea.Width, (int)DrawArea.Height);
    //bmp = BitmapFactory.New(1500, 1000);
    //ApplicationView.PreferredLaunchViewSize = new Size(1750, 1250);
    //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
}

protected override void OnNavigatedTo(NavigationEventArgs e) {
    //base.OnNavigatedTo(e);

    PointerMoved += MainPage_PointerMoved;
}


private void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e) {

    #region Notes
    //bmp.SetPixel((int)current.X, (int)current.Y, Colors.Black);
    
    //Rects with outline, also gaps
    //bmp.DrawLineAa((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color, (int)brushSize.Value);    
    
    
    //Pen Like drawing - This could be a brush by its self.
    //bmp.DrawLineDDA((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color);
    
    //Filled Circles

    //Cant take in constant input
    //int[] points = { (int)old.X, (int)old.Y, (int)current.X, (int)current.Y};
    //bmp.FillCurveClosed(points, 0.5f,  colorPicker.Color);
    
    //int w = bmp.PixelWidth;
    //int h = bmp.PixelHeight;
    //WriteableBitmapExtensions.DrawLine(bmp.GetBitmapContext() , w, h, 1, 2, 30, 40, 255);
    
    //Draws lines spaced out
    //bmp.DrawLineBresenham((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color);
    
    //bmp.Invalidate();

    //Clears the canvas
    //bmp.Clear();

    //Used to pickup color
    //colorPicker.Color = bmp.GetPixel((int)current.X, (int)current.Y);
    #endregion

    #region Drawing1.0
    var pointerPosition = Window.Current.CoreWindow.PointerPosition;
    var x = pointerPosition.X - Window.Current.Bounds.X - 60;
    var y = pointerPosition.Y -Window.Current.Bounds.Y - 110;
    Pointer ptr = e.Pointer;
    Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(null);
    if (ptr.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse) {
    
    old = current;
    current.Y = y;
    current.X = x;

    Mouse.Text = "x:" + x + " | y:" + y;
    if (ptrPt.Properties.IsLeftButtonPressed) {
    
    drawing.Source = bmp;
    using (bmp.GetBitmapContext()) {
    bmp.FillEllipseCentered((int)current.X, (int)current.Y, (int)brushSize.Value, (int)brushSize.Value, colorPicker.Color);
    bmp.FillEllipseCentered((int)betweem.X, (int)betweem.Y, (int)brushSize.Value, (int)brushSize.Value, colorPicker.Color);
    bmp.FillEllipseCentered((int)old.X, (int)old.Y, (int)brushSize.Value, (int)brushSize.Value, colorPicker.Color);
    bmp.Invalidate();
    
    }
    }
    }
    
    if (ptrPt.Properties.IsRightButtonPressed) {
    
    bmp.FillEllipseCentered((int)current.X, (int)current.Y, (int)brushSize.Value, (int)brushSize.Value, Color.FromArgb(0, 0, 0, 0));
    bmp.FillEllipseCentered((int)old.X, (int)old.Y, (int)brushSize.Value, (int)brushSize.Value, Color.FromArgb(0, 0, 0, 0));
    bmp.Invalidate();
    
    }

    #endregion

    #region Drawing2.0

    //if (e.GetCurrentPoint(DrawArea).Properties.IsLeftButtonPressed) {
    //drawPoints.Add(e.GetCurrentPoint(DrawArea).Position);

    //if (drawPoints.Count() > 10) {
    //bmp.DrawCurve(PointsToInts(drawPoints), 0, Colors.Black);

    //drawPoints.RemoveAt(0);
    //}
    //} else {
    //drawPoints.Clear();
    //}
    //}
    #endregion
}

}

}


