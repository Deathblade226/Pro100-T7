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
using Pro100_T7.Models;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pro100_T7
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
public sealed partial class MainPage : Page {

bool debug = true;
FileSavePicker fileSavePicker = new FileSavePicker();
WriteableBitmap bmp;
Point current = new Point();
Point old = new Point();
bool newFile = true;

public MainPage() {
    this.InitializeComponent();
    bmp = BitmapFactory.New((int)DrawArea.Width, (int)DrawArea.Height);
    Models.History.StartHistory(bmp.PixelBuffer.ToArray());
    //bmp = BitmapFactory.New(1500, 1000);
    //ApplicationView.PreferredLaunchViewSize = new Size(1750, 1250);
    //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
}

protected override void OnNavigatedTo(NavigationEventArgs e) {
    base.OnNavigatedTo(e);
    KeyDown += KeyPressed;
    PointerMoved += MainPage_PointerMoved;
    DrawArea.PointerReleased += MainPage_PointerReleased;
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
    
    using (bmp.GetBitmapContext()) {
    bmp.FillEllipseCentered((int)current.X, (int)current.Y, (int)brushSize.Value, (int)brushSize.Value, colorPicker.Color);
    bmp.Invalidate();
    
    }
    }
    }
    
    if (ptrPt.Properties.IsRightButtonPressed) {
    
    bmp.FillEllipseCentered((int)current.X, (int)current.Y, (int)brushSize.Value, (int)brushSize.Value, Color.FromArgb(0, 0, 0, 0));
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

private void KeyPressed(object sender, KeyRoutedEventArgs e) { 

if (IsShiftKeyPressed() && IsCtrlKeyPressed()) { 
    switch(e.Key) {
    case VirtualKey.S: FileSaveAs_Click(null, null); break;
    case VirtualKey.Z: FileRedo_Click(null, null); break;

    }
} 
if (IsCtrlKeyPressed()){

    switch(e.Key) {
    case VirtualKey.S: FileSave_Click(null, null); break;
    case VirtualKey.Z: FileUndo_Click(null, null); break;
    case VirtualKey.Y: FileRedo_Click(null, null); break;
    case VirtualKey.L: break;

    }
}
    switch(e.Key) {
    case VirtualKey.Escape: if (debug) { FileExit_Click(null, null); } break;

    public sealed partial class MainPage : Page
    {
        CanvasMaster canvas = new CanvasMaster(800, 1000);
        DrawPoint drawPoint = new DrawPoint();

        Stroke defaultStroke = new Stroke() { StrokeColor = Colors.OrangeRed, StrokeRadius = 15 };

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PointerMoved += MainPage_PointerMoved;
        }

        private void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            drawPoint.CurrentPoint = Window.Current.CoreWindow.PointerPosition;
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

public static bool IsCtrlKeyPressed() {
    var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
    return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
}
public static bool IsShiftKeyPressed() {
    var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
    return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
}

private void MainPage_PointerReleased(object sender, PointerRoutedEventArgs e) {
	byte[] b1 = bmp.PixelBuffer.ToArray();
	byte[] b = new byte[b1.Length];
	b1.CopyTo(b, 0);
    Models.History.EndAction(new Models.Action(b));
}

private void FileUndo_Click(object sender, RoutedEventArgs e) {
    byte[] b = Models.History.Undo().bmp;
	bmp.PixelBuffer.AsStream().Write(b, 0, b.Length);
	bmp.Invalidate();
}

private void FileRedo_Click(object sender, RoutedEventArgs e) {
    byte[] b = Models.History.Redo().bmp;
	bmp.PixelBuffer.AsStream().Write(b, 0, b.Length);
	bmp.Invalidate();
}

private void FileLoad_Click(object sender, RoutedEventArgs e) {

}

private void FileSaveAs_Click(object sender, RoutedEventArgs e) {
    fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
    fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
    fileSavePicker.SuggestedFileName = "image";

    var outputFile = fileSavePicker.PickSaveFileAsync();

    if (outputFile == null) { // The user cancelled the picking operation
    return;
    }
    newFile = false;
}

private void FileSave_Click(object sender, RoutedEventArgs e) {
    if (newFile) { 
    FileSaveAs_Click(sender, e);
    }else { 

    var outputFile = fileSavePicker.PickSaveFileAsync();

    if (outputFile == null) { // The user cancelled the picking operation
    return;
    }
    }
}

private void FileExit_Click(object sender, RoutedEventArgs e) {
    Application.Current.Exit();
}

}

