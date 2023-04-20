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
    using System;

    namespace Decorator
    {
        public abstract class ButtonDecorator : ButtonBase
        {
            Button newButtonPause;
            Button newButtonRev;
            Button newButtonFFWD;
            Button newButtonFREV;

            public ButtonDecorator(Button p, Button r, Button ff, Button fr)
            {
                newButtonPause = p;
                newButtonRev = r;
                newButtonFFWD = ff;
                newButtonFREV = fr;
            }
            public virtual void SetButtonImage(string fpath1, string fpath2, string fpath3, string fpath4)
            {

                Button Button = new Button();
                ImageBrush ButtonThemeP = new ImageBrush();
                ImageBrush ButtonThemeR = new ImageBrush();
                ImageBrush ButtonThemeFF = new ImageBrush();
                ImageBrush ButtonThemeFR = new ImageBrush();
                ButtonThemeP.ImageSource = new BitmapImage(new Uri(fpath1, UriKind.Relative));
                ButtonThemeR.ImageSource = new BitmapImage(new Uri(fpath2, UriKind.Relative));
                ButtonThemeFF.ImageSource = new BitmapImage(new Uri(fpath3, UriKind.Relative));
                ButtonThemeFR.ImageSource = new BitmapImage(new Uri(fpath4, UriKind.Relative));
                newButtonPause.Background = (ButtonThemeP);
                newButtonRev.Background = (ButtonThemeR);
                newButtonFFWD.Background = (ButtonThemeFF);
                newButtonFREV.Background = (ButtonThemeFR);
            }
        }
        public class ImageDecorator : ButtonDecorator
        {
            public ImageDecorator(Button p, Button r, Button ff, Button fr) : base(p, r, ff, fr) { }
            public override void SetButtonImage(string fpath1, string fpath2, string fpath3, string fpath4)
            {
                string nfp1 = fpath1;
                string nfp2 = fpath2;
                string nfp3 = fpath3;
                string nfp4 = fpath4;
                base.SetButtonImage(nfp1, nfp2, nfp3, nfp4);

            }
        }
    }
}
