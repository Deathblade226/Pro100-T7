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
using Windows.Storage.FileProperties;
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
    readonly bool debug = true;
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
    private string hostip = "0.0.0.0";
    private string debugip = "127.0.0.1";

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
//public void SetFocus() { this.Focus(FocusState.Programmatic); }

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
    encoder.BitmapTransform.ScaledWidth = (uint)drawArea.ImageData.Width;//(uint)DrawCanvas.GetControlCanvasUIElement().Width;
    encoder.BitmapTransform.ScaledHeight = (uint)drawArea.ImageData.Height;//(uint)DrawCanvas.GetControlCanvasUIElement().Height;
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
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
    BrushType = 1;
}
/// <summary>
/// Sets the current brush to double.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void DoubleBrush_Click(object sender, RoutedEventArgs e) {
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
    BrushType = 2;
}
/// <summary>
/// Sets the current brush to pen.
/// </summary>
/// <param name="sender">Set to null</param>
/// <param name="e">Set to null</param>
private void PenBrush_Click(object sender, RoutedEventArgs e) {
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
    BrushType = 3;
}
private void ClearCanvas_Click(object sender, RoutedEventArgs e){
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
    BrushType = 4;
}
private void TriangleBrush_Click(object sender, RoutedEventArgs e){
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
    BrushType = 5;
}
private void HourglassBrush_Click(object sender, RoutedEventArgs e){
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
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
    DrawingCanvas.rebuildHistory();
}

private async void Host_Click(object sender, RoutedEventArgs e)
{
    var box = new OnlineConnect();
    var input = await box.ShowAsync();

    if (input == ContentDialogResult.Primary) {
    //hostip = box.IP;
    //buildSession(new Client(), new Server());
    }
}

private async void Connect_Click(object sender, RoutedEventArgs e)
{
    var box = new OnlineConnect();
    var input = await box.ShowAsync();

    if (input == ContentDialogResult.Primary) {
    //hostip = box.IP;
    //buildSession(new Client());
    }
}

private void buildSession(Client client, Server server = null) { 
    Session.Initialize(true);
    Session.Build(client, server);
    AttemptConnect();
}

private void AttemptConnect()
{
    if (debug && debugip != "") hostip = debugip;

    bool success = false;
    uint trycount = 0;
    IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(hostip), 5555);
    do { success = Session.CurrentClientSession.TryConnectToServer(ipep); Debug.WriteLine($"Connected: {success}"); }
    while ( !success || trycount < 1000);
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

private void FileSaveCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    if (outputFile != null) isNewFile = Exists();
    Save();
}

private async void FileNewCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    ContentDialog newFile = new ContentDialog {
    Title = "Do you want to save before opening a new canvas?",
    CloseButtonText = "Cancel",
    PrimaryButtonText = "Yes",
    SecondaryButtonText = "No"};

    var result = await newFile.ShowAsync();
    filenew(result);
}

private void filenew(ContentDialogResult result) { 
    if (result == ContentDialogResult.Primary) {
    openNew = true;
    newSize = true;
    FileSaveCommand_ExecuteRequested(null, null);
    NewWindowSize();
    }
    else if (result == ContentDialogResult.Secondary) { //No problem
    isNewFile = true;
    outputFile = null;
    NewWindowSize();
    }
}

