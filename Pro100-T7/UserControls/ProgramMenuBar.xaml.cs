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
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
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
    FileOpenPicker fileOpenPicker = new FileOpenPicker();
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

public ProgramMenuBar() {
    this.InitializeComponent();
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

private async void NewWindowSize() {
    var box = new CanvasSizeBox();
    var input = await box.ShowAsync();
    newHeight = box.HeightVal;
    newWidth = box.WidthVal;
    SetDrawingArea(newWidth, newHeight);
}

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
/// <summary>
/// Takes in width and hegith to build a new canvas and image
/// </summary>
/// <param name="width">Width</param>
/// <param name="height">Height</param>
private void SetDrawingArea(int width, int height) { 
    DrawArea.ImageDataLayer.BitmapDrawingData.Clear();
    DrawArea.ImageDataLayer.BitmapDrawingData = BitmapFactory.New(width, height);
    DrawingCanvas.DrawCanvas.Width = width;
    DrawingCanvas.DrawCanvas.Height = height;
    History.ClearHistory();
}

public bool Exists() {
    string filePath = "";
    if (outputFile != null) {filePath = outputFile.Path;}
    try {
    string path = Path.GetDirectoryName(filePath);
    var fileName = Path.GetFileName(filePath);
    StorageFolder accessFolder = StorageFolder.GetFolderFromPathAsync(path).AsTask().GetAwaiter().GetResult();
    StorageFile file = accessFolder.GetFileAsync(fileName).AsTask().GetAwaiter().GetResult();
    return file != null;
    } catch { 
    return false;
}
}

private async void FileSaveCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    if (isNewFile) { FileSaveAsCommand_ExecuteRequested(null, null); }
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
    EditUndoCommand_ExecuteRequested(null, null);
    isNewFile = true;
    outputFile = null;
    openNew = false; }
    }
}

private async void FileNewCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    ContentDialog newFile = new ContentDialog {
    Title = "Do you want to save before opening a new canvas?",
    CloseButtonText = "Cancel",
    PrimaryButtonText = "Yes",
    SecondaryButtonText = "No"};

    var result = await newFile.ShowAsync();

    if (result == ContentDialogResult.Primary) {
    openNew = true;
    newSize = true;
    FileSaveCommand_ExecuteRequested(null, null);
    NewWindowSize();
    }
    else if (result == ContentDialogResult.Secondary) { //No problem
    isNewFile = true;
    outputFile = null;
    History.ClearHistory();
    NewWindowSize();
    }
}

private async void FileSaveAsCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
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
    EditUndoCommand_ExecuteRequested(null, null);
    isNewFile = true;
    outputFile = null;
    openNew = false; }
    isNewFile = false;
}

private async void FileLoadCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            StorageFile inputFile = await fileOpenPicker.PickSingleFileAsync();
            //User cancelled load
            if (inputFile == null) { return; }

            using (IRandomAccessStream fileStream = await inputFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                /* BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, fileStream);
                 //if (inputFile.FileType == ".png") decoder = await BitmapDecoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                 //if (inputFile.FileType == ".jpg") decoder = await BitmapDecoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
                 SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();
                 byte[] pixels = bitmap.BitmapPixelFormat
                 drawArea.ImageDataLayer.BitmapDrawingData.SetPixel*/

                WriteableBitmap bi = new WriteableBitmap(1000, 800);
                await bi.SetSourceAsync(fileStream);
                byte[] pixels = bi.ToByteArray();
                drawArea.ImageDataLayer.BitmapDrawingData.FromByteArray(pixels);


            }
        }

private async void FileExportCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
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

private async void FileExitCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    ContentDialog newFile = new ContentDialog {
    Title = "Do you want to save before exiting the application?",
    CloseButtonText = "Cancel",
    PrimaryButtonText = "Yes",
    SecondaryButtonText = "No"};

    var result = await newFile.ShowAsync();

    if (result == ContentDialogResult.Primary) {
    exit = true;
    FileSaveCommand_ExecuteRequested(null, null);

    } else if (result == ContentDialogResult.Secondary) {
    Application.Current.Exit();
    }
}

private void EditUndoCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    byte[] b = History.Undo().bmp;
    DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    DrawArea.ImageDataLayer.BitmapDrawingData.Invalidate();
}

private void EditRedoCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    byte[] b = History.Redo().bmp;
    DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    DrawArea.ImageDataLayer.BitmapDrawingData.Invalidate();
}

private void RegilarBrushCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    BrushType = 0;
}

private void ToolsEyeDropperCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    BrushType = 8;
}

private void ToolsFillCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
	BrushType = 9;
}
private void ToolsEraseCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    BrushType = 7;
}
private void ToolsSelectCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
	BrushType = 10;
}
private void EditDeleteCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    SelectionTool.ClearSelection();
}
private void ToolsLine_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    BrushType = 11;
}

        //Dynamic IP connection code
        //private async void Host_Click(object sender, RoutedEventArgs e)
        //{
        //    var box = new OnlineConnect();
        //    var input = await box.ShowAsync();

        //    if (input == ContentDialogResult.Primary) {
        //    //hostip = box.IP;
        //    //buildSession(new Client(), new Server());
        //    }
        //}

        //private async void Connect_Click(object sender, RoutedEventArgs e)
        //{
        //    var box = new OnlineConnect();
        //    var input = await box.ShowAsync();

        //    if (input == ContentDialogResult.Primary) {
        //    //hostip = box.IP;
        //    //buildSession(new Client());
        //    }
        //}

        //private void buildSession(Client client, Server server = null) { 
        //    Session.Initialize(true);
        //    Session.Build(client, server);
        //    AttemptConnect();
        //}

        //private void AttemptConnect()
        //{
        //    if (debug && debugip != "") hostip = debugip;

        //    bool success = false;
        //    uint trycount = 0;
        //    IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(hostip), 5555);
        //    do { success = Session.CurrentClientSession.TryConnectToServer(ipep); Debug.WriteLine($"Connected: {success}"); }
        //    while ( !success || trycount < 1000);
        //}

        //Static IP connection code
        private void Host_Click(object sender, RoutedEventArgs e)
        {
            Session.Initialize(true, true);
            Session.Build(new Client(), new Server());

            AttemptConnect(true);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Session.Initialize(true);
            Session.Build(new Client());

            AttemptConnect(false);
        }

        private void AttemptConnect(bool host)
        {
            bool success = false;
            uint trycount = 0;
            IPEndPoint ipep = new IPEndPoint((host) ? IPAddress.Parse("127.0.0.1") : IPAddress.Parse("172.0.0.1"), 5555);
            do { success = Session.CurrentClientSession.TryConnectToServer(ipep); Debug.WriteLine($"{trycount} Connected: {success}"); }
            while (!success && ++trycount < 100);
        }
    }

}

