﻿using System;
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
        bool playing_fowards = true;
        public MainWindow()
        {
            InitializeComponent();
            vidTimer = new DispatcherTimer();
            vidTimer.Interval = TimeSpan.FromMilliseconds(42);
            vidTimer.Tick += new EventHandler(tickTimer);
            viewport.LoadedBehavior = MediaState.Manual;
            viewport.UnloadedBehavior = MediaState.Manual;
        }

        void tickTimer(object sender, EventArgs e)
        {
            Seeker.Value = viewport.Position.TotalMilliseconds;

            if (!playing_fowards && isPlaying) {
                Seeker.Value -= 42;
                //double pos = viewport.Position.TotalMilliseconds;
                //viewport.Position -= TimeSpan.FromMilliseconds(42);
            }   
        }
        //This probably won't remain a global variable
        bool isPlaying;
        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying == true)
            {
                viewport.Pause();
                isPlaying = false;
            }
            else if (isPlaying == false)
            {
                viewport.Play();
                isPlaying = true;
            }

        }

        private void DropDown(object sender, DragEventArgs e)
        {
            string vidFile = (string)((DataObject)e.Data).GetFileDropList()[0];
            //Sets the dropped file to the viewport
            viewport.Source = new Uri(vidFile);

            viewport.Volume = 1; //Temporary, will use a slider
            viewport.Play();
            isPlaying = true;
        }

        private void OpenMedia(object sender, RoutedEventArgs e)
        {
            TimeSpan mediaRunTime = viewport.NaturalDuration.TimeSpan;
            // Represents each millisecond of the video opened
            Seeker.Minimum = 0;
            Seeker.Maximum = mediaRunTime.TotalMilliseconds;
            //Starts a timer needed to run the video
            vidTimer.Start();
        }

        private void Seeker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            viewport.Position = TimeSpan.FromMilliseconds(Seeker.Value);
        }

        private void Playback_TextChanged(object sender, TextChangedEventArgs e)
        {
            double playback;
            
            //This is used to check whether or not the value in the box is a double
            bool parseTest = double.TryParse(ManualPlayback.Text, out playback);

            if (parseTest == true)
            {
                // only change the playback speed if between these values
                if (playback > 0 && playback < 3)
                    viewport.SpeedRatio = playback;
            }
            else if (parseTest == false)
            {
                //Sets this as the default speed if the value is not a double
                playback = 1.0;
                viewport.SpeedRatio = playback;
            }
        }

        //TEMPORARY, Want to use the Playback_TextChanged element for this feature
        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            playing_fowards = !playing_fowards;          
        }

        private void FREV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FFWD_Click(object sender, RoutedEventArgs e)
        {

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
            fileDialog.Filter = "MP4 Files (*.mp4)|*.mp4|WMV Files (*.wmv)|*.wmv|AVI Files (*.avi)|*.avi";
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
    }
}
