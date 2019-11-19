using Pro100_T7.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pro100_T7.UserControls {
public sealed partial class DrawingCanvas : UserControl {

DrawPoint drawPoint = new DrawPoint();
Stroke defaultStroke = new Stroke() { StrokeColor = Colors.OrangeRed, StrokeRadius = 15 };
CanvasMaster canvas = new CanvasMaster(1000, 800);


public DrawingCanvas() {
    this.InitializeComponent();
    GetControlCanvasUIElement().Children.Add(canvas.ImageData);
}

public Canvas GetControlCanvasUIElement() => DrawArea;

public void OnNavigatedTo(NavigationEventArgs e) {
    PointerReleased += ActionPointerReleased;
    PointerMoved += Canvas_PointerMoved;
}

private void ActionPointerReleased(object sender, PointerRoutedEventArgs e) {
    byte[] b1 = canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.ToArray();
    byte[] b = new byte[b1.Length];
    b1.CopyTo(b, 0);
    History.EndAction(new Models.Action(b));
}

private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e) {
    Point current = Window.Current.CoreWindow.PointerPosition;
    current.X += Window.Current.Bounds.X - 44;
    current.Y += Window.Current.Bounds.Y - 164;
    drawPoint.CurrentPoint = current;

    if (drawPoint.OldPoint == null) drawPoint.OldPoint = drawPoint.CurrentPoint;

    PointerPoint ptrPt = e.GetCurrentPoint(null);
    if (ptrPt.Properties.IsLeftButtonPressed) {
    
    canvas.ImageDataLayer.DrawBrush(defaultStroke, drawPoint);
    } else if (ptrPt.Properties.IsRightButtonPressed) {
    //canvas.ImageDataLayer.BitmapDrawingData.Clear();
    }
    drawPoint.OldPoint = drawPoint.CurrentPoint;
}

}

}
