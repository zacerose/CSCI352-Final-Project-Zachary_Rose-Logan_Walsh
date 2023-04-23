using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    public class Keybinder
    {
        public System.Windows.Input.Key PlayPause { get; set; }
        public System.Windows.Input.Key FREV { get; set; }
        public System.Windows.Input.Key FWD { get; set; }
        public System.Windows.Input.Key Reverse { get; set; }
        public System.Windows.Input.Key Mute { get; set; }
        public System.Windows.Input.Key VolumeUp { get; set; }
        public System.Windows.Input.Key VolumeDown { get; set; }
        public System.Windows.Input.Key SliderLeft { get; set; }
        public System.Windows.Input.Key SliderRight { get; set; }
        public System.Windows.Input.Key Fullscreen { get; set; }
        public void readKeybindingsFile()
        {

        }
        public void writeKeybindingsFile() 
        { 

        }
        public void restoreDefaults()
        {
            PlayPause = System.Windows.Input.Key.Space;
            FREV = System.Windows.Input.Key.OemComma;
            FWD = System.Windows.Input.Key.OemPeriod;
            Reverse = System.Windows.Input.Key.R;
            Mute = System.Windows.Input.Key.M;
            VolumeUp = System.Windows.Input.Key.Up;
            VolumeDown = System.Windows.Input.Key.Down;
            SliderLeft = System.Windows.Input.Key.Left;
            SliderRight = System.Windows.Input.Key.Right;
            Fullscreen = System.Windows.Input.Key.F;
        }

    }
}
