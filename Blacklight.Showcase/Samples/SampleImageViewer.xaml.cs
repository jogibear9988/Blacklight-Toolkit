//-----------------------------------------------------------------------
// <copyright file="SampleImageViewer.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>30-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>Sample image viewer.</summary>
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
    /// Sample image viewer.
    /// </summary>
    public partial class SampleImageViewer : UserControl
    {
        /// <summary>
        /// Sample image viewer constructor.
        /// </summary>
        public SampleImageViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets sets the image source.
        /// </summary>
        public ImageSource Source 
        {
            get
            {
                return this.image.Source;
            }

            set
            {
                this.image.Source = value;
            }
        }
    }
}
