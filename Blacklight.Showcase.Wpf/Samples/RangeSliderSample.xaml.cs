//-----------------------------------------------------------------------
// <copyright file="RangeSliderSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>12-July-2009</date>
// <author>Martin Grayson</author>
// <summary>The range slider sample.</summary>
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
    /// The range slider sample.
    /// </summary>
    public partial class RangeSliderSample : UserControl
    {
        /// <summary>
        /// RangeSliderSample constructor.
        /// </summary>
        public RangeSliderSample()
        {
            InitializeComponent();
            this.minimum.TextChanged += new TextChangedEventHandler(this.Minimum_TextChanged);
            this.maximum.TextChanged += new TextChangedEventHandler(this.Maximum_TextChanged);
            this.minimumRangeSpan.TextChanged += new TextChangedEventHandler(this.MinimumRangeSpan_TextChanged);
        }

        /// <summary>
        /// Updates the range slider properties.
        /// </summary>
        /// <param name="sender">The source text box.</param>
        /// <param name="e">Text Changed Event Args.</param>
        private void MinimumRangeSpan_TextChanged(object sender, TextChangedEventArgs e)
        {
            double value = 0;
            double.TryParse(this.minimumRangeSpan.Text, out value);
            this.rangeSlider.MinimumRangeSpan = value;
        }

        /// <summary>
        /// Updates the range slider properties.
        /// </summary>
        /// <param name="sender">The source text box.</param>
        /// <param name="e">Text Changed Event Args.</param>
        private void Maximum_TextChanged(object sender, TextChangedEventArgs e)
        {
            double value = 0;
            double.TryParse(this.maximum.Text, out value);
            this.rangeSlider.Maximum = value;
        }

        /// <summary>
        /// Updates the range slider properties.
        /// </summary>
        /// <param name="sender">The source text box.</param>
        /// <param name="e">Text Changed Event Args.</param>
        private void Minimum_TextChanged(object sender, TextChangedEventArgs e)
        {
            double value = 0;
            double.TryParse(this.minimum.Text, out value);
            this.rangeSlider.Minimum = value;
        }
    }
}
