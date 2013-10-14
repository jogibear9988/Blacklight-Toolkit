//-----------------------------------------------------------------------
// <copyright file="MediaPlayerSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>The media player sample.</summary>
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
    /// The media player sample.
    /// </summary>
    public partial class MediaPlayerSample : UserControl
    {
        /// <summary>
        /// Media player sample constructor.
        /// </summary>
        public MediaPlayerSample()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the media source.
        /// </summary>
        /// <value>The media source.</value>
        public string MediaSource
        {
            get
            {
                return this.MediaPlayer.MediaSource;
            }

            set
            {
                this.MediaPlayer.MediaSource = value;
            }
        }
    }
}
