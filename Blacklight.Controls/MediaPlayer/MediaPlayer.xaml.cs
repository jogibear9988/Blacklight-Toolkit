//-----------------------------------------------------------------------
// <copyright file="MediaPlayer.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>Simple media player control.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    /// <summary>
    /// Simple media player control.
    /// </summary>
    public partial class MediaPlayer : UserControl
    {
        /// <summary>
        /// The media source property.
        /// </summary>
        public static readonly DependencyProperty MediaSourceProperty =
            DependencyProperty.Register("MediaSource", typeof(string), typeof(MediaPlayer), new PropertyMetadata(new PropertyChangedCallback(MediaSource_Changed)));

        #region Private members
        /// <summary>
        /// The progress timer.
        /// </summary>
        private DispatcherTimer progressTimer;

        /// <summary>
        /// Media ended flag.
        /// </summary>
        private bool mediaEnded = false;

        /// <summary>
        /// Bar visible flag.
        /// </summary>
        private bool barVisible = false;

        /// <summary>
        /// Stores the media element Uri.
        /// </summary>
        private Uri mediaSourceUri;

        /// <summary>
        /// Stores when the last click on the media element was.
        /// </summary>
        private DateTime lastMediaClick = DateTime.Now;
        #endregion

        /// <summary>
        /// Media player constructor.
        /// </summary>
        public MediaPlayer()
        {
            InitializeComponent();

            this.progressTimer = new DispatcherTimer();
            this.progressTimer.Interval = TimeSpan.FromSeconds(1);
            this.progressTimer.Tick += new EventHandler(this.ProgressTimer_Tick);

            this.SizeChanged += new SizeChangedEventHandler(this.MediaPlayer_SizeChanged);
            ((Storyboard)this.Resources["ShowBar"]).Completed += new EventHandler(this.ShowBar_Completed);
            this.media.BufferingProgressChanged += new RoutedEventHandler(this.Media_BufferingProgressChanged);
            this.media.MouseLeftButtonUp += new MouseButtonEventHandler(this.Media_MouseLeftButtonUp);
        }

        #region Public members
        /// <summary>
        /// Sets the media source.
        /// </summary>
        [System.ComponentModel.Category("Media Player Properties"), System.ComponentModel.Description("Sets the media source Uri.")]
        public Uri MediaSourceUri
        {
            set
            {
                this.mediaSourceUri = value;
                this.clickToPlayGrid.Opacity = 1;
                this.clickToPlayGrid.IsHitTestVisible = true;
            }
        }

        /// <summary>
        /// Gets or sets the media source Url.
        /// </summary>
        [System.ComponentModel.Category("Media Player Properties"), System.ComponentModel.Description("Sets the media source.")]
        public string MediaSource
        {
            get { return (string)GetValue(MediaSourceProperty); }
            set { SetValue(MediaSourceProperty, value); }
        }

        /// <summary>
        /// Updates the media source Uri.
        /// </summary>
        /// <param name="dependencyObject">The media player.</param>
        /// <param name="eventArgs">Dependency property changed event args.</param>
        private static void MediaSource_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            MediaPlayer mediaPlayer = (MediaPlayer)dependencyObject;
            mediaPlayer.MediaSourceUri = new Uri((string)eventArgs.NewValue, UriKind.RelativeOrAbsolute);
        }

        #endregion

        #region Size events
        /// <summary>
        /// Updates the progress bar and player clip.
        /// </summary>
        /// <param name="sender">The media player.</param>
        /// <param name="e">Size changed event args.</param>
        private void MediaPlayer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Update the scrub bar progress
            this.playPosition.Width = (this.media.Position.TotalSeconds / this.media.NaturalDuration.TimeSpan.TotalSeconds) * this.scrubCanvas.ActualWidth;

            // Updates the players clip.
            this.playerClip.Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
        }
        #endregion

        #region Player bar events
        /// <summary>
        /// Shows the bar.
        /// </summary>
        /// <param name="sender">The media element.</param>
        /// <param name="e">Mouse event args.</param>
        private void Media_MouseMove(object sender, MouseEventArgs e)
        {
            // If the bar isnt visible
            if (!this.barVisible)
            {
                // begin the show bar storyboard
                ((Storyboard)this.Resources["ShowBar"]).Begin();

                // set the visible flag to true
                this.barVisible = true;
            }
        }

        /// <summary>
        /// Sets the bar visible flag to false.
        /// </summary>
        /// <param name="sender">The show bar animation.</param>
        /// <param name="e">Event args.</param>
        private void ShowBar_Completed(object sender, EventArgs e)
        {
            this.barVisible = false;
        }

        /// <summary>
        /// Keeps the bar visible.
        /// </summary>
        /// <param name="sender">The controls bar.</param>
        /// <param name="e">Mouse event args.</param>
        private void Bar_MouseEnter(object sender, MouseEventArgs e)
        {
            // Stop the hide bar animation
            ((Storyboard)this.Resources["ShowBar"]).Stop();

            // Make sure the bar is in full view
            this.barTranslate.Y = 0;
        }

        /// <summary>
        /// Starts the animation to hide the bar.
        /// </summary>
        /// <param name="sender">The controls bar.</param>
        /// <param name="e">Mouse event args.</param>
        private void Bar_MouseLeave(object sender, MouseEventArgs e)
        {
            // Start the hide bar storyboard
            ((Storyboard)this.Resources["ShowBar"]).Begin();
        }
        #endregion

        #region Fullscreen events
        /// <summary>
        /// Toggles between fullscreen or not.
        /// </summary>
        /// <param name="sender">The media element.</param>
        /// <param name="e">Mouse button event args.</param>
        private void Media_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // The the last click was within 500ms...
            if ((DateTime.Now - lastMediaClick).TotalMilliseconds < 500)
            {
                // Toggle fullscreen
                Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
                this.lastMediaClick = DateTime.Now.AddSeconds(-1);
            }
            else
            {
                // Store the last click time
                this.lastMediaClick = DateTime.Now;
            }
        }
        #endregion

        #region Time display
        /// <summary>
        /// Updates the time display.
        /// </summary>
        /// <param name="sender">The progress timer.</param>
        /// <param name="e">Event args.</param>
        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            // Update the time UI
            this.UpdateTime();
        }

        /// <summary>
        /// Updates the time UI
        /// </summary>
        private void UpdateTime()
        {
            // Update the time text e.g. 01:50 / 03:30
            this.time.Text = string.Format(
                "{0}:{1} / {2}:{3}",
                Math.Floor(this.media.Position.TotalMinutes).ToString("00"),
                this.media.Position.Seconds.ToString("00"),
                Math.Floor(this.media.NaturalDuration.TimeSpan.TotalMinutes).ToString("00"),
                this.media.NaturalDuration.TimeSpan.Seconds.ToString("00"));

            // Update the rectangle showing the play position
            this.playPosition.Width = (this.media.Position.TotalSeconds / this.media.NaturalDuration.TimeSpan.TotalSeconds) * this.scrubCanvas.ActualWidth;
        }
        #endregion

        #region Media events
        /// <summary>
        /// Starts the media playing.
        /// </summary>
        /// <param name="sender">The media element.</param>
        /// <param name="e">Routed event args.</param>
        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Start the media
            this.media.Play();

            // Start the progress timer
            this.progressTimer.Start();

            // Set media ended flag to false
            this.mediaEnded = false;
        }

        /// <summary>
        /// Stores the UI timers.
        /// </summary>
        /// <param name="sender">The media element.</param>
        /// <param name="e">Routed event args.</param>
        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Update time UI
            this.UpdateTime();

            // Stop the progress timer
            this.progressTimer.Stop();

            // Set play pause toggle to be unchecked
            this.playPause.IsChecked = false;

            // Set media ended flag to false
            this.mediaEnded = true;

            // Show click to play grid
            this.clickToPlayGrid.Opacity = 1;
            this.clickToPlayGrid.IsHitTestVisible = true;
        }

        /// <summary>
        /// Updates the buffering UI.
        /// </summary>
        /// <param name="sender">The media element.</param>
        /// <param name="e">Routed event args.</param>
        private void Media_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            // If the media hasnt finished buffering...
            if (this.media.BufferingProgress != 1)
            {
                // Show the buffering text
                this.bufferingGrid.Opacity = 1;

                // Update the buffering text
                this.buffer.Text = string.Format("{0}%", (int)(this.media.BufferingProgress * 100));

                if (this.loadingAnimation.AnimationState != AnimationState.Playing)
                {
                    this.loadingAnimation.Begin();
                }
            }
            else 
            {
                // hide the buffering text
                this.bufferingGrid.Opacity = 0;

                this.loadingAnimation.Stop();
            }
        }

        /// <summary>
        /// Shows the media failed message and disables the control.
        /// </summary>
        /// <param name="sender">The media element.</param>
        /// <param name="e">Exception routed event args.</param>
        private void Media_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.failedGrid.Opacity = 1;
            this.IsHitTestVisible = false;
        }
        #endregion

        #region UI events
        /// <summary>
        /// Updates the media position.
        /// </summary>
        /// <param name="sender">The scrub bar.</param>
        /// <param name="e">Mouse button event args.</param>
        private void ScrubCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Hide the click to play stuff (if its showing)
            this.clickToPlayGrid.Opacity = 0;
            this.clickToPlayGrid.IsHitTestVisible = false;

            // Set the media ended flag to false
            this.mediaEnded = false;
            
            // Update the media element's positions
            this.media.Position = TimeSpan.FromSeconds((e.GetPosition(this.scrubCanvas).X / this.scrubCanvas.ActualWidth) * this.media.NaturalDuration.TimeSpan.TotalSeconds);

            // Update the time UI
            this.UpdateTime();
        }

        /// <summary>
        /// Plays the media.
        /// </summary>
        /// <param name="sender">The play pause toggle.</param>
        /// <param name="e">Routed event args.</param>
        private void PlayPause_Checked(object sender, RoutedEventArgs e)
        {
            // If the media has ended we need to rewind
            if (this.mediaEnded)
            {
                // set the media ended to false
                this.mediaEnded = false;

                // This checked event sometimes get called before the media element has loaded,
                // so we need to check its not null! If it isnt stop the media element which 
                // takes it back to the beginning
                if (this.media != null)
                {
                    this.media.Stop();
                }

                // Reset the time UI
                this.time.Text = "00:00 / 00:00";
                this.playPosition.Width = 0;

                // Hide the click to play UI
                this.clickToPlayGrid.Opacity = 0;
                this.clickToPlayGrid.IsHitTestVisible = false;
            }

            // If the media element has been initialised
            if (this.media != null)
            {
                // Play it...
                this.media.Play();

                // And start the update timer
                this.progressTimer.Start();
            }
        }

        /// <summary>
        /// Pauses the media.
        /// </summary>
        /// <param name="sender">The play pause toggle.</param>
        /// <param name="e">Routed event args.</param>
        private void PlayPause_Unchecked(object sender, RoutedEventArgs e)
        {
            // Pauses the media element
            this.media.Pause();
        }

        /// <summary>
        /// Starts the media playing.
        /// </summary>
        /// <param name="sender">The click to play border.</param>
        /// <param name="e">Mouse button event args.</param>
        private void ClickToPlayBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Set play / pause checked to true
            this.playPause.IsChecked = true;

            // Set the media elements source
            // We do this because we dont set the source until click to play
            // is clicked to save bandwidth
            this.media.Source = this.mediaSourceUri;

            // Hide the click to play UI
            this.clickToPlayGrid.Opacity = 0;
            this.clickToPlayGrid.IsHitTestVisible = false;
        }
        #endregion
    }
}
