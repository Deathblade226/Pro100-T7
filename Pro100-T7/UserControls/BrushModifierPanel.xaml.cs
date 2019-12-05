using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pro100_T7.UserControls {
public sealed partial class BrushModifierPanel : UserControl {
private Color secondary = Color.FromArgb(255, 255, 0, 0);

public Color Secondary {
    set { secondary = value; }
}

public BrushModifierPanel() {
    this.InitializeComponent();
    colorPicker.Color = Color.FromArgb(255,0,0,255);
    RColor.Fill = new SolidColorBrush(Color.FromArgb(secondary.A, secondary.R, secondary.G, secondary.B));
	if(ApplicationData.Current.LocalSettings.Values != null) {
	string brushSize = (string)ApplicationData.Current.LocalSettings.Values["brushSize"];
	if(brushSize != null) {
	brushSizeBox.Text = brushSize;
	}
	Application.Current.Suspending += Current_Suspending;
	if (ApplicationData.Current.LocalSettings.Values.ContainsKey("brushColor")) {
	int? intColor = (int)ApplicationData.Current.LocalSettings.Values["brushColor"];
	colorPicker.Color = Color.FromArgb((byte)((intColor & 0xff000000) >> 24), (byte)((intColor & 0x00ff0000) >> 16), (byte)((intColor & 0x0000ff00) >> 8), (byte)((intColor & 0x000000ff) >> 0));
	}			
	}
			
}

private void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e) {
    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
	StorageFolder localFolder = ApplicationData.Current.LocalFolder;

    localSettings.Values["brushSize"] = brushSizeBox.Text;
	Color color = colorPicker.Color;
	int intColor = ((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
	localSettings.Values["brushColor"] = intColor;
}

public ColorPicker GetColorPickerUIElement() => colorPicker;
public Color GetColorPickerSecondary() => secondary;
public TextBox GetBrushSizeTextBoxUIElement() => brushSizeBox;
public Slider GetBrushSizeSliderUIElement() => brushSizeSlider;

private void colorPicker_RightClick(object sender, RightTappedRoutedEventArgs e){
    RColor.Fill = new SolidColorBrush(GetColorPickerSecondary());
    secondary = colorPicker.Color;
}

private void RColorB_Click(object sender, RoutedEventArgs e) {
    RColor.Fill = new SolidColorBrush(GetColorPickerUIElement().Color);
    secondary = colorPicker.Color;
}

private void Default_Click(object sender, RoutedEventArgs e) {
    colorPicker.Color = Color.FromArgb(255, 255, 0, 0);
    RColor.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
    LColor.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
    secondary = Color.FromArgb(255, 0, 0, 255);
}

private void ColorSwap_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    Color temp = secondary;
    secondary = colorPicker.Color;
    colorPicker.Color = temp;
    RColor.Fill = new SolidColorBrush(secondary);
    LColor.Fill = new SolidColorBrush(colorPicker.Color);
}
private void IncreaseBrushSize_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    if (brushSizeSlider.Maximum != brushSizeSlider.Value) brushSizeSlider.Value++;
}
private void DecreaseBrushSize_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) {
    if (brushSizeSlider.Minimum != brushSizeSlider.Value) brushSizeSlider.Value--;
}

}

}
