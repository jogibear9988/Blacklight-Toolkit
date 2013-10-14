//-----------------------------------------------------------------------
// <copyright file="DeepZoomViewerSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>06-Mar-2009</date>
// <author>Martin Grayson</author>
// <summary>The Deep Zoom Viewer sample.</summary>
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
    /// The Deep Zoom Viewer sample.
    /// </summary>
    public partial class DeepZoomViewerSample : UserControl
    {
        /// <summary>
        /// DeepZoomViewerSample constructor,
        /// </summary>
        public DeepZoomViewerSample()
        {
            InitializeComponent();
            this.GoButton.Click += new RoutedEventHandler(this.GoButton_Click);
        }

        /// <summary>
        /// Changes the deep zoom uri.
        /// </summary>
        /// <param name="sender">The go button.</param>
        /// <param name="e">Route Event Args.</param>
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeepZoomViewer.SourceUri = new Uri(this.deepZoomSource.Text);
        }
    }
}
