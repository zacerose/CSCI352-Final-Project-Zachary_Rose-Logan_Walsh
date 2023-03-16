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
//Last Edited: 3-14-2023

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //This is the timer used for running the video
        DispatcherTimer vidTimer;
        public MainWindow()
        {
            InitializeComponent();
            vidTimer = new DispatcherTimer();
            vidTimer.Interval = TimeSpan.FromMilliseconds(500);
            vidTimer.Tick += new EventHandler(tickTimer);
            viewport.LoadedBehavior = MediaState.Manual;
            viewport.UnloadedBehavior = MediaState.Manual;
        }

        void tickTimer(object sender, EventArgs e)
        {
            Seeker.Value = viewport.Position.TotalSeconds;
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
            Seeker.Maximum = mediaRunTime.TotalSeconds;
            //Starts a timer needed to run the video
            vidTimer.Start();
        }

        private void Seeker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            viewport.Position = TimeSpan.FromSeconds(Seeker.Value);
        }
    }
}
