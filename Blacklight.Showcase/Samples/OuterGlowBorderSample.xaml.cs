//-----------------------------------------------------------------------
// <copyright file="OuterGlowBorderSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>16-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>The outer glow border sample.</summary>
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
    /// The outer glow border sample.
    /// </summary>
    public partial class OuterGlowBorderSample : UserControl
    {
        /// <summary>
        /// Outer glow border sample constructor.
        /// </summary>
        public OuterGlowBorderSample()
        {
            InitializeComponent();

            this.opacitySlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.OpacitySlider_ValueChanged);
            this.sizeSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.SizeSlider_ValueChanged);

            this.cornerRadiusTopLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusTopRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);

            this.outerGlowColorATextBox.TextChanged += new TextChangedEventHandler(this.OuterGlowColorTextBox_TextChanged);
            this.outerGlowColorRTextBox.TextChanged += new TextChangedEventHandler(this.OuterGlowColorTextBox_TextChanged);
            this.outerGlowColorGTextBox.TextChanged += new TextChangedEventHandler(this.OuterGlowColorTextBox_TextChanged);
            this.outerGlowColorBTextBox.TextChanged += new TextChangedEventHandler(this.OuterGlowColorTextBox_TextChanged);
        }

        /// <summary>
        /// Updates the glow size.
        /// </summary>
        /// <param name="sender">The size slider.</param>
        /// <param name="e">Routed Property Chnaged Event Args.</param>
        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.outerGlowBorder.OuterGlowSize = e.NewValue;
        }

        /// <summary>
        /// Updates the outer glow color.
        /// </summary>
        /// <param name="sender">The outer glow color text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void OuterGlowColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte colorA = 255;
            byte.TryParse(this.outerGlowColorATextBox.Text, out colorA);

            byte colorR = 0;
            byte.TryParse(this.outerGlowColorRTextBox.Text, out colorR);

            byte colorG = 0;
            byte.TryParse(this.outerGlowColorGTextBox.Text, out colorG);

            byte colorB = 0;
            byte.TryParse(this.outerGlowColorBTextBox.Text, out colorB);

            this.outerGlowBorder.OuterGlowColor = Color.FromArgb(colorA, colorR, colorG, colorB);
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

            this.outerGlowBorder.CornerRadius = new CornerRadius(
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
            this.outerGlowBorder.OuterGlowOpacity = e.NewValue;
        }
    }
}
