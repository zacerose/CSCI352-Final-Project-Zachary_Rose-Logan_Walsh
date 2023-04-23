using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaPlayer
{
    public class Keybinder
    {
        private string keybindingsFilePath = "..//..//Assets//keybindings.txt";
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

        public Keybinder()
        {
            readKeybindingsFile();
        }
        public void readKeybindingsFile()
        {
            Console.WriteLine("ENTERED READING FUNCTION");
            try
            {
                if (!File.Exists(keybindingsFilePath))
                {
                    restoreDefaults();
                    return;
                }
                else
                {
                    System.Windows.Input.Key key;
                    string[] lines = File.ReadAllLines(keybindingsFilePath);
                    if (Enum.TryParse(lines[0], out key))
                    {
                        PlayPause = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[1], out key))
                    {
                        FREV = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[2], out key))
                    {
                        FWD = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[3], out key))
                    {
                        Reverse = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[4], out key))
                    {
                        Mute = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[5], out key))
                    {
                        VolumeUp = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[6], out key))
                    {
                        VolumeDown = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[7], out key))
                    {
                        SliderLeft = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[8], out key))
                    {
                        SliderRight = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                    if (Enum.TryParse(lines[9], out key))
                    {
                        Fullscreen = key;
                    }
                    else
                    {
                        restoreDefaults();
                        return;
                    }
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
        }
        public void writeKeybindingsFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(keybindingsFilePath))
                {
                    sw.WriteLine(PlayPause.ToString());
                    sw.WriteLine(FREV.ToString());
                    sw.WriteLine(FWD.ToString());
                    sw.WriteLine(Reverse.ToString());
                    sw.WriteLine(Mute.ToString());
                    sw.WriteLine(VolumeUp.ToString());
                    sw.WriteLine(VolumeDown.ToString());
                    sw.WriteLine(SliderLeft.ToString());
                    sw.WriteLine(SliderRight.ToString());
                    sw.WriteLine(Fullscreen.ToString());
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
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
