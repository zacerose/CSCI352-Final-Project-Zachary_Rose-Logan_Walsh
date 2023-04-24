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

    public interface Theme
    {
        ImageBrush ChangeBackground();
        void ChangeButtonImage(ButtonDecorator button);
        SolidColorBrush ChangeLabelColor();
    }
    public abstract class Light : Theme
    {
        public abstract ImageBrush ChangeBackground();
        public abstract void ChangeButtonImage(ButtonDecorator button);
        public SolidColorBrush ChangeLabelColor()
        {
            return Brushes.Black;
        }
    }
    public abstract class Dark : Theme
    {
        public abstract ImageBrush ChangeBackground();
        public abstract void ChangeButtonImage(ButtonDecorator button);
        public SolidColorBrush ChangeLabelColor()
        {
            return Brushes.White;
        }
    }

    public class Gray : Light
    {
        public override ImageBrush ChangeBackground()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//Assets//TempBGColors/TempGray.jpg", UriKind.Relative));
            return ThemeColor;
        }
        public override void ChangeButtonImage(ButtonDecorator button)
        {
            switch (button.type)
            {
                case ButtonDecorator.Type.FREV:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_FREV.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.FWD:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_FWD.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Pause:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_Pause.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Play:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_Play.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Replay:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_Replay.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Reverse:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_Reverse.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Fullscreen:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_Fullscreen.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Template:
                    button.SetButtonImage(new Uri("..//..//Assets//Default//Def_Template.png", UriKind.Relative));
                    break;
            }
        }
    }
    public class Night : Dark
    {
        public override ImageBrush ChangeBackground()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//Assets//TempBGColors/TempNight.jpg", UriKind.Relative));
            return ThemeColor;
        }
        public override void ChangeButtonImage(ButtonDecorator button)
        {
            switch (button.type)
            {
                case ButtonDecorator.Type.FREV:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_FREV.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.FWD:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_FWD.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Pause:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_Pause.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Play:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_Play.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Replay:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_Replay.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Reverse:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_Reverse.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Fullscreen:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_Fullscreen.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Template:
                    button.SetButtonImage(new Uri("..//..//Assets//Night//Night_Template.png", UriKind.Relative));
                    break;
            }
        }
    }

    public class Orange : Light
    {
        public override ImageBrush ChangeBackground()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//Assets//TempBGColors/TempOrange.jpg", UriKind.Relative));
            return ThemeColor;
        }
        public override void ChangeButtonImage(ButtonDecorator button)
        {
            switch (button.type)
            {
                case ButtonDecorator.Type.FREV:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_FREV.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.FWD:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_FWD.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Pause:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_Pause.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Play:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_Play.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Replay:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_Replay.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Reverse:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_Reverse.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Fullscreen:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_Fullscreen.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Template:
                    button.SetButtonImage(new Uri("..//..//Assets//Orange//Orange_Template.png", UriKind.Relative));
                    break;
            }
        }
    }
    public class Edgy : Dark
    {
        public override ImageBrush ChangeBackground()
        {
            ImageBrush ThemeColor = new ImageBrush();
            ThemeColor.ImageSource = new BitmapImage(new Uri("..//..//Assets//TempBGColors/TempEdgy.jpg", UriKind.Relative));
            return ThemeColor;
        }
        public override void ChangeButtonImage(ButtonDecorator button)
        {
            switch (button.type)
            {
                case ButtonDecorator.Type.FREV:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_FREV.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.FWD:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_FWD.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Pause:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_Pause.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Play:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_Play.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Replay:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_Replay.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Reverse:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_Reverse.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Fullscreen:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_Fullscreen.png", UriKind.Relative));
                    break;
                case ButtonDecorator.Type.Template:
                    button.SetButtonImage(new Uri("..//..//Assets//Edgy//Edgy_Template.png", UriKind.Relative));
                    break;
            }
        }
    }
}
