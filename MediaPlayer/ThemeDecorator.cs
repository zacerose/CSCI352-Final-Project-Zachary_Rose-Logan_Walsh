using MediaPlayer;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace MediaPlayer
{
    // decorator for Windows.Controls.Button, adds extended state and behavior for changing the image based on the button "type"
    public class ButtonDecorator : Button
    {
        public Type type { get; set; }
        public enum Type
        {
            FREV, FWD, Pause, Play, Replay, Reverse, Template
        }

        public void SetButtonImage(Uri imageUri)
        {
            ImageBrush buttonTheme = new ImageBrush();
            buttonTheme.ImageSource = new BitmapImage(imageUri);
            this.Background = buttonTheme;
        }
    }
}
