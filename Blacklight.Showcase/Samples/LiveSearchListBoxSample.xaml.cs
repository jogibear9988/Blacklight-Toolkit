//-----------------------------------------------------------------------
// <copyright file="LiveSearchListBoxSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>06-Mar-2009</date>
// <author>Martin Grayson</author>
// <summary>The Live Search List Box sample.</summary>
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
    /// The Live Search List Box sample.
    /// </summary>
    public partial class LiveSearchListBoxSample : UserControl
    {
        /// <summary>
        /// LiveSearchListBoxSample constructor.
        /// </summary>
        public LiveSearchListBoxSample()
        {
            InitializeComponent();
            this.searchTypeComboBox.Items.Add("Web");
            this.searchTypeComboBox.Items.Add("Image");
            this.searchTypeComboBox.Items.Add("News");
            this.searchTypeComboBox.SelectedIndex = 0;
            this.searchTypeComboBox.SelectionChanged += new SelectionChangedEventHandler(this.SearchTypeComboBox_SelectionChanged);
        }

        /// <summary>
        /// Updates the search type.
        /// </summary>
        /// <param name="sender">The combo box.</param>
        /// <param name="e">Selection Changed Event Args.</param>
        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LiveSearchListBox.ItemsSource = null; 

            switch (this.searchTypeComboBox.SelectedItem.ToString())
            {
                case "Web":
                    this.LiveSearchListBox.LiveSearchType = Blacklight.Controls.LiveSearchService.SourceType.Web;
                    this.LiveSearchListBox.ItemTemplate = this.Resources["WebTemplate"] as DataTemplate;
                    break;
                case "Image":
                    this.LiveSearchListBox.LiveSearchType = Blacklight.Controls.LiveSearchService.SourceType.Image;
                    this.LiveSearchListBox.ItemTemplate = this.Resources["ImageTemplate"] as DataTemplate;
                    break;
                case "News":
                    this.LiveSearchListBox.LiveSearchType = Blacklight.Controls.LiveSearchService.SourceType.News;
                    this.LiveSearchListBox.ItemTemplate = this.Resources["NewsTemplate"] as DataTemplate;
                    break;
            }

            this.LiveSearchListBox.Refresh();
        }
    }
}
