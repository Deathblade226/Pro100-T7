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
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using System;

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

        bool debug = true;
        FileSavePicker fileSavePicker = new FileSavePicker();
        bool newFile = true;
        string brushType = "regular";

        Stroke defaultStroke = new Stroke() { StrokeColor = Colors.OrangeRed, StrokeRadius = 15 };

        public MainPage()
        {
            this.InitializeComponent();

            DrawCanvas.GetControlCanvasUIElement().Children.Add(canvas.ImageData);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            KeyDown += KeyPressed;
            PointerMoved += MainPage_PointerMoved;
            DrawCanvas.GetControlCanvasUIElement().PointerReleased += MainPage_PointerReleased;
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

        //private void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e)
        //{
        //    #region Notes
        //    //bmp.SetPixel((int)current.X, (int)current.Y, Colors.Black);

        //    //Rects with outline, also gaps
        //    //bmp.DrawLineAa((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color, (int)brushSize.Value);    


        //    //Pen Like drawing - This could be a brush by its self.
        //    //bmp.DrawLineDDA((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color);

        //    //Filled Circles

        //    //Cant take in constant input
        //    //int[] points = { (int)old.X, (int)old.Y, (int)current.X, (int)current.Y};
        //    //bmp.FillCurveClosed(points, 0.5f,  colorPicker.Color);

        //    //int w = bmp.PixelWidth;
        //    //int h = bmp.PixelHeight;
        //    //WriteableBitmapExtensions.DrawLine(bmp.GetBitmapContext() , w, h, 1, 2, 30, 40, 255);

        //    //Draws lines spaced out
        //    //bmp.DrawLineBresenham((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color);

        //    //bmp.Invalidate();

        //    //Clears the canvas
        //    //bmp.Clear();

        //    //Used to pickup color
        //    //colorPicker.Color = bmp.GetPixel((int)current.X, (int)current.Y);
        //    #endregion

        //    #region Drawing1.0
        //    var pointerPosition = Window.Current.CoreWindow.PointerPosition;
        //    var x = pointerPosition.X - Window.Current.Bounds.X - 60;
        //    var y = pointerPosition.Y - Window.Current.Bounds.Y - 110;
        //    Pointer ptr = e.Pointer;
        //    Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(null);
        //    if (ptr.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
        //    {

        //        old = current;
        //        current.Y = y;
        //        current.X = x;

        //        Mouse.Text = "x:" + x + " | y:" + y;
        //        if (ptrPt.Properties.IsLeftButtonPressed)
        //        {

        //            drawing.Source = bmp;
        //            using (bmp.GetBitmapContext())
        //            {
        //                Brush brush = new Models.Brush((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, colorPicker.Color, (int)brushSize.Value, bmp);

        //                if (brushType.Equals("regular"))
        //                {
        //                    brush.Regular();
        //                }
        //                else if (brushType.Equals("wavy"))
        //                {
        //                    brush.Wavy();
        //                }
        //                else if (brushType.Equals("pen"))
        //                {
        //                    brush.Pen();
        //                }
        //                else if (brushType.Equals("double"))
        //                {
        //                    brush.Double();
        //                }
        //                bmp.Invalidate();
        //            }
        //        }
        //    }

        //    if (ptrPt.Properties.IsRightButtonPressed)
        //    {

        //        bmp.FillEllipseCentered((int)current.X, (int)current.Y, (int)brushSize.Value, (int)brushSize.Value, Color.FromArgb(0, 0, 0, 0));
        //        bmp.Invalidate();

        //    }

        //    #endregion

        //    #region Drawing2.0

        //    //if (e.GetCurrentPoint(DrawArea).Properties.IsLeftButtonPressed) {
        //    //drawPoints.Add(e.GetCurrentPoint(DrawArea).Position);

        //    //if (drawPoints.Count() > 10) {
        //    //bmp.DrawCurve(PointsToInts(drawPoints), 0, Colors.Black);

        //    //drawPoints.RemoveAt(0);
        //    //}
        //    //} else {
        //    //drawPoints.Clear();
        //    //}
        //    //}
        //    #endregion
        //}

        private void KeyPressed(object sender, KeyRoutedEventArgs e)
        {

            if (IsShiftKeyPressed() && IsCtrlKeyPressed())
            {
                switch (e.Key)
                {
                    case VirtualKey.S: FileSaveAs_Click(null, null); break;
                    case VirtualKey.Z: FileRedo_Click(null, null); break;

                }
            }
            if (IsCtrlKeyPressed())
            {

                switch (e.Key)
                {
                    case VirtualKey.S: FileSave_Click(null, null); break;
                    case VirtualKey.Z: FileUndo_Click(null, null); break;
                    case VirtualKey.Y: FileRedo_Click(null, null); break;
                    case VirtualKey.L: break;

                }
            }
            switch (e.Key)
            {
                case VirtualKey.Escape: if (debug) { FileExit_Click(null, null); } break;

            }



        }

        public static bool IsCtrlKeyPressed()
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
            return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }
        public static bool IsShiftKeyPressed()
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
            return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }

        private void MainPage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            byte[] b1 = canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.ToArray();
            byte[] b = new byte[b1.Length];
            b1.CopyTo(b, 0);
            History.EndAction(new Models.Action(b));
        }

        private void FileUndo_Click(object sender, RoutedEventArgs e)
        {
            byte[] b = History.Undo().bmp;
            canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
            canvas.ImageDataLayer.BitmapDrawingData.Invalidate();
        }

        private void FileRedo_Click(object sender, RoutedEventArgs e)
        {
            byte[] b = History.Redo().bmp;
            canvas.ImageDataLayer.BitmapDrawingData.PixelBuffer.AsStream().Write(b, 0, b.Length);
            canvas.ImageDataLayer.BitmapDrawingData.Invalidate();
        }

        private void FileLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
            fileSavePicker.SuggestedFileName = "image";

            var outputFile = fileSavePicker.PickSaveFileAsync();

            if (outputFile == null)
            { // The user cancelled the picking operation
                return;
            }
            newFile = false;
        }

        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            if (newFile)
            {
                FileSaveAs_Click(sender, e);
            }
            else
            {

                var outputFile = fileSavePicker.PickSaveFileAsync();

                if (outputFile == null)
                { // The user cancelled the picking operation
                    return;
                }
            }
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void RegularBrush_Click(object sender, RoutedEventArgs e)
        {
            brushType = "regular";
        }
        private void WavyBrush_Click(object sender, RoutedEventArgs e)
        {
            brushType = "wavy";
        }
        private void DoubleBrush_Click(object sender, RoutedEventArgs e)
        {
            brushType = "double";
        }
        private void PenBrush_Click(object sender, RoutedEventArgs e)
        {
            brushType = "pen";
        }

        private async void SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile)
        {
            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                if (outputFile.FileType == ".png") encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                if (outputFile.FileType == ".jpg") encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);

                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);

                // Set additional encoding parameters, if needed
                encoder.BitmapTransform.ScaledWidth = (uint)DrawCanvas.GetControlCanvasUIElement().Width;
                encoder.BitmapTransform.ScaledHeight = (uint)DrawCanvas.GetControlCanvasUIElement().Height;
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.IsThumbnailGenerated = true;

                try { await encoder.FlushAsync(); }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
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

        private async void FileNew_Click(object sender, RoutedEventArgs e)
        {

            ContentDialog newFile = new ContentDialog
            {
                Title = "Do you want to save before opening a new canvas.",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            var result = await newFile.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                FileSave_Click(null, null);
                canvas.ImageDataLayer.BitmapDrawingData.Clear();
                History.ClearHistory();
            }
            else if (result == ContentDialogResult.Secondary)
            {
                canvas.ImageDataLayer.BitmapDrawingData.Clear();
                History.ClearHistory();
            }

        }

    }
}