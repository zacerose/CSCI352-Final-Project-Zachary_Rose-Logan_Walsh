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
using MediaPlayer.Decorator;

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

        // abstract factory used to change the theme when needed
        ThemeFactory theme = new StandardFactory();

        double videoPosition;

        bool playing_fowards = true;
        bool draggingSeeker = false;
        // whether or not the video is paused
        bool isPlaying;
        public MainWindow()
        {
            InitializeComponent();
            vidTimer = new DispatcherTimer();
            //vidTimer.Interval = TimeSpan.FromMilliseconds(18);
            vidTimer.Interval = TimeSpan.FromMilliseconds(100);

            vidTimer.Tick += new EventHandler(TickTimer);

            reverseTimer = new DispatcherTimer();
            reverseTimer.Tick += new EventHandler(ReverseTimer);

            //seekerUpdateTimer.Start();

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

        void ReverseTimer(object sender, EventArgs e) {
            if (isPlaying)
            {
                Seeker.Value = viewport.Position.TotalMilliseconds - (reverseTimer.Interval.TotalMilliseconds * parse_SpeedRatio());
                lbl_time_remaining.Content = String.Format("{0}/{1}", viewport.Position.ToString(@"hh\:mm\:ss"), viewport.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));
                viewport.Position = TimeSpan.FromMilliseconds(Seeker.Value);
            }
        }
        void TickTimer(object sender, EventArgs e)
        {
            // if video is at the end, change text of play button to replay
            // later, when queues are implemented, this will have to check if there is another video to play afterwards
            ImageBrush playBrush = new ImageBrush();
            playBrush.ImageSource = new BitmapImage(new Uri(playFilepath, UriKind.Relative));
            try
            {
                if (viewport.Position == viewport.NaturalDuration.TimeSpan)
                {
                    PlayPause.Background = playBrush;
                    //PlayPause.Content = "Replay";
                    isPlaying = false;
                }
            }
            catch (System.InvalidOperationException error) {
                // catches error that happens between selecting videos
            }
            if (isPlaying)
            {
                updateSeeker();
                lbl_time_remaining.Content = String.Format("{0}/{1}", viewport.Position.ToString(@"hh\:mm\:ss"), viewport.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));
                videoPosition = viewport.Position.TotalMilliseconds;
            }
        }
        void updateSeeker()
        {
            Seeker.Value = viewport.Position.TotalMilliseconds;
        }

        //Kept outside for future use

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            //These are used to check the source of the buttons based on the selected theme
            ImageBrush playBrush = new ImageBrush();
            playBrush.ImageSource = new BitmapImage(new Uri(playFilepath, UriKind.Relative));
            ImageBrush pauseBrush = new ImageBrush();
            pauseBrush.ImageSource = new BitmapImage(new Uri(pauseFilepath, UriKind.Relative));

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
                PlayPause.Background = pauseBrush;
                return;
            }
            // pause the media
            if (isPlaying == true)
            {
                viewport.Pause();
                isPlaying = false;
                if (!draggingSeeker)
                    PlayPause.Background = playBrush;
            }
            // unpause the media
            else
            {
                viewport.Play();
                //vidTimer.Start();
                isPlaying = true;
                if (!draggingSeeker)
                    PlayPause.Background = pauseBrush;
            }
#if SPEED3
            // weird solution that allows the player to maintain speeds over 2 between pauses
            if (speed > 2)
            {
                viewport.SpeedRatio = 2;
                Thread.Sleep(6);
            }
#endif
            viewport.SpeedRatio = speed;

        }

        private void DropDown(object sender, DragEventArgs e)
        {
            string vidFile = (string)((DataObject)e.Data).GetFileDropList()[0];
            //Sets the dropped file to the viewport
            viewport.Source = new Uri(vidFile);

            viewport.Volume = 1; //Temporary, will use a slider
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
#if SPEED3

                // speed caps at 3
                if (speed <= 2.75)
                {
                    speed += 0.25;
                    ManualPlayback.Text = speed.ToString();
                }
                else
                    ManualPlayback.Text = 3.ToString();
            }
#else
                // speed caps at 2
                if (speed <= 1.75)
                {
                    speed += 0.25;
                    ManualPlayback.Text = speed.ToString();
                }
                else
                    ManualPlayback.Text = 2.ToString();
            }
#endif
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
#if SPEED3
                // speed caps at 3
                if (speed <= 2.75)
                {
                    speed += 0.25;
                    ManualPlayback.Text = speed.ToString();
                }
                else
                    ManualPlayback.Text = 3.ToString();
#else
                // speed caps at 2
                if (speed <= 1.75)
                {
                    speed += 0.25;
                    ManualPlayback.Text = speed.ToString();
                }
                else
                    ManualPlayback.Text = 2.ToString();
            }
