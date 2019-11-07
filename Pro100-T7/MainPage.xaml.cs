using System;
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
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pro100_T7
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        WriteableBitmap bmp = BitmapFactory.New(1500, 800);
        Point current = new Point();
        Point old = new Point();
        List<Image> images = new List<Image>();

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(1500, 1000);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PointerMoved += MainPage_PointerMoved;
        }

        private void MainPage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Pointer ptr = e.Pointer;
            if (ptr.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(null);
                current = e.GetCurrentPoint(null).Position;
                if (ptrPt.Properties.IsLeftButtonPressed)
                {
                    Mouse.Text = "x:" + e.GetCurrentPoint(null).Position.X.ToString() + " | y:" + e.GetCurrentPoint(null).Position.Y.ToString();

                    Image.Source = bmp;
                    using (bmp.GetBitmapContext())
                    {
                        //bmp.SetPixel((int)current.X, (int)current.Y, Colors.Black);
                        Rect size = new Rect();
                        bmp.DrawLineAa((int)old.X, (int)old.Y, (int)current.X, (int)current.Y, Colors.Black, 5);
                    }
                    old = e.GetCurrentPoint(null).Position;
                }
                else { old = current; }

                if (ptrPt.Properties.IsRightButtonPressed)
                {

                }
            }
        }
    }

}

