using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private Grid mainGrid;
        DispatcherTimer timer;   //  Generation timer
        private int genCounter;
        private List<AdWindow> adWindows;


        public MainWindow()
        {
            InitializeComponent();
            mainGrid = new Grid(MainCanvas);

            timer = new DispatcherTimer();
            timer.Tick += OnTimer;
            timer.Interval = TimeSpan.FromMilliseconds(200);
        }


        private void StartAd()
        {

            {
                adWindows = new List<AdWindow>();
                for (int i = 0; i < 2; i++)
                {
                    var adWindow = new AdWindow(this);
                    adWindow.Closed += AdWindowOnClosed;
                    adWindow.Top = this.Top + (330 * i) + 70;
                    adWindow.Left = this.Left + 240;
                    adWindow.Show();
                    adWindows.Add(adWindow);
                }


            }
        }

        private void AdWindowOnClosed(object sender, EventArgs eventArgs)
        {
            var adWindow = sender as AdWindow;
            adWindow.Closed -= AdWindowOnClosed;
            adWindows.Remove(adWindow);

            if (!adWindows.Any())
            {
                adWindows = null;
            }
        }


        private void Button_OnClick(object sender, EventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                ButtonStart.Content = "Stop";
                StartAd();
            }
            else
            {
                timer.Stop();
                ButtonStart.Content = "Start";
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            mainGrid.Update();
            genCounter++;
            lblGenCount.Content = "Generations: " + genCounter;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Clear();
        }


    }
}
