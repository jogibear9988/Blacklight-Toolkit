//-----------------------------------------------------------------------
// <copyright file="StrokeTextBlockSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>24-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>The stroke text block sample.</summary>
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
    /// The stroke text block sample.
    /// </summary>
    public partial class StrokeTextBlockSample : UserControl
    {
        /// <summary>
        /// DropShadowTextBlockSample constructor.
        /// </summary>
        public StrokeTextBlockSample()
        {
            InitializeComponent();

            this.opacitySlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.OpacitySlider_ValueChanged);
            this.thicknessSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.ThicknessSlider_ValueChanged);
            this.fontSizeSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.FontSizeSlider_ValueChanged);
            this.text.TextChanged += new TextChangedEventHandler(this.Text_TextChanged);

            this.colorATextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorRTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorGTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
            this.colorBTextBox.TextChanged += new TextChangedEventHandler(this.ColorTextBox_TextChanged);
        }

        /// <summary>
        /// Updates the font size.
        /// </summary>
        /// <param name="sender">The font size slider.</param>
        /// <param name="e">Routed Property Changed Event Args.</param>
        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.strokeTextBlock.FontSize = e.NewValue;
        }

        /// <summary>
        /// Updates the color.
        /// </summary>
        /// <param name="sender">The color text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void ColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte colorA = 255;
            byte.TryParse(this.colorATextBox.Text, out colorA);

            byte colorR = 255;
            byte.TryParse(this.colorRTextBox.Text, out colorR);

            byte colorG = 255;
            byte.TryParse(this.colorGTextBox.Text, out colorG);

            byte colorB = 255;
            byte.TryParse(this.colorBTextBox.Text, out colorB);

            this.strokeTextBlock.Stroke = new SolidColorBrush(Color.FromArgb(colorA, colorR, colorG, colorB));
        }

        /// <summary>
        /// Updates the text.
        /// </summary>
        /// <param name="sender">The drop shadow text text box.</param>
        /// <param name="e">Text Changed Event Args</param>
        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.strokeTextBlock.Text = this.text.Text;
        }

        /// <summary>
        /// Updates the stroke thickness.
        /// </summary>
        /// <param name="sender">The drop shadow distance slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.strokeTextBlock.StrokeThickness = e.NewValue;
        }

        /// <summary>
        /// Updates the stroke opacity.
        /// </summary>
        /// <param name="sender">The drop shadow opacity slider.</param>
        /// <param name="e">Routed Property Changed Event Args</param>
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.strokeTextBlock.StrokeOpacity = e.NewValue;
        }
    }
}
