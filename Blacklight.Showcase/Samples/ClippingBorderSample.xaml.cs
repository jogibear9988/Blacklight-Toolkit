//-----------------------------------------------------------------------
// <copyright file="ClippingBorderSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>08-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>The clipping border sample.</summary>
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
    /// The clipping border sample.
    /// </summary>
    public partial class ClippingBorderSample : UserControl
    {
        /// <summary>
        /// ClippingBorderSample constructor.
        /// </summary>
        public ClippingBorderSample()
        {
            InitializeComponent();

            this.clipContentCheckBox.Checked += new RoutedEventHandler(this.ClipContentCheckBox_CheckedChanged);
            this.clipContentCheckBox.Unchecked += new RoutedEventHandler(this.ClipContentCheckBox_CheckedChanged);

            this.borderThicknessLeftTextBox.TextChanged += new TextChangedEventHandler(this.BorderThicknessTextBox_TextChanged);
            this.borderThicknessTopTextBox.TextChanged += new TextChangedEventHandler(this.BorderThicknessTextBox_TextChanged);
            this.borderThicknessRightTextBox.TextChanged += new TextChangedEventHandler(this.BorderThicknessTextBox_TextChanged);
            this.borderThicknessBottomTextBox.TextChanged += new TextChangedEventHandler(this.BorderThicknessTextBox_TextChanged);

            this.cornerRadiusTopLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusTopRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomRightTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
            this.cornerRadiusBottomLeftTextBox.TextChanged += new TextChangedEventHandler(this.CornerRadiusTextBox_TextChanged);
        }

        /// <summary>
        /// Clips or unclips the content.
        /// </summary>
        /// <param name="sender">The clipping checkbox.</param>
        /// <param name="e">Routed event args.</param>
        private void ClipContentCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.clippingBorder.ClipContent = this.clipContentCheckBox.IsChecked.Value;
        }

        /// <summary>
        /// Updates the border thickness.
        /// </summary>
        /// <param name="sender">The border thickness text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void BorderThicknessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double leftBorderThickness = 0;
            double.TryParse(this.borderThicknessLeftTextBox.Text, out leftBorderThickness);

            double topBorderThickness = 0;
            double.TryParse(this.borderThicknessTopTextBox.Text, out topBorderThickness);

            double rightBorderThickness = 0;
            double.TryParse(this.borderThicknessRightTextBox.Text, out rightBorderThickness);

            double bottomBorderThickness = 0;
            double.TryParse(this.borderThicknessBottomTextBox.Text, out bottomBorderThickness);

            this.clippingBorder.BorderThickness = new Thickness(leftBorderThickness, topBorderThickness, rightBorderThickness, bottomBorderThickness);
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

            this.clippingBorder.CornerRadius = new CornerRadius(topLeftCornerRadius, topRightCornerRadius, bottomRightCornerRadius, bottomLeftCornerRadius);
        }
    }
}
