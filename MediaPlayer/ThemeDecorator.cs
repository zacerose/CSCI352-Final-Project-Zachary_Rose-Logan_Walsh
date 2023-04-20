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
    using System.IO;

    namespace Decorator
    {
        public abstract class ButtonDecorator : ButtonBase
        {
            Button newButtonPause;
            Button newButtonRev;
            Button newButtonFFWD;
            Button newButtonFREV;
            string fpath1;
            string fpath2;
            string fpath3;
            string fpath4;

            public ButtonDecorator(Button p, Button r, Button ff, Button fr, string nfp1, string nfp2, string nfp3, string nfp4)
            {
                newButtonPause = p;
                newButtonRev = r;
                newButtonFFWD = ff;
                newButtonFREV = fr;
                nfp1 = fpath1 = nfp1;
                fpath2 = nfp2;
                fpath3 = nfp3;
                fpath4 = nfp4;
            }
            public virtual void SetButtonImage()
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
            public ImageDecorator(Button p, Button r, Button ff, Button fr, string nfp1, string nfp2, string nfp3, string nfp4) : base(p, r, ff, fr, nfp1, nfp2, nfp3, nfp4) { }
            public override void SetButtonImage()
            {

                base.SetButtonImage();

            }
        }
    }
}
