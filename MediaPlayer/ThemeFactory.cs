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
    public interface ThemeFactory
    {
        Light GetLight();
        Dark GetDark();
    }

    public class StandardFactory : ThemeFactory
    {
        public Light GetLight()
        {
            Light GrayTheme = new Gray();
            return GrayTheme;
        }
        public Dark GetDark()
        {
            Dark NightTheme = new Night();
            return NightTheme;
        }
    }

    public class AdditionalFactory : ThemeFactory
    {
        public Light GetLight()
        {
            Light OrangeTheme = new Orange();
            return OrangeTheme;
        }
        public Dark GetDark()
        {
            Dark EdgyTheme = new Edgy();
            return EdgyTheme;
        }
    }


    public interface Light
    {
        ImageBrush ChangeImage();
    }
    public interface Dark
    {
        ImageBrush ChangeImage();
    }

    public class Gray : Light
    {
        public ImageBrush ChangeImage()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//TempBGColors/TempGray.jpg", UriKind.Relative));
            return ThemeColor;
        }
    }
    public class Night : Dark
    {
        public ImageBrush ChangeImage()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//TempBGColors/TempNight.jpg", UriKind.Relative));
            return ThemeColor;
        }
    }

    public class Orange : Light
    {
        public ImageBrush ChangeImage()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//TempBGColors/TempOrange.jpg", UriKind.Relative));
            return ThemeColor;
        }
    }
    public class Edgy : Dark
    {
        public ImageBrush ChangeImage()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//TempBGColors/TempEdgy.jpg", UriKind.Relative));
            return ThemeColor;
        }
    }
}
