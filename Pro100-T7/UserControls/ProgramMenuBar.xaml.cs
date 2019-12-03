using Pro100_T7.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pro100_T7.UserControls {

public sealed partial class ProgramMenuBar : UserControl {
    int brushType = 0;
    bool debug = true;
    bool isNewFile = true;
    FileSavePicker fileSavePicker = new FileSavePicker();
    StorageFile outputFile;
    private CanvasMaster drawArea;
    private int brushSize = 1;
    bool exit = false;
    bool openNew = false;
    bool newSize = false;
    int newWidth = 1000;
    int newHeight = 800;

    private string customFileExtension = ".dpf";

public CanvasMaster DrawArea {
    get { return drawArea; }
    set { drawArea = value; }
}

public int BrushType { 
    get { return brushType; }
    set { brushType = value; }
}

public int BrushSize {
    get { return brushSize; }
    set { BrushSize = value; }
}

public Canvas DrawCanvas { get; set; }

public ProgramMenuBar() {
    this.InitializeComponent();
}
/// <summary>
/// Sets the keydown event to keypressed.
/// </summary>
/// <param name="e">e</param>
public void OnNavigatedTo(NavigationEventArgs e) { 
    KeyDown += KeyPressed;        
}
/// <summary>
/// Sets the focus to the menu bar for keybinds to work.
/// </summary>
public void SetFocus() { this.Focus(FocusState.Programmatic); }

public void IncreaseBrushSize() { 
    if (BrushSize <= 200) { BrushSize++; }
}
public void ReduceBrushSize() { 
    if (BrushSize >= 1) { BrushSize--; }
}

/// <summary>
/// Reads current keys pressed and calles method.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">KeyRoutedEventArgs</param>
private void KeyPressed(object sender, KeyRoutedEventArgs e) {
    if (IsCtrlKeyPressed()) {
    switch (e.Key) {
    case VirtualKey.S: FileSave_Click(null, null); break;
    case VirtualKey.Z: FileUndo_Click(null, null); break;
    case VirtualKey.Y: FileRedo_Click(null, null); break;
    case VirtualKey.Add: IncreaseBrushSize(); break;
    case VirtualKey.Subtract: ReduceBrushSize(); break;
    case VirtualKey.L: break;
    }
    }
    switch (e.Key) {
    case VirtualKey.Escape: if (debug) { FileExit_Click(null, null); } break;
    case VirtualKey.B: RegularBrush_Click(null, null); break;
    case VirtualKey.E: Eraser_Click(null, null); break;
    case VirtualKey.I: eyeDropper_Click(null, null); break;
    }
	if(e.Key == VirtualKey.Delete || e.Key == VirtualKey.Back)
			{
				SelectionTool.ClearSelection();
			}
}

/// <summary>
/// Checks if the Ctrl key is pressed.
/// </summary>
/// <returns></returns>
public static bool IsCtrlKeyPressed() {
    var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down; }
/// <summary>
/// Checks if the shift key is pressed.
/// </summary>
/// <returns></returns>
public static bool IsShiftKeyPressed() {
    var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;}
/// <summary>
/// Creates a new file and cleares history.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private async void FileNew_Click(object sender, RoutedEventArgs e) {
    ContentDialog newFile = new ContentDialog {
    Title = "Do you want to save before opening a new canvas?",
    CloseButtonText = "Cancel",
    PrimaryButtonText = "Yes",
    SecondaryButtonText = "No"};

    var result = await newFile.ShowAsync();

    if (result == ContentDialogResult.Primary) {
    openNew = true;
    newSize = true;
    SavingImage();
    NewWindowSize();
    }
    else if (result == ContentDialogResult.Secondary) { //No problem
    isNewFile = true;
    outputFile = null;
    History.ClearHistory();
    NewWindowSize();
    }
}

private async void NewWindowSize() {
    var box = new CanvasSizeBox();
    var input = await box.ShowAsync();
    newHeight = box.HeightVal;
    newWidth = box.WidthVal;
    SetDrawingArea(newWidth, newHeight);
}

/// <summary>
/// Saves file to existing file.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private async void FileSave_Click(object sender, RoutedEventArgs e) {
    if (isNewFile) { FileSaveAs_Click(sender, e); }
    else { 
    if (outputFile == null) { // The user cancelled the picking operation
    return;
    }
    SoftwareBitmap outputBitmap = SoftwareBitmap.CreateCopyFromBuffer(
    drawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer,
    BitmapPixelFormat.Bgra8,
    drawArea.ImageDataLayer.BitmapDrawingData.PixelWidth,
    drawArea.ImageDataLayer.BitmapDrawingData.PixelHeight
    );

    await SaveSoftwareBitmapToFile(outputBitmap, outputFile);
    if (exit) { Application.Current.Exit(); }
    if (openNew) {     
    FileUndo_Click(null, null);
    isNewFile = true;
    outputFile = null;
    openNew = false; }
    }
}
/// <summary>
/// Creates a new file to save to.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private async void FileSaveAs_Click(object sender, RoutedEventArgs e) {
    fileSavePicker = new FileSavePicker();
    fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
    
    fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
    fileSavePicker.FileTypeChoices.Add("PNG files", new List<string>() { ".png" });
    fileSavePicker.SuggestedFileName = "image";
    outputFile = await fileSavePicker.PickSaveFileAsync();
    if (outputFile == null) { // The user cancelled the picking operation
    return;
    }

    SoftwareBitmap outputBitmap = SoftwareBitmap.CreateCopyFromBuffer(
    drawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer,
    BitmapPixelFormat.Bgra8,
    drawArea.ImageDataLayer.BitmapDrawingData.PixelWidth,
    drawArea.ImageDataLayer.BitmapDrawingData.PixelHeight
    );

    await SaveSoftwareBitmapToFile(outputBitmap, outputFile);
    if (exit) { Application.Current.Exit(); }
    if (openNew) {     
    FileUndo_Click(null, null);
    isNewFile = true;
    outputFile = null;
    openNew = false; }
    isNewFile = false;
}

