//-----------------------------------------------------------------------
// <copyright file="InnerGlowBorderSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>03-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>The inner glow border sample.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Showcase.Wpf.Samples
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

    /// <summary>
    /// The inner glow border sample.
    /// </summary>
    public partial class InnerGlowBorderSample : UserControl
    {
        /// <summary>
        /// InnerGlowBorderSample constructor.
        /// </summary>
        public InnerGlowBorderSample()
        {
            InitializeComponent();

            this.opacitySlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.OpacitySlider_ValueChanged);

            this.glowSizeLeftTextBox.TextChanged += new TextChangedEventHandler(this.GlowSizeTextBox_TextChanged);
            this.glowSizeTopTextBox.TextChanged += new TextChangedEventHandler(this.GlowSizeTextBox_TextChanged);
            this.glowSizeRightTextBox.TextChanged += new TextChangedEventHandler(this.GlowSizeTextBox_TextChanged);
            this.glowSizeBottomTextBox.TextChanged += new TextChangedEventHandler(this.GlowSizeTextBox_TextChanged);

            this.cornerRadiusTopLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusTopRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);

            this.innerGlowColorATextBox.TextChanged += new TextChangedEventHandler(this.InnerGlowColorTextBox_TextChanged);
            this.innerGlowColorRTextBox.TextChanged += new TextChangedEventHandler(this.InnerGlowColorTextBox_TextChanged);
            this.innerGlowColorGTextBox.TextChanged += new TextChangedEventHandler(this.InnerGlowColorTextBox_TextChanged);
            this.innerGlowColorBTextBox.TextChanged += new TextChangedEventHandler(this.InnerGlowColorTextBox_TextChanged);
        }

        /// <summary>
        /// Updates the outer glow color.
        /// </summary>
        /// <param name="sender">The outer glow color text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void InnerGlowColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte colorA = 255;
            byte.TryParse(this.innerGlowColorATextBox.Text, out colorA);

            byte colorR = 0;
            byte.TryParse(this.innerGlowColorRTextBox.Text, out colorR);

            byte colorG = 0;
            byte.TryParse(this.innerGlowColorGTextBox.Text, out colorG);

            byte colorB = 0;
            byte.TryParse(this.innerGlowColorBTextBox.Text, out colorB);

            this.innerGlowBorder.InnerGlowColor = Color.FromArgb(colorA, colorR, colorG, colorB);
        }

        /// <summary>
        /// Updates the corner radius.
        /// </summary>
        /// <param name="sender">The corner radius text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void CornerRadiusTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double topLeftCornerRadius = 0;
            double.TryParse(this.cornerRadiusTopLeftTextBox.Text, out topLeftCornerRadius);

            double topRightCornerRadius = 0;
            double.TryParse(this.cornerRadiusTopRightTextBox.Text, out topRightCornerRadius);

            double bottomRightCornerRadius = 0;
            double.TryParse(this.cornerRadiusBottomRightTextBox.Text, out bottomRightCornerRadius);

            double bottomLeftCornerRadius = 0;
            double.TryParse(this.cornerRadiusBottomLeftTextBox.Text, out bottomLeftCornerRadius);

            this.innerGlowBorder.CornerRadius = new CornerRadius(
                Math.Max(0, topLeftCornerRadius),
                Math.Max(0, topRightCornerRadius),
                Math.Max(0, bottomRightCornerRadius),
                Math.Max(0, bottomLeftCornerRadius));
        }

        /// <summary>
        /// Updates the glow size.
        /// </summary>
        /// <param name="sender">The glow size text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void GlowSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double leftMargin = 0;
            double.TryParse(this.glowSizeLeftTextBox.Text, out leftMargin);

            double topMargin = 0;
            double.TryParse(this.glowSizeTopTextBox.Text, out topMargin);

            double rightMargin = 0;
            double.TryParse(this.glowSizeRightTextBox.Text, out rightMargin);

            double bottomMargin = 0;
            double.TryParse(this.glowSizeBottomTextBox.Text, out bottomMargin);

            this.innerGlowBorder.InnerGlowSize = new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);
        }

        /// <summary>
        /// Changes the opacity of the border.
        /// </summary>
        /// <param name="sender">The opacity slider.</param>
        /// <param name="e">Routed property changed event args.</param>
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.innerGlowBorder.InnerGlowOpacity = e.NewValue;
        }
    }
}
