//-----------------------------------------------------------------------
// <copyright file="WatermarkedTextBoxSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>18-July-2009</date>
// <author>Martin Grayson</author>
// <summary>The watermarked text box sample.</summary>
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
    /// The watermarked text box sample.
    /// </summary>
    public partial class WatermarkedTextBoxSample : UserControl
    {
        /// <summary>
        /// WatermarkedTextBoxSample constructor.
        /// </summary>
        public WatermarkedTextBoxSample()
        {
            InitializeComponent();

            this.watermarkTypeComboBox.Items.Add("Text");
            this.watermarkTypeComboBox.Items.Add("Image");
            this.watermarkTypeComboBox.Items.Add("UIElement");
            this.watermarkTypeComboBox.SelectedIndex = 0;
            this.watermarkTypeComboBox.SelectionChanged += new SelectionChangedEventHandler(this.WatermarkTypeComboBox_SelectionChanged);
        }

        /// <summary>
        /// Updates the watermark example.
        /// </summary>
        /// <param name="sender">The watermark type combo box.</param>
        /// <param name="e">Selection Changed Event Args.</param>
        private void WatermarkTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.watermarkTypeComboBox.SelectedItem.ToString())
            {
                case "Text":
                    this.watermarkedTextBox.WatermarkTemplate = this.Resources["TextWatermark"] as DataTemplate;
                    break;
                case "Image":
                    this.watermarkedTextBox.WatermarkTemplate = this.Resources["ImageWatermark"] as DataTemplate;
                    break;
                case "UIElement":
                    this.watermarkedTextBox.WatermarkTemplate = this.Resources["UIElementWatermark"] as DataTemplate;
                    break;
            }
        }
    }
}
