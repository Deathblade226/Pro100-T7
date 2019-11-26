using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
Color secondary = Color.FromArgb(255, 255, 0, 0);

public Color Secondary {
    set { secondary = value; }
}

public BrushModifierPanel() {
    this.InitializeComponent();
    Default_Click(null, null);
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
    colorPicker.Color = Color.FromArgb(255, 0, 0, 255);
    RColor.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
    LColor.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
    secondary = Color.FromArgb(255, 255, 0, 0);
}

private void TradeColor_Click(object sender, RoutedEventArgs e) {
    Color temp = secondary;
    secondary = colorPicker.Color;
    colorPicker.Color = temp;
    RColor.Fill = new SolidColorBrush(secondary);
    LColor.Fill = new SolidColorBrush(colorPicker.Color);

}

}

}
