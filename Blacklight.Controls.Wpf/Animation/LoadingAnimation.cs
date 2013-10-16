//-----------------------------------------------------------------------
// <copyright file="LoadingAnimation.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>24-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>A control that shows a loading animation.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
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
    /// A control that shows a loading animation.
    /// </summary>
    public class LoadingAnimation : ContentControl
    {
        /// <summary>
        /// Ellipse fill property.
        /// </summary>
        public static readonly DependencyProperty EllipseFillProperty =
            DependencyProperty.Register("EllipseFill", typeof(Brush), typeof(LoadingAnimation), null);

        /// <summary>
        /// Ellipse stroke property.
        /// </summary>
        public static readonly DependencyProperty EllipseStrokeProperty =
            DependencyProperty.Register("EllipseStroke", typeof(Brush), typeof(LoadingAnimation), null);

        /// <summary>
        /// Stores the loading animation storyboard.
        /// </summary>
        private Storyboard loadingAnimation;

        /// <summary>
        /// Stores whether the animation is running.
        /// </summary>
        private AnimationState animationState;

        /// <summary>
        /// Stores whether the animation should play on load.
        /// </summary>
        private bool autoPlay;

        /// <summary>
        /// LoadingAnimation constructor.
        /// </summary>
        public LoadingAnimation()
        {
            this.DefaultStyleKey = typeof(LoadingAnimation);
        }

        /// <summary>
        /// Gets or sets the ellipse fill.
        /// </summary>
        [System.ComponentModel.Category("Loading Animation Properties"), System.ComponentModel.Description("The fill for the little ellipses.")]
        public Brush EllipseFill
        {
            get { return (Brush)GetValue(EllipseFillProperty); }
            set { SetValue(EllipseFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ellipse stroke.
        /// </summary>
        [System.ComponentModel.Category("Loading Animation Properties"), System.ComponentModel.Description("The stroke for the little ellipses.")]
        public Brush EllipseStroke
        {
            get { return (Brush)GetValue(EllipseStrokeProperty); }
            set { SetValue(EllipseStrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation should play on load.
        /// </summary>
        [System.ComponentModel.Category("Loading Animation Properties"), System.ComponentModel.Description("Whether the animation auto plays.")]
        public bool AutoPlay
        {
            get 
            { 
                return this.autoPlay; 
            }

            set 
            { 
                this.autoPlay = value;

                if (this.loadingAnimation != null)
                {
                    this.loadingAnimation.Stop();
                    if (this.autoPlay)
                    {
                        this.loadingAnimation.Begin();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the animation state,
        /// </summary>
        public AnimationState AnimationState
        {
            get { return this.animationState; }
        }

        /// <summary>
        /// Gets the parts out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.loadingAnimation = (Storyboard)this.GetTemplateChild("PART_LoadingAnimation");

            if (this.loadingAnimation != null && this.autoPlay)
            {
                this.loadingAnimation.Begin();
            }
        }

        /// <summary>
        /// Begins the loading animation.
        /// </summary>
        public void Begin()
        {
            if (this.loadingAnimation != null)
            {
                this.animationState = AnimationState.Playing;
                this.loadingAnimation.Begin();
            }
        }

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public void Pause()
        {
            if (this.loadingAnimation != null)
            {
                this.animationState = AnimationState.Paused;
                this.loadingAnimation.Pause();
            }
        }

        /// <summary>
        /// Resumes the animation.
        /// </summary>
        public void Resume()
        {
            if (this.loadingAnimation != null)
            {
                this.animationState = AnimationState.Playing;
                this.loadingAnimation.Resume();
            }
        }

        /// <summary>
        /// Stops the animation.
        /// </summary>
        public void Stop()
        {
            if (this.loadingAnimation != null)
            {
                this.animationState = AnimationState.Stopped;
                this.loadingAnimation.Stop();
            }
        }
    }
}
