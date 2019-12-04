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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pro100_T7.UserControls {
public sealed partial class CanvasSizeBox : ContentDialog {

public int WidthVal { get; set; }
public int HeightVal { get; set; }

public CanvasSizeBox() {
    this.InitializeComponent();
}

private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
    int w = 0;
    int.TryParse(wval.Text, out w);
    if (w < 100) w = 100;
    if (w > 1400) w = 1400;
    WidthVal = w;
    int h = 0;
    int.TryParse(wval.Text, out h);
    if (h < 100) h = 100;
    if (h > 1000) h = 1000;
    HeightVal = h;
}

private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
    if (WidthVal == 0) WidthVal = 1000;
    if (HeightVal == 0) HeightVal = 800;
}

}

}