private async void Save() { 
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
    fileOpenPicker.FileTypeFilter.Add(customFileExtension);
    StorageFile inputFile = await fileOpenPicker.PickSingleFileAsync();
    //User cancelled load
    if (inputFile == null) { return; }
    ImageProperties imageProperties = await inputFile.Properties.GetImagePropertiesAsync();

			if (inputFile.FileType == customFileExtension)
			{
				using (Stream stream = await inputFile.OpenStreamForReadAsync())
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(Exportable));
					object obj = ser.ReadObject(stream);
					Exportable exp = (Exportable)obj;

					SetDrawingArea((int)exp.width, (int)exp.height);
					DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(exp.bytes, 0, exp.bytes.Length);
					DrawArea.ImageDataLayer.BitmapDrawingData.Invalidate();
					History.StartHistory(exp.bytes);
                    DrawingCanvas.canvas.ImageDataLayer.DrawBrush(new Stroke(), new DrawPoint(new Point(0, 0), new Point(0, 0)));
                    History.Undo();
                }
				return;
			}


    using (IRandomAccessStream fileStream = await inputFile.OpenAsync(Windows.Storage.FileAccessMode.Read)) {
    /* BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, fileStream);
    //if (inputFile.FileType == ".png") decoder = await BitmapDecoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
    //if (inputFile.FileType == ".jpg") decoder = await BitmapDecoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
    SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();
    byte[] pixels = bitmap.BitmapPixelFormat
    drawArea.ImageDataLayer.BitmapDrawingData.SetPixel*/
    newHeight = (int)imageProperties.Height;
    newWidth = (int)imageProperties.Width;
    SetDrawingArea(newWidth, newHeight);

    //WriteableBitmap bi = new WriteableBitmap(newWidth, newHeight);
    //await bi.SetSourceAsync(fileStream);
    //byte[] pixels = bi.ToByteArray();
    //DrawArea.ImageDataLayer.BitmapDrawingData.FromByteArray(pixels);
    //^V does the same
    await drawArea.ImageDataLayer.BitmapDrawingData.SetSourceAsync(fileStream); //Look at this
    updateLoad();
    }
}
		[DataContract]
		public struct Exportable
		{
			[DataMember]
			public byte[] bytes;
			[DataMember]
			public double width;
			[DataMember]
			public double height;

			public Exportable(byte[] bytes, double width, double height)
			{
				this.bytes = bytes;
				this.width = width;
				this.height = height;
			}
		}

private void updateLoad() { 
    History.StartHistory(DrawingCanvas.canvas.ImageDataLayer.BitmapDrawingData.ToByteArray());
    DrawingCanvas.canvas.ImageDataLayer.DrawBrush(new Stroke(), new DrawPoint(new Point(0, 0), new Point(0, 0)));
    History.Undo();
}

private async void FileExportCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
	//serialization here
	FileSavePicker picker = new FileSavePicker();
	picker.FileTypeChoices.Add("Drawing Project file", new List<string>() { customFileExtension });
	picker.SuggestedFileName = "New Project";
	StorageFile file = await picker.PickSaveFileAsync();
	if (file != null) {
	using (Stream stream = await file.OpenStreamForWriteAsync()) {
    DataContractSerializer ser = new DataContractSerializer(typeof(Exportable));
					Exportable export = new Exportable(DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.ToArray(), DrawArea.ImageData.ActualWidth, DrawArea.ImageData.ActualHeight);
					ser.WriteObject(stream, export);
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
    exitcode(result);
}

private void exitcode(ContentDialogResult result) { 
    if (result == ContentDialogResult.Primary) {
    exit = true;
    DrawingCanvas.StopTimer();
    FileSaveCommand_ExecuteRequested(null, null);
    
    } else if (result == ContentDialogResult.Secondary) {
    DrawingCanvas.StopTimer();
    Application.Current.Exit();
    }
}

private void EditUndoCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    byte[] b = History.Undo().bmp;
	SelectionTool.UndoSelection();
    DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    DrawArea.ImageDataLayer.BitmapDrawingData.Invalidate();
}

private void EditRedoCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    byte[] b = History.Redo().bmp;
	SelectionTool.RedoSelection();
    DrawArea.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
    DrawArea.ImageDataLayer.BitmapDrawingData.Invalidate();
}

private void RegilarBrushCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
    BrushType = 0;
}

private void ToolsEyeDropperCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.UpArrow, 0);
    BrushType = 8;
}

private void ToolsFillCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Pin, 0);
	BrushType = 9;
}
private void ToolsEraseCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.UniversalNo, 0);
    BrushType = 7;
}
private void ToolsSelectCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeAll, 0);
	BrushType = 10;
}
private void EditDeleteCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    SelectionTool.ClearSelection();
}
private void ToolsLine_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    BrushType = 11;
}

}

}

