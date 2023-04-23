using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for KeybindingsWindow.xaml
    /// </summary>
    public partial class KeybindingsWindow : Window
    {
        MainWindow mainwindow;
        bool awaitingInput = false;
        whichKey currentlyChanging;
        enum whichKey
        {
            PlayPause, FREV, FWD, Reverse, Mute, VolumeUp, VolumeDown, SliderLeft, SliderRight, Fullscreen
        }
        public KeybindingsWindow(MainWindow mw)
        {
            InitializeComponent();
            mainwindow = mw;
            resetButtonContent();
        }
        private void resetButtonContent()
        {
            btn_playPauseKeybinding.Content = mainwindow.keybinder.PlayPause.ToString();
            btn_FREVKeybinding.Content = mainwindow.keybinder.FREV.ToString();
            btn_FWDKeybinding.Content = mainwindow.keybinder.FWD.ToString();
            btn_ReverseKeybinding.Content = mainwindow.keybinder.Reverse.ToString();
            btn_MuteKeybinding.Content = mainwindow.keybinder.Mute.ToString();
            btn_VolumeUpKeybinding.Content = mainwindow.keybinder.VolumeUp.ToString();
            btn_VolumeDownKeybinding.Content = mainwindow.keybinder.VolumeDown.ToString();
            btn_SliderLeftKeybinding.Content = mainwindow.keybinder.SliderLeft.ToString();
            btn_SliderRightKeybinding.Content = mainwindow.keybinder.SliderRight.ToString();
            btn_FullscreenKeybinding.Content = mainwindow.keybinder.Fullscreen.ToString();
        }

        private void btn_playPauseKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.PlayPause;
            awaitingInput = true;
            btn_playPauseKeybinding.Content = "Awaiting Input";
        }

        private void btn_FREVKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.FREV;
            awaitingInput = true;
            btn_FREVKeybinding.Content = "Awaiting Input";
        }

        private void btn_FWDKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.FWD;
            awaitingInput = true;
            btn_FWDKeybinding.Content = "Awaiting Input";
        }

        private void btn_ReverseKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.Reverse;
            awaitingInput = true;
            btn_ReverseKeybinding.Content = "Awaiting Input";
        }

        private void btn_MuteKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.Mute;
            awaitingInput = true;
            btn_MuteKeybinding.Content = "Awaiting Input";
        }

        private void btn_VolumeUpKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.VolumeUp;
            awaitingInput = true;
            btn_VolumeUpKeybinding.Content = "Awaiting Input";
        }

        private void btn_VolumeDownKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.VolumeDown;
            awaitingInput = true;
            btn_VolumeDownKeybinding.Content = "Awaiting Input";
        }

        private void btn_SliderLeftKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.SliderLeft;
            awaitingInput = true;
            btn_SliderLeftKeybinding.Content = "Awaiting Input";
        }

        private void btn_SliderRightKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.SliderRight;
            awaitingInput = true;
            btn_SliderRightKeybinding.Content = "Awaiting Input";

        }

        private void btn_FullscreenKeybinding_Click(object sender, RoutedEventArgs e)
        {
            if (awaitingInput)
            {
                resetButtonContent();
            }
            currentlyChanging = whichKey.Fullscreen;
            awaitingInput = true;
            btn_FullscreenKeybinding.Content = "Awaiting Input";
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (awaitingInput)
            {
                switch (currentlyChanging)
                {
                    case whichKey.PlayPause:
                        mainwindow.keybinder.PlayPause = e.Key;
                        awaitingInput = false;
                        btn_playPauseKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.FREV:
                        mainwindow.keybinder.FREV = e.Key;
                        awaitingInput = false;
                        btn_FREVKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.FWD:
                        mainwindow.keybinder.FWD = e.Key;
                        awaitingInput = false;
                        btn_FWDKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.Reverse:
                        mainwindow.keybinder.Reverse = e.Key;
                        awaitingInput = false;
                        btn_ReverseKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.Mute:
                        mainwindow.keybinder.Mute = e.Key;
                        awaitingInput = false;
                        btn_MuteKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.VolumeUp:
                        mainwindow.keybinder.VolumeUp = e.Key;
                        awaitingInput = false;
                        btn_VolumeUpKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.VolumeDown:
                        mainwindow.keybinder.VolumeDown = e.Key;
                        awaitingInput = false;
                        btn_VolumeDownKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.SliderLeft:
                        mainwindow.keybinder.SliderLeft = e.Key;
                        awaitingInput = false;
                        btn_SliderLeftKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.SliderRight:
                        mainwindow.keybinder.SliderRight = e.Key;
                        awaitingInput = false;
                        btn_SliderRightKeybinding.Content = e.Key.ToString();
                        break;
                    case whichKey.Fullscreen:
                        mainwindow.keybinder.Fullscreen = e.Key;
                        awaitingInput = false;
                        btn_FullscreenKeybinding.Content = e.Key.ToString();
                        break;
                }
            }
        }
    }
}
