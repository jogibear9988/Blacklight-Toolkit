//-----------------------------------------------------------------------
// <copyright file="RadialShadowBorderSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>17-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>The radial shadow border sample.</summary>
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
    /// The radial shadow border sample.
    /// </summary>
    public partial class RadialShadowBorderSample : UserControl
    {
        /// <summary>
        /// RadialShadowBorderSample constructor.
        /// </summary>
        public RadialShadowBorderSample()
        {
            InitializeComponent();

            this.opacitySlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.OpacitySlider_ValueChanged);
            this.widthSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.WidthSlider_ValueChanged);
            this.verticalOffsetSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.VerticalOffsetSlider_ValueChanged);
            this.spreadSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.SpreadSlider_ValueChanged);

            this.colorATextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorRTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorGTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorBTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
        }

        /// <summary>
        /// Updates the drop shadow spread.
        /// </summary>
        /// <param name="sender">The spread slider.</param>
        /// <param name="e">Routed Property Changed Event Args.</param>
        private void SpreadSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.radialShadowBorder.RadialShadowSpread = e.NewValue;
        }

        /// <summary>
        /// Updates the drop shadow color.
        /// </summary>
        /// <param name="sender">The drop shadow color color text box.</param>
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

            this.radialShadowBorder.RadialShadowColor = Color.FromArgb(colorA, colorR, colorG, colorB);
        }

        /// <summary>
        /// Updates the radial shadow width.
        /// </summary>
        /// <param name="sender">The drop shadow angle slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.radialShadowBorder.RadialShadowWidth = e.NewValue;
        }

        /// <summary>
        /// Updates the radial shadow vertical offset.
        /// </summary>
        /// <param name="sender">The drop shadow distance slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void VerticalOffsetSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.radialShadowBorder.RadialShadowVerticalOffset = e.NewValue;
        }

        /// <summary>
        /// Updates the radial shadow opacity.
        /// </summary>
        /// <param name="sender">The drop shadow opacity slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.radialShadowBorder.RadialShadowOpacity = e.NewValue;
        }
    }
}
