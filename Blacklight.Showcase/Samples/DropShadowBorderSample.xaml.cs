//-----------------------------------------------------------------------
// <copyright file="DropShadowBorderSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>17-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>The drop shadow border sample.</summary>
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
    /// The drop shadow border sample.
    /// </summary>
    public partial class DropShadowBorderSample : UserControl
    {
        /// <summary>
        /// DropShadowBorderSample constructor.
        /// </summary>
        public DropShadowBorderSample()
        {
            InitializeComponent();

            this.opacitySlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.OpacitySlider_ValueChanged);
            this.angleSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.AngleSlider_ValueChanged);
            this.distanceSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.DistanceSlider_ValueChanged);
            this.spreadSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.SpreadSlider_ValueChanged);

            this.dropShadowColorATextBox.TextChanged += new TextChangedEventHandler(this.DropShadowColorTextBox_TextChanged);
            this.dropShadowColorRTextBox.TextChanged += new TextChangedEventHandler(this.DropShadowColorTextBox_TextChanged);
            this.dropShadowColorGTextBox.TextChanged += new TextChangedEventHandler(this.DropShadowColorTextBox_TextChanged);
            this.dropShadowColorBTextBox.TextChanged += new TextChangedEventHandler(this.DropShadowColorTextBox_TextChanged);
        }

        /// <summary>
        /// Updates the drop shadow spread.
        /// </summary>
        /// <param name="sender">The spread slider.</param>
        /// <param name="e">Routed Property Changed Event Args.</param>
        private void SpreadSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.dropShadowBorder.DropShadowSpread = e.NewValue;
        }

        /// <summary>
        /// Updates the drop shadow color.
        /// </summary>
        /// <param name="sender">The drop shadow color color text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void DropShadowColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte colorA = 255;
            byte.TryParse(this.dropShadowColorATextBox.Text, out colorA);

            byte colorR = 0;
            byte.TryParse(this.dropShadowColorRTextBox.Text, out colorR);

            byte colorG = 0;
            byte.TryParse(this.dropShadowColorGTextBox.Text, out colorG);

            byte colorB = 0;
            byte.TryParse(this.dropShadowColorBTextBox.Text, out colorB);

            this.dropShadowBorder.DropShadowColor = Color.FromArgb(colorA, colorR, colorG, colorB);
        }

        /// <summary>
        /// Updates the drop shadow angle.
        /// </summary>
        /// <param name="sender">The drop shadow angle slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void AngleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.dropShadowBorder.DropShadowAngle = e.NewValue;
        }

        /// <summary>
        /// Updates the drop shadow distance.
        /// </summary>
        /// <param name="sender">The drop shadow distance slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void DistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.dropShadowBorder.DropShadowDistance = e.NewValue;
        }

        /// <summary>
        /// Updates the drop shadow opacity.
        /// </summary>
        /// <param name="sender">The drop shadow opacity slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.dropShadowBorder.DropShadowOpacity = e.NewValue;
        }
    }
}
