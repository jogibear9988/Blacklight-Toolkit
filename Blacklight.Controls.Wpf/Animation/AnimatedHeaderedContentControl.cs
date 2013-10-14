//-----------------------------------------------------------------------
// <copyright file="AnimatedHeaderedContentControl.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>02-Dec-2008</date>
// <author>Martin Grayson</author>
// <summary>Animated headered content control base class.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls.Wpf
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// Animated headered content control base class.
    /// </summary>
    public class AnimatedHeaderedContentControl : HeaderedContentControl
    {
        #region Private memebers
        /// <summary>
        /// Stores the width key frame.
        /// </summary>
        private SplineDoubleKeyFrame sizeAnimationWidthKeyFrame;

        /// <summary>
        /// Stores the height key frame.
        /// </summary>
        private SplineDoubleKeyFrame sizeAnimationHeightKeyFrame;

        /// <summary>
        /// Stores the posisition X key frame.
        /// </summary>
        private SplineDoubleKeyFrame positionAnimationXKeyFrame;

        /// <summary>
        /// Stores the position Y keyframe.
        /// </summary>
        private SplineDoubleKeyFrame positionAnimationYKeyFrame;

        /// <summary>
        /// Stores the size animation.
        /// </summary>
        private Storyboard sizeAnimation;

        /// <summary>
        /// Stores the position animation.
        /// </summary>
        private Storyboard positionAnimation;

        /// <summary>
        /// Stores a flag indicating if the size is animating.
        /// </summary>
        private bool sizeAnimating;

        /// <summary>
        /// Stores a flag storing if the position is animating.
        /// </summary>
        private bool positionAnimating;

        /// <summary>
        /// Stores the size animation timespan.
        /// </summary>
        private TimeSpan sizeAnimationTimespan = new TimeSpan(0, 0, 0, 0, 500);

        /// <summary>
        /// Stores the position animation time span.
        /// </summary>
        private TimeSpan positionAnimationTimespan = new TimeSpan(0, 0, 0, 0, 500);
        #endregion

        /// <summary>
        /// Blank Constructor
        /// </summary>
        public AnimatedHeaderedContentControl()
        {
            this.sizeAnimation = new Storyboard();
            DoubleAnimationUsingKeyFrames widthAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard.SetTargetProperty(widthAnimation, new System.Windows.PropertyPath("(FrameworkElement.Width)"));
            this.sizeAnimationWidthKeyFrame = new SplineDoubleKeyFrame();
            this.sizeAnimationWidthKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            this.sizeAnimationWidthKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            this.sizeAnimationWidthKeyFrame.Value = 0;
            widthAnimation.KeyFrames.Add(this.sizeAnimationWidthKeyFrame);

            DoubleAnimationUsingKeyFrames heightAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(heightAnimation, this);
            Storyboard.SetTargetProperty(heightAnimation, new System.Windows.PropertyPath("(FrameworkElement.Height)"));
            this.sizeAnimationHeightKeyFrame = new SplineDoubleKeyFrame();
            this.sizeAnimationHeightKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            this.sizeAnimationHeightKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            this.sizeAnimationHeightKeyFrame.Value = 0;
            heightAnimation.KeyFrames.Add(this.sizeAnimationHeightKeyFrame);
            
            this.sizeAnimation.Children.Add(widthAnimation);
            this.sizeAnimation.Children.Add(heightAnimation);
            this.sizeAnimation.Completed += new EventHandler(this.SizeAnimation_Completed);

            this.positionAnimation = new Storyboard();

            DoubleAnimationUsingKeyFrames positionXAnimation = new DoubleAnimationUsingKeyFrames();  
            Storyboard.SetTarget(positionXAnimation, this);
            Storyboard.SetTargetProperty(positionXAnimation, new System.Windows.PropertyPath("(Canvas.Left)"));
            this.positionAnimationXKeyFrame = new SplineDoubleKeyFrame();
            this.positionAnimationXKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            this.positionAnimationXKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            this.positionAnimationXKeyFrame.Value = 0;
            positionXAnimation.KeyFrames.Add(this.positionAnimationXKeyFrame);

            DoubleAnimationUsingKeyFrames positionYAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(positionYAnimation, this);
            Storyboard.SetTargetProperty(positionYAnimation, new System.Windows.PropertyPath("(Canvas.Top)"));
            this.positionAnimationYKeyFrame = new SplineDoubleKeyFrame();
            this.positionAnimationYKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            this.positionAnimationYKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            this.positionAnimationYKeyFrame.Value = 0;
            positionYAnimation.KeyFrames.Add(this.positionAnimationYKeyFrame);

            positionXAnimation.FillBehavior = FillBehavior.Stop;
            positionYAnimation.FillBehavior = FillBehavior.Stop;
            widthAnimation.FillBehavior = FillBehavior.Stop;
            heightAnimation.FillBehavior = FillBehavior.Stop;

            this.positionAnimation.Children.Add(positionXAnimation);
            this.positionAnimation.Children.Add(positionYAnimation);

            this.positionAnimation.Completed += new EventHandler(this.PositionAnimation_Completed);
        }

        #region Public members
        /// <summary>
        /// Gets or sets the size animation duration.
        /// </summary>
        [System.ComponentModel.Category("Animation Properties"), System.ComponentModel.Description("The size animation duration.")]
        public TimeSpan SizeAnimationDuration
        {
            get
            {
                return this.sizeAnimationTimespan;
            }

            set
            {
                this.sizeAnimationTimespan = value;
                if (this.sizeAnimationWidthKeyFrame != null)
                {
                    this.sizeAnimationWidthKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.sizeAnimationTimespan);
                }

                if (this.sizeAnimationHeightKeyFrame != null)
                {
                    this.sizeAnimationHeightKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.sizeAnimationTimespan);
                }
            }
        }

        /// <summary>
        /// Gets or sets the position animation duration.
        /// </summary>
        [System.ComponentModel.Category("Animation Properties"), System.ComponentModel.Description("The position animation duration.")]
        public TimeSpan PositionAnimationDuration
        {
            get
            {
                return this.positionAnimationTimespan;
            }

            set
            {
                this.positionAnimationTimespan = value;
                if (this.positionAnimationXKeyFrame != null)
                {
                    this.positionAnimationXKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.positionAnimationTimespan);
                }

                if (this.positionAnimationYKeyFrame != null)
                {
                    this.positionAnimationYKeyFrame.KeyTime = KeyTime.FromTimeSpan(this.positionAnimationTimespan);
                }
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Animates the size of the control
        /// </summary>
        /// <param name="width">The target width</param>
        /// <param name="height">The target height</param>
        public void AnimateSize(double width, double height)
        {
            if (this.sizeAnimating)
            {
                this.sizeAnimation.Pause();
            }

            // Ensure we are in the tree
            if (VisualTreeHelper.GetParent(this) != null)
            {
                this.Width = this.ActualWidth;
                this.Height = this.ActualHeight;
                this.sizeAnimating = true;
                this.sizeAnimationWidthKeyFrame.Value = width;
                this.sizeAnimationHeightKeyFrame.Value = height;
                this.sizeAnimation.Begin();
            }
        }

        /// <summary>
        /// Animates the Canvas.Left and Canvas.Top of the control
        /// </summary>
        /// <param name="x">New X position</param>
        /// <param name="y">New Y position</param>
        public void AnimatePosition(double x, double y)
        {
            if (this.positionAnimating)
            {
                this.positionAnimation.Pause();
            }

            // Ensure we are in the tree
            if (VisualTreeHelper.GetParent(this) != null)
            {
                this.positionAnimating = true;
                this.positionAnimationXKeyFrame.Value = x;
                this.positionAnimationYKeyFrame.Value = y;
                this.positionAnimation.Begin();
            }
        }
        #endregion

        /// <summary>
        /// Stores the position
        /// </summary>
        /// <param name="sender">The position animation.</param>
        /// <param name="e">Event args.</param>
        private void PositionAnimation_Completed(object sender, EventArgs e)
        {
            Canvas.SetLeft(this, this.positionAnimationXKeyFrame.Value);
            Canvas.SetTop(this, this.positionAnimationYKeyFrame.Value);
        }

        /// <summary>
        /// Stores the values once the animation has completed.
        /// </summary>
        /// <param name="sender">The animated content control.</param>
        /// <param name="e">The event args.</param>
        private void SizeAnimation_Completed(object sender, EventArgs e)
        {
            this.Width = this.sizeAnimationWidthKeyFrame.Value;
            this.Height = this.sizeAnimationHeightKeyFrame.Value;
        }
    }
}
