using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading; //Added for the timer
using System.Windows.Shapes;

//Video player Program by Logan Walsh and Zachary Rose
//Last Edited: 3-18-2023

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //This is the timer used for running the video
        DispatcherTimer vidTimer;
        DispatcherTimer reverseTimer;

        public Keybinder keybinder = new Keybinder();
        // abstract product generated from a factory, used to change colors and buttons when needed
        public Theme theme = new Gray();

        // persistent value that allows returning to previous volume after unmuting
        double volume;

        bool playing_fowards = true;
        bool draggingSeeker = false;
        // whether or not the video is paused
        bool isPlaying;
        // True if MediaElement should keep its aspect ratio, false if it should fill the window
        bool enforceAspectRatio = true;
        public MainWindow()
        {
            InitializeComponent();
            vidTimer = new DispatcherTimer();
            //vidTimer.Interval = TimeSpan.FromMilliseconds(18);
            vidTimer.Interval = TimeSpan.FromMilliseconds(100);

            vidTimer.Tick += new EventHandler(TickTimer);

            reverseTimer = new DispatcherTimer();
            reverseTimer.Tick += new EventHandler(ReverseTimer);

            Seeker.SmallChange = 100;
            Seeker.LargeChange = 200;
            Seeker.Minimum = 0;
            // Seeker.Maximum set upon loading media

            slider_volume.Minimum = 0;
            slider_volume.Maximum = 1;
            slider_volume.Value = 0.5;

            viewport.LoadedBehavior = MediaState.Manual;
            viewport.UnloadedBehavior = MediaState.Manual;
            lbl_time_remaining.Visibility = Visibility.Hidden;

        }
        // an approximation of playing in reverse
        void ReverseTimer(object sender, EventArgs e) {
            if (isPlaying)
            {
                Seeker.Value = viewport.Position.TotalMilliseconds - (reverseTimer.Interval.TotalMilliseconds * parse_SpeedRatio());
                lbl_time_remaining.Content = String.Format("{0}/{1}", viewport.Position.ToString(@"hh\:mm\:ss"), viewport.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));
                viewport.Position = TimeSpan.FromMilliseconds(Seeker.Value);
            }
        }
        // controls updating the seeker and elapsed time
        void TickTimer(object sender, EventArgs e)
        {
            // if video is at the end, change text of play button to replay
            try
            {
                if (viewport.Position == viewport.NaturalDuration.TimeSpan)
                {
                    PlayPause.type = ButtonDecorator.Type.Replay;
                    theme.ChangeButtonImage(PlayPause);
                    isPlaying = false;
                }
                // if the video isn't at the end, update the elapsed time
                else
                    lbl_time_remaining.Content = String.Format("{0}/{1}", viewport.Position.ToString(@"hh\:mm\:ss"), viewport.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));
            }
            catch (System.InvalidOperationException error) {
                // catches error that happens between selecting videos, doesn't need to be handled
            }
            if (isPlaying)
            {
                updateSeeker();             
            }
        }
        void updateSeeker()
        {
            Seeker.Value = viewport.Position.TotalMilliseconds;
        }

        
        // pauses or unpauses the playback
        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            double speed = parse_SpeedRatio();

            // don't do anything if the media isn't loaded yet
            if (!viewport.CanPause)
                return;
            // if the video is over, play again from the beginning
            if (viewport.Position == viewport.NaturalDuration.TimeSpan)
            {
                if (playing_fowards)
                    Seeker.Value = 0;
                isPlaying = true;
                PlayPause.type = ButtonDecorator.Type.Pause;
                theme.ChangeButtonImage(PlayPause);
                return;
            }
            // pause the media
            if (isPlaying == true)
            {
                viewport.Pause();
                isPlaying = false;
                if (!draggingSeeker)
                {
                    PlayPause.type = ButtonDecorator.Type.Play;
                    theme.ChangeButtonImage(PlayPause);
                }
            }
            // unpause the media
            else
            {
                viewport.Play();
                //vidTimer.Start();
                isPlaying = true;
                if (!draggingSeeker)
                {
                    PlayPause.type = ButtonDecorator.Type.Pause;
                    theme.ChangeButtonImage(PlayPause);
                }
            }
            viewport.SpeedRatio = speed;

        }

        private void DropDown(object sender, DragEventArgs e)
        {
            string vidFile = (string)((DataObject)e.Data).GetFileDropList()[0];
            //Sets the dropped file to the viewport
            viewport.Source = new Uri(vidFile);

            viewport.Volume = slider_volume.Value;
        }

        private void OpenMedia(object sender, RoutedEventArgs e)
        {
            TimeSpan mediaRunTime = viewport.NaturalDuration.TimeSpan;
            // Represents each millisecond of the video opened
            Seeker.Maximum = mediaRunTime.TotalMilliseconds;
            //Starts a timer needed to run the video

            reverseTimer.Interval = vidTimer.Interval;

            vidTimer.Start();
            viewport.Play();

            // if the video has no audio, hide the volume slider
            if (viewport.HasAudio)
            {
                slider_volume.Visibility = Visibility.Visible;
                slider_volume.Value = 0.5;
            }
            else
            {
                slider_volume.Visibility = Visibility.Collapsed;
                slider_volume.Value = 0;
            }
            lbl_time_remaining.Visibility = Visibility.Visible;
            lbl_time_remaining.Content = String.Format("0/{0}", mediaRunTime.ToString(@"hh\:mm\:ss"));
            isPlaying = true;
            viewport.IsMuted = false;
            playing_fowards = true;
            viewport.SpeedRatio = parse_SpeedRatio();
        }

        private void Seeker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            viewport.Position = TimeSpan.FromMilliseconds(Seeker.Value);
        }

        private void Playback_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewport.SpeedRatio = parse_SpeedRatio();
        }
        // calculates a speed from the playback speed textbox.
        // always returns some valid value, even given invalid input
        private double parse_SpeedRatio()
        {
            double playback;

            //This is used to check whether or not the value in the box is a double
            bool parseTest = double.TryParse(ManualPlayback.Text, out playback);

            if (parseTest == true)
            {
                // only change the playback speed if between these values
                if (playback >= 0 && playback <= 2)
                    return playback;              
            }
            else if (parseTest == false)
            {
                //Sets this as the default speed if the value is not a double
                playback = 1.0;
            }

            return playback;
        }
        //TEMPORARY, Want to use the Playback_TextChanged element for this feature
        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            if (playing_fowards)
            {

                playing_fowards = false;
                vidTimer.Stop();
                reverseTimer.Start();
                viewport.IsMuted = true;
                //viewport.Pause();
            }
            else
            {
                playing_fowards = true;
                vidTimer.Start();
                reverseTimer.Stop();
                viewport.IsMuted = false;      
                //viewport.Play();
            }
            viewport.SpeedRatio = parse_SpeedRatio();
        }

        private void FREV_Click(object sender, RoutedEventArgs e)
        {
            double speed = parse_SpeedRatio();
            // if already playing backwards, play faster (backwards)
            if (!playing_fowards)
            {
                // speed caps at 2
                if (speed <= 1.75)
                {
                    speed += 0.25;
                    ManualPlayback.Text = speed.ToString();
                }
                else
                    ManualPlayback.Text = 2.ToString();
            }
            // if playing forwards, go closer to playing backwards
            else
                {
                speed -= 0.25;
                if (speed < 0)
                {
                    Reverse_Click(this, e);
                    speed *= -1;
                }
                ManualPlayback.Text = speed.ToString();
            }
        }

        private void FFWD_Click(object sender, RoutedEventArgs e)
        {
            double speed = parse_SpeedRatio();
            // if already playing forwards, play faster
            if (playing_fowards)
            {
                // speed caps at 2
                if (speed <= 1.75)
                {
                    speed += 0.25;
                    ManualPlayback.Text = speed.ToString();
                }
                else
                    ManualPlayback.Text = 2.ToString();
            }
            // if in reverse, go closer to playing forwards
            else
            {
                speed -= 0.25;
                if (speed < 0)
                {
                    Reverse_Click(this, e);
                    speed *= -1;
                }
                ManualPlayback.Text = speed.ToString();
            }
        }

        private void FileMenu_Click(object sender, RoutedEventArgs e)
        {

        }


        private void AspRatio_Click(object sender, RoutedEventArgs e)
        {
            
            if (enforceAspectRatio == true)
            {
                viewport.Stretch = Stretch.Fill;
                AspStretch.Header = "Enforce media aspect ratio";
                enforceAspectRatio = false;
            }
            else
            {
                viewport.Stretch = Stretch.Uniform;
                AspStretch.Header = "Stretch media to fill";
                enforceAspectRatio = true;
            }
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".mp4";
            fileDialog.Filter = "All Files (*.*)|*|MP4 Files (*.mp4)|*.mp4|WMV Files (*.wmv)|*.wmv|AVI Files (*.avi)|*.avi|WAV Files (*.wav)|*.wav|MP3 Files (*.mp3)|*.mp3";
            Nullable<bool> success = fileDialog.ShowDialog();
            if (success == true)
            {
                string vidFile = fileDialog.FileName;
                viewport.Source = new Uri(vidFile);

                viewport.Volume = slider_volume.Value;
                viewport.Play();
                isPlaying = true;
            }
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            string AboutText = "Video Player Program (2023), developed by Zachary Rose and Logan Walsh.";
            string txt = "About";
            MessageBox.Show(AboutText, txt);
        }

        private void Instructions_Click(object sender, RoutedEventArgs e)
        {
            string AboutText = "Load a video with the load file button, click the buttons to do stuff. Sorry, we couldn't afford any better instructions, we're on a strict schedule here.";
            string txt = "User's Manual";
            MessageBox.Show(AboutText, txt);
        }
        
        private void UpdateWindowTheme()
        {
            MainUI.Background = theme.ChangeBackground();

            lbl_time_remaining.Foreground = theme.ChangeLabelColor();
            lbl_playback_text.Foreground = theme.ChangeLabelColor();

            theme.ChangeButtonImage(FastBackward);
            theme.ChangeButtonImage(PlayPause);
            theme.ChangeButtonImage(Reverse);
            theme.ChangeButtonImage(FastForward);
            theme.ChangeButtonImage(FullScreen);
        }
        private void DefaultTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new StandardFactory().GetLight();
            UpdateWindowTheme();
        }

        private void NightTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new StandardFactory().GetDark();
            UpdateWindowTheme();
        }

        private void OrangeTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new AdditionalFactory().GetLight();
            UpdateWindowTheme();
        }

        private void EdgyTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new AdditionalFactory().GetDark();
            UpdateWindowTheme();
        }
        // opens up a window for setting keybindings
        private void Hotkeys_Click(object sender, RoutedEventArgs e)
        {
            KeybindingsWindow keybindingsWindow = new KeybindingsWindow(this);
            keybindingsWindow.Show();
        }
        // for clicking the seeker
        private void Seeker_MouseUp(object sender, MouseButtonEventArgs e)
        {
            viewport.Position = TimeSpan.FromMilliseconds(Seeker.Value);

            double speed = parse_SpeedRatio();
            viewport.SpeedRatio = speed;
        }
        // for dragging the seeker
        private void Seeker_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                PlayPause_Click(this, e);

                draggingSeeker = true;
            }
        }

        private void Seeker_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggingSeeker)
            {
                draggingSeeker = false;
                viewport.Position = TimeSpan.FromMilliseconds(Seeker.Value);
                PlayPause_Click(this, e);
            }
        }

        private void slider_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            viewport.Volume = slider_volume.Value;
        }
        // mutes the volume of the media player, or unmutes if it was already muted
        private void MuteClick(object sender, RoutedEventArgs e)
        {
            if (slider_volume.Value > 0)
            {
                volume = slider_volume.Value;
                slider_volume.Value = 0;
            }
            else
            {
                slider_volume.Value = volume;
            }
        }
        // captures hotkey input
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == keybinder.PlayPause)
            {
                PlayPause_Click(sender, e);
            }
            else if (e.Key == keybinder.FREV)
            {
                FREV_Click(sender, e);
            }
            else if (e.Key == keybinder.FWD)
            {
                FFWD_Click(sender, e);
            }
            else if (e.Key == keybinder.Reverse)
            {
                Reverse_Click(sender, e);
            }
            else if (e.Key == keybinder.Mute)
            {
                MuteClick(sender, e);
            }
            else if (e.Key == keybinder.VolumeUp)
            {
                slider_volume.Value += .1;
            }
            else if (e.Key == keybinder.VolumeDown)
            {
                slider_volume.Value -= .1;
            }
            else if (e.Key == keybinder.SliderLeft)
            {
                Seeker.Value -= Seeker.Maximum / 20;
            }
            else if (e.Key == keybinder.SliderRight)
            {
                Seeker.Value += Seeker.Maximum / 20;
            }
            else if (e.Key == keybinder.Fullscreen)
            {
               FullScreen_Click(sender, e);
            }
        }
        // Maximizes the windows
        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                this.Left = RestoreBounds.Left;
                this.Top = RestoreBounds.Top;
                this.Width = RestoreBounds.Width;
                this.Height = RestoreBounds.Height;
            }
            else
                this.WindowState = WindowState.Maximized;
        }
        // Allows removing focus from the textbox when clicking the window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainUI.Focus();
        }
    }
}