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
using Brush = Pro100_T7.Models.Brush;

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
bool onCanvas = false;
String brushType = "regular";

public MainPage() {
    this.InitializeComponent();
    bmp = BitmapFactory.New((int)DrawArea.Width, (int)DrawArea.Height);
    Models.History.StartHistory(bmp.PixelBuffer.ToArray());
    //bmp = BitmapFactory.New(1500, 1000);
    //ApplicationView.PreferredLaunchViewSize = new Size(1750, 1250);
    //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
}

protected async override void OnNavigatedTo(NavigationEventArgs e) {
//    KeyDown += KeyPressed;
    PointerMoved += MainPage_PointerMoved;
    DrawArea.PointerReleased += MainPage_PointerReleased;

}

private async void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e) {

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
    //bmp.DrawLineAa((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color, (int)brushSize.Value * 2);    
    // 
    Brush brush = new Models.Brush((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color, (int)brushSize.Value, bmp);
    if (brushType.Equals("regular")){
    brush.Regular();
    }
    else if (brushType.Equals("wavy")){
    brush.Wavy();
    }
    else if (brushType.Equals("pen")){
    brush.Pen();
    }
    else if (brushType.Equals("double")){
    brush.Double();
    }
    bmp.Invalidate();    
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

}

private void KeyPressed(object sender, KeyRoutedEventArgs e) {
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
    fileSavePicker.FileTypeChoices.Add("PNG files", new List<string>() { ".png" });
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
private void DrawArea_PointerExited(object sender, PointerRoutedEventArgs e) {
    onCanvas = false;
}

private void DrawArea_PointerEntered(object sender, PointerRoutedEventArgs e) {

}

private void WavyBrush_Click(object sender, RoutedEventArgs e){
    brushType = "wavy";
}
private void DoubleBrush_Click(object sender, RoutedEventArgs e){
    brushType = "double";
}
private void PenBrush_Click(object sender, RoutedEventArgs e){
    brushType = "pen";
}

private void RegularBrush_Click(object sender, RoutedEventArgs e) {
    brushType = "regular";
}

}

}