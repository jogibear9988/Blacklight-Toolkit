//-----------------------------------------------------------------------
// <copyright file="SyndicationFeedListBoxSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>06-Mar-2009</date>
// <author>Martin Grayson</author>
// <summary>The Synidcation Feed List Box sample.</summary>
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
    /// The Synidcation Feed List Box sample.
    /// </summary>
    public partial class SyndicationFeedListBoxSample : UserControl
    {
        /// <summary>
        /// SyndicationFeedListBoxSample constructor.
        /// </summary>
        public SyndicationFeedListBoxSample()
        {
            InitializeComponent();
            this.feedKindComboBox.Items.Add("Absolute");
            this.feedKindComboBox.Items.Add("Relative");
            this.feedKindComboBox.SelectedIndex = 1;
            this.GoButton.Click += new RoutedEventHandler(this.GoButton_Click);
        }

        /// <summary>
        /// Refreshes the feed.
        /// </summary>
        /// <param name="sender">The go button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            this.SyndicationFeedListBox.FeedUriKind = this.GetUriKind();
            this.SyndicationFeedListBox.FeedUri = this.feedUrl.Text;
            this.SyndicationFeedListBox.Refresh();
        }

        /// <summary>
        /// Gets the Uri kind.
        /// </summary>
        /// <returns>The selected uri kind.</returns>
        private UriKind GetUriKind()
        {
            switch (this.feedKindComboBox.SelectedItem.ToString())
            {
                case "Absolute":
                    return UriKind.Absolute;
                case "Relative":
                    return UriKind.Relative;
                default:
                    return UriKind.RelativeOrAbsolute;
            }
        }
    }
}
