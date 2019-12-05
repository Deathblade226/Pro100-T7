using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pro100_T7.UserControls {
public sealed partial class OnlineConnect : ContentDialog {

private string ip = "0.0.0.0";

public string IP {
    get { return ip; }
    set { ip = value; }
}

public OnlineConnect() {
    this.InitializeComponent();
}
private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
    string test = ipval.Text;
    int validSpots = 0;
    for (int i = 0; i < test.Length; i++) { 
    if (test[i] == '.') { 
    if (i != 0 || i != test.Length - 1) {
    if (IsNumber(test[i - 1]) && IsNumber(test[i + 1])) { 
    validSpots++;
    }
    }            
    }
    }
    if (validSpots == 3) {IP = test;}
    else { await(new MessageDialog("You Entered an Invalid IP.")).ShowAsync(); }
}

private bool IsNumber(char input) { 
    for (int i = 0; i < 10; i++) { 
    string temp = i.ToString();
    if (input == temp[0]) return true;
    }
return false;}

}

}