private async Task<bool> SavingImage(){
    FileSave_Click(null, null);
return true;}

/// <summary>
/// Saves images to file.
/// </summary>
/// <param name="softwareBitmap">Set to null</param>
/// <param name="outputFile">Set to null</param>
private async Task<bool> SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile) {
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
    if (encoder.IsThumbnailGenerated == false) {
                    
    await encoder.FlushAsync(); 
    }
    }
    if (newSize) SetDrawingArea(newWidth, newHeight); newSize = false;
return true;}
/// <summary>
/// Loades a file with history data.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void FileLoad_Click(object sender, RoutedEventArgs e) {

}
/// <summary>
/// Closes application and asks to save.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private async void FileExit_Click(object sender, RoutedEventArgs e) {
    ContentDialog newFile = new ContentDialog {
    Title = "Do you want to save before exiting the application?",
    CloseButtonText = "Cancel",
    PrimaryButtonText = "Yes",
    SecondaryButtonText = "No"};

    var result = await newFile.ShowAsync();

    if (result == ContentDialogResult.Primary) {
    exit = true;
    await SavingImage();

    } else if (result == ContentDialogResult.Secondary) {
    Application.Current.Exit();
    }
}
/// <summary>
/// Undoes the last action.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void FileUndo_Click(object sender, RoutedEventArgs e) {
    byte[] b = History.Undo().bmp;
    DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    DrawArea.ImageDataLayer.BitmapDrawingData.Invalidate();
	SelectionTool.SelectionUndo();
}
/// <summary>
/// Redoes the last action.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void FileRedo_Click(object sender, RoutedEventArgs e) {
    byte[] b = History.Redo().bmp;
    DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    DrawArea.ImageDataLayer.BitmapDrawingData.Invalidate();
}
/// <summary>
/// Sets current brush to base brush.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void RegularBrush_Click(object sender, RoutedEventArgs e) {
    BrushType = 0;
}
/// <summary>
/// Sets the current brush to wavy. (Bugged)
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void WavyBrush_Click(object sender, RoutedEventArgs e) {
    BrushType = 1;
}
/// <summary>
/// Sets the current brush to double.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void DoubleBrush_Click(object sender, RoutedEventArgs e) {
    BrushType = 2;
}
/// <summary>
/// Sets the current brush to pen.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
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
private void Fill_Click(object sender, RoutedEventArgs e)
{
	BrushType = 9;
}

private void Eraser_Click(object sender, RoutedEventArgs e) {
    BrushType = 7;
}
private void eyeDropper_Click(object sender, RoutedEventArgs e) {
    BrushType = 8;
}

private void Selection_Click(object sender, RoutedEventArgs e)
{
	BrushType = 10;
	//DrawArea.UICanvasObject.PointerReleased += SelectionRelease;
}

//private void SelectionRelease(object sender, PointerRoutedEventArgs e)
//{
//	SelectionTool.SelectRelease();
//}

/// <summary>
/// Exports the file to be loaded back into latter. Saves current settings.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private async void FileExport_Click(object sender, RoutedEventArgs e) {
	//serialization here
	FileSavePicker picker = new FileSavePicker();
	picker.FileTypeChoices.Add("Drawing Project file", new List<string>() { customFileExtension });
	picker.SuggestedFileName = "New Project";
	StorageFile file = await picker.PickSaveFileAsync();
	if (file != null) {
	using (Stream stream = await file.OpenStreamForWriteAsync()) {
    DataContractSerializer ser = new DataContractSerializer(typeof(byte[]));
	ser.WriteObject(stream, DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.ToArray());
	}
    }	
}

private void SetDrawingArea(int width, int height) { 
    DrawArea.ImageDataLayer.BitmapDrawingData.Clear();
    DrawArea.ImageDataLayer.BitmapDrawingData = BitmapFactory.New(width, height);
    DrawCanvas.Width = width;
    DrawCanvas.Height = height;
    History.ClearHistory();
}

        

        private void Host_Click(object sender, RoutedEventArgs e)
        {
            Session.Initialize(true, true);
            Session.Build(new Client(), new Server());

            AttemptConnect();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Session.Initialize(true);
            Session.Build(new Client());

            AttemptConnect();
        }

        private void AttemptConnect()
        {
            bool success = false;
            uint trycount = 0;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            do { success = Session.CurrentClientSession.TryConnectToServer(ipep); Debug.WriteLine($"Connected: {success}"); }
            while ( !success || trycount < 1000);
        }
    }

}
