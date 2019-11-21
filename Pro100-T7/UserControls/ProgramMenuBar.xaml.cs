using Pro100_T7.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pro100_T7.UserControls {

public sealed partial class ProgramMenuBar : UserControl {
    int brushType = 0;
    bool debug = true;
    bool newFile = true;
    FileSavePicker fileSavePicker = new FileSavePicker();
    private CanvasMaster drawArea;
    private CanvasMaster canvas;

public CanvasMaster DrawArea {
    get { return drawArea; }
    set { drawArea = value; }
}

public int BrushType { 
    get { return brushType; }
    set { brushType = value; }
}

public CanvasMaster Canvas {
    get { return canvas; }
    set { canvas = value; }
}

public ProgramMenuBar() {
    this.InitializeComponent();
}

public void OnNavigatedTo(NavigationEventArgs e) { 
    KeyDown += KeyPressed;        
}

private void KeyPressed(object sender, KeyRoutedEventArgs e) {
    if (IsShiftKeyPressed() && IsCtrlKeyPressed()) {
    switch (e.Key) {
    case VirtualKey.S: FileSaveAs_Click(null, null); break;
    case VirtualKey.Z: FileRedo_Click(null, null); break;
    }
    }
    if (IsCtrlKeyPressed()) {
    switch (e.Key) {
    case VirtualKey.S: FileSave_Click(null, null); break;
    case VirtualKey.Z: FileUndo_Click(null, null); break;
    case VirtualKey.Y: FileRedo_Click(null, null); break;
    case VirtualKey.L: break;
    }
    }
    switch (e.Key) {
    case VirtualKey.Escape: if (debug) { FileExit_Click(null, null); } break;
    }
}

public static bool IsCtrlKeyPressed() {
    var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down; }
public static bool IsShiftKeyPressed() {
    var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;}

private async void FileNew_Click(object sender, RoutedEventArgs e) {
    ContentDialog newFile = new ContentDialog {
    Title = "Do you want to save before opening a new canvas?",
    CloseButtonText = "Cancel",
    PrimaryButtonText = "Yes",
    SecondaryButtonText = "No"};

    var result = await newFile.ShowAsync();

    if (result == ContentDialogResult.Primary) {
    //FileSave_Click(null, null);
    //canvas.ImageDataLayer.BitmapDrawingData.Clear(); //Clearing the canvas
    History.ClearHistory(); }
    else if (result == ContentDialogResult.Secondary) {
    //canvas.ImageDataLayer.BitmapDrawingData.Clear(); //Clearing the canvas
    History.ClearHistory();
    }
}

private void FileSave_Click(object sender, RoutedEventArgs e) {
    if (newFile) { FileSaveAs_Click(sender, e); }
    else { 
    var outputFile = fileSavePicker.PickSaveFileAsync();
    if (outputFile == null) { // The user cancelled the picking operation
    return;
    }
    }
}

private async void FileSaveAs_Click(object sender, RoutedEventArgs e) {
    fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
    fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
    fileSavePicker.FileTypeChoices.Add("PNG files", new List<string>() { ".png" });
    fileSavePicker.SuggestedFileName = "image";
    var outputFile = await fileSavePicker.PickSaveFileAsync();
    if (outputFile == null) { // The user cancelled the picking operation
    return;
    }

    SoftwareBitmap outputBitmap = SoftwareBitmap.CreateCopyFromBuffer(
    drawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer,
    BitmapPixelFormat.Bgra8,
    drawArea.ImageDataLayer.BitmapDrawingData.PixelWidth,
    drawArea.ImageDataLayer.BitmapDrawingData.PixelHeight
    );

    SaveSoftwareBitmapToFile(outputBitmap, outputFile);
    newFile = false;
}

private async void SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile) {
    using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite)) {
    // Create an encoder with the desired format
    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
    if (outputFile.FileType == ".png") encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
    if (outputFile.FileType == ".jpg") encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
    // Set the software bitmap
    encoder.SetSoftwareBitmap(softwareBitmap);
    // Set additional encoding parameters, if needed 
    encoder.BitmapTransform.ScaledWidth = 1000;//(uint)DrawCanvas.GetControlCanvasUIElement().Width;
    encoder.BitmapTransform.ScaledHeight = 800;//(uint)DrawCanvas.GetControlCanvasUIElement().Height;
    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
    encoder.IsThumbnailGenerated = true;
    try { await encoder.FlushAsync(); } 
    catch (Exception err) {
    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
    switch (err.HResult) {
    case WINCODEC_ERR_UNSUPPORTEDOPERATION:
    // If the encoder does not support writing a thumbnail, then try again
    // but disable thumbnail generation.
    encoder.IsThumbnailGenerated = false;
    break;
    default: throw;
    }
    }
    if (encoder.IsThumbnailGenerated == false)
    {
    await encoder.FlushAsync();
    }
    }
}

private void FileLoad_Click(object sender, RoutedEventArgs e) {

}

private async void FileExit_Click(object sender, RoutedEventArgs e) {
    ContentDialog newFile = new ContentDialog {
    Title = "Do you want to save before exiting the application?",
    CloseButtonText = "Cancel",
    PrimaryButtonText = "Yes",
    SecondaryButtonText = "No"};

    var result = await newFile.ShowAsync();

    if (result == ContentDialogResult.Primary) {
    FileSave_Click(null, null);
    Application.Current.Exit();
    } else if (result == ContentDialogResult.Secondary) {
    Application.Current.Exit();
    }
}

private void FileUndo_Click(object sender, RoutedEventArgs e) {
    byte[] b = History.Undo().bmp;
    canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    canvas.ImageDataLayer.BitmapDrawingData.Invalidate();
}

private void FileRedo_Click(object sender, RoutedEventArgs e) {
    byte[] b = History.Redo().bmp;
    canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    canvas.ImageDataLayer.BitmapDrawingData.Invalidate();
}
private void RegularBrush_Click(object sender, RoutedEventArgs e) {
    BrushType = 0;
}
private void WavyBrush_Click(object sender, RoutedEventArgs e) {
    BrushType = 1;
}
private void DoubleBrush_Click(object sender, RoutedEventArgs e) {
    BrushType = 2;
}
private void PenBrush_Click(object sender, RoutedEventArgs e) {
    BrushType = 3;
}
private void ClearCanvas_Click(object sender, RoutedEventArgs e){
    BrushType = 4;
}
private void TriangleBrush_Click(object sender, RoutedEventArgs e){
    BrushType = 5;
}
private void HourglassBrush_Click(object sender, RoutedEventArgs e){
    BrushType = 6;
}

        private void FileExport_Click(object sender, RoutedEventArgs e) {

}

}

}
