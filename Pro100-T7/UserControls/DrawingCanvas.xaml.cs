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

private CanvasMaster canvas;
private static Color color;
private static Color secondary;
private int size;
private int type = 0;
private bool isMouseDownOnCanvas = false;
private DispatcherTimer dt = new DispatcherTimer();

public BrushModifierPanel BrushMod { get; set; }

public CanvasMaster Canvas {
    get { return canvas; }
    set { canvas = value; }
}

public Color Color {
    get { return color; }
    set { color = value; }
}

public Color Secondary {
    get { return secondary; }
    set { secondary = value; }
}
public int Size {
    get { return size; }
    set { size = value; }
}
public int Type {
    get { return type; }
    set { type = value; }
}
private PointerPoint ptrPt;
DrawPoint drawPoint = new DrawPoint();
Stroke defaultStroke = new Stroke() { StrokeColor = color, StrokeRadius = 1 };

public DrawingCanvas() {
    this.InitializeComponent();
    dt.Interval = TimeSpan.FromSeconds(1/120);
    dt.Tick += Dt_Tick;
    dt.Start();
    canvas = new CanvasMaster(1000, 800);
    byte[] b1 = canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.ToArray();
    byte[] b = new byte[b1.Length];
    b1.CopyTo(b, 0);
    History.StartHistory(b1);
    GetControlCanvasUIElement().Children.Add(canvas.ImageData);
    PointerReleased += ActionPointerReleased;
    PointerMoved += Canvas_PointerMoved;
    PointerPressed += DrawingCanvas_PointerPressed;
}
private void Dt_Tick(object sender, object e) {
    if (ptrPt != null && type != 10) {
    if (ptrPt.Properties.IsLeftButtonPressed) {
    defaultStroke.StrokeColor = color;
    canvas.ImageDataLayer.DrawBrush(defaultStroke, drawPoint, type, 1);

    } else if (ptrPt.Properties.IsRightButtonPressed) {
    defaultStroke.StrokeColor = secondary;
    canvas.ImageDataLayer.DrawBrush(defaultStroke, drawPoint, type, 2);
    }
    drawPoint.OldPoint = drawPoint.CurrentPoint;
    }
}

private void DrawingCanvas_PointerPressed(object sender, PointerRoutedEventArgs e) {
	Canvas_PointerMoved(sender, e);
   if(isMouseDownOnCanvas) {
    ActionPointerReleased(sender, e);
    }
	isMouseDownOnCanvas = true;
	if(type != 10)
	{
		SelectionTool.ToolChanged();
	}
}

public void ActionPointerReleased(object sender, PointerRoutedEventArgs e) {

   if(isMouseDownOnCanvas) {
    byte[] b1 = canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.ToArray();
	byte[] b = new byte[b1.Length];
	b1.CopyTo(b, 0);
	History.EndAction(new Models.Action(b));
	isMouseDownOnCanvas = false;
    }

	if(type == 10)
	{
		SelectionTool.SelectRelease();
	}

	canvas.ImageDataLayer.BitmapDrawingData.Invalidate();

}

private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e) {
    defaultStroke.StrokeRadius = size;

	PointerPoint current = e.GetCurrentPoint(DrawArea);
    drawPoint.CurrentPoint = new Point(current.Position.X, current.Position.Y);

    if (drawPoint.OldPoint == null) drawPoint.OldPoint = drawPoint.CurrentPoint;
    ptrPt = e.GetCurrentPoint(null);
        
    if (type == 10) {
    if (ptrPt.Properties.IsLeftButtonPressed) {
    defaultStroke.StrokeColor = color;
    canvas.ImageDataLayer.DrawBrush(defaultStroke, drawPoint, type, 1);

    } else if (ptrPt.Properties.IsRightButtonPressed) {
    defaultStroke.StrokeColor = secondary;
    canvas.ImageDataLayer.DrawBrush(defaultStroke, drawPoint, type, 2);
    }
    drawPoint.OldPoint = drawPoint.CurrentPoint;
    }
}

}

}
