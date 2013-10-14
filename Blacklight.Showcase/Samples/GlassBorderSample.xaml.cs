//-----------------------------------------------------------------------
// <copyright file="GlassBorderSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>03-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>The glassborder sample.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Showcase.Samples
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
    public partial class GlassBorderSample : UserControl
    {
        /// <summary>
        /// InnerGlowBorderSample constructor.
        /// </summary>
        public GlassBorderSample()
        {
            InitializeComponent();

            this.opacitySlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.OpacitySlider_ValueChanged);

            this.cornerRadiusTopLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusTopRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);

            this.colorATextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorRTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorGTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorBTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);

            this.contentZIndex.TextChanged += new TextChangedEventHandler(this.ContentZIndex_TextChanged);
        }

        /// <summary>
        /// Updates the content z-index.
        /// </summary>
        /// <param name="sender">The content z-index text box.</param>
        /// <param name="e">Text Changed Event Args.</param>
        private void ContentZIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            int contentZIndexValue = 0;
            int.TryParse(this.contentZIndex.Text, out contentZIndexValue);

            this.glassBorder.ContentZIndex = contentZIndexValue;
        }

        /// <summary>
        /// Updates the outer glow color.
        /// </summary>
        /// <param name="sender">The outer glow color text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void ColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte colorA = 255;
            byte.TryParse(this.colorATextBox.Text, out colorA);

            byte colorR = 0;
            byte.TryParse(this.colorRTextBox.Text, out colorR);

            byte colorG = 0;
            byte.TryParse(this.colorGTextBox.Text, out colorG);

            byte colorB = 0;
            byte.TryParse(this.colorBTextBox.Text, out colorB);

            this.glassBorder.Background = new SolidColorBrush(Color.FromArgb(colorA, colorR, colorG, colorB));
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

            this.glassBorder.CornerRadius = new CornerRadius(
                Math.Max(0, topLeftCornerRadius), 
                Math.Max(0, topRightCornerRadius), 
                Math.Max(0, bottomRightCornerRadius), 
                Math.Max(0, bottomLeftCornerRadius));
        }

        /// <summary>
        /// Changes the opacity of the border.
        /// </summary>
        /// <param name="sender">The opacity slider.</param>
        /// <param name="e">Routed property changed event args.</param>
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.glassBorder.GlassOpacity = e.NewValue;
        }
    }
}