#endif
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
        void PropertyValues()
        {

            //playback = double.Parse(Playback_TextChanged.Text);
            //viewport.SpeedRatio = playback;

        }

        private void FileMenu_Click(object sender, RoutedEventArgs e)
        {

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

                viewport.Volume = 1; //Temporary, will use a slider
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
        
        //These are used to make sure the buttons when pressing play/pause stay consistent with their themes
        string playFilepath = "..//..//assets/default/Def_Play.png";
        string pauseFilepath = "..//..//assets/default/Def_Pause.png";
        private void DefaultTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new StandardFactory();
            SetBackground(theme.GetLight().ChangeImage());

            playFilepath = "..//..//assets/default/Def_Play.png";
            pauseFilepath = "..//..//assets/default/Def_Pause.png";
            lbl_time_remaining.Foreground = Brushes.Black;
            lbl_playback_text.Foreground = Brushes.Black;

            string f1 = "..//..//assets/Default/Def_Pause.png";
            string f2 = "..//..//assets/Default/Def_Template.png";
            string f3 = "..//..//assets/Default/Def_FWD.png";
            string f4 = "..//..//assets/Default/Def_FREV.png";
            SwapButtons(f1, f2, f3, f4);

        }

        private void NightTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new StandardFactory();
            SetBackground(theme.GetDark().ChangeImage());

            playFilepath = "..//..//assets/Night/Night_Play.png";
            pauseFilepath = "..//..//assets/Night/Night_Pause.png";
            lbl_time_remaining.Foreground = Brushes.White;
            lbl_playback_text.Foreground = Brushes.White;

            string f1 = "..//..//assets/Night/Night_Pause.png";
            string f2 = "..//..//assets/Night/Night_Template.png";
            string f3 = "..//..//assets/Night/Night_FWD.png";
            string f4 = "..//..//assets/Night/Night_FREV.png";
            SwapButtons(f1, f2, f3, f4);
        }

        private void OrangeTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new AdditionalFactory();
            SetBackground(theme.GetLight().ChangeImage());

            playFilepath = "..//..//assets/Orange/Orange_Play.png";
            pauseFilepath = "..//..//assets/Orange/Orange_Pause.png";
            lbl_time_remaining.Foreground = Brushes.Black;
            lbl_playback_text.Foreground = Brushes.Black;

            string f1 = "..//..//assets/Orange/Orange_Pause.png";
            string f2 = "..//..//assets/Orange/Orange_Template.png";
            string f3 = "..//..//assets/Orange/Orange_FWD.png";
            string f4 = "..//..//assets/Orange/Orange_FREV.png";
            SwapButtons(f1, f2, f3, f4);
        }

        private void EdgyTheme_Click(object sender, RoutedEventArgs e)
        {
            theme = new AdditionalFactory();
            SetBackground(theme.GetDark().ChangeImage());

            playFilepath = "..//..//assets/Edgy/Edgy_Play.png";
            pauseFilepath = "..//..//assets/Edgy/Edgy_Pause.png";
            lbl_time_remaining.Foreground = Brushes.White;
            lbl_playback_text.Foreground = Brushes.White;

            string f1 = "..//..//assets/Edgy/Edgy_Pause.png";
            string f2 = "..//..//assets/Edgy/Edgy_Template.png";
            string f3 = "..//..//assets/Edgy/Edgy_FWD.png";
            string f4 = "..//..//assets/Edgy/Edgy_FREV.png";
            SwapButtons(f1, f2, f3, f4);
        }

        private void SwapButtons(string f1, string f2, string f3, string f4)
        {
            Button b1 = PlayPause;
            Button b2 = Reverse;
            Button b3 = FastForward;
            Button b4 = FastBackward;
            ImageDecorator i = new ImageDecorator(b1, b2, b3, b4, f1, f2, f3, f4);
            i.SetButtonImage();
            //PlayPause = b1;
            //Reverse = b2;
            //FastForward = b3;
            //FastBackward = b4;
        }
        //Will be used to also change the buttons with whatever color is wanted. 
        private void SetBackground(ImageBrush backColor)
        {
            MainUI.Background = backColor;
        }

        // for clicking the seeker
        private void Seeker_MouseUp(object sender, MouseButtonEventArgs e)
        {
            viewport.Position = TimeSpan.FromMilliseconds(Seeker.Value);

            double speed = parse_SpeedRatio();
#if SPEED3
            // weird solution that allows the player to maintain speeds over 2 between pauses
            if (speed > 2)
            {
                viewport.SpeedRatio = 2;
                Thread.Sleep(6);
            }
#endif
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

        private void MuteClick(object sender, RoutedEventArgs e)
        {
            if (slider_volume.Value > 0)
            {
                slider_volume.Value = 0;
            }
            else
            {
                slider_volume.Value = 0.50;
            }
        }
    }
}