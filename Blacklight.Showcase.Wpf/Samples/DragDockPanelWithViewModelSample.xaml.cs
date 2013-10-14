//-----------------------------------------------------------------------
// <copyright file="DragDockPanelSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>The drag dock panel sample.</summary>
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
using Blacklight.Controls;
using System.Collections.ObjectModel;

    public class Vm
    {
        public string DisplayName { get; set; }
        public object View { get; set; }
    }
    /// <summary>
    /// The drag dock panel sample.
    /// </summary>
    public partial class DragDockPanelWithViewModelSample : UserControl
    {
        /// <summary>
        /// Stores a collection of the panels.
        /// </summary>
        private ObservableCollection<Vm> panels = new ObservableCollection<Vm>();

        /// <summary>
        /// Drag dock panel sample constructor.
        /// </summary>
        public DragDockPanelWithViewModelSample()
        {
            this.InitializeComponent();

            dragDockPanelHostWithItemTemplate.ItemsSource = this.panels;

            for (int i = 0; i < 6; i++)
            {
                this.AddViewModel();
            }

            this.minimizedPositionComboBox.SelectionChanged += new SelectionChangedEventHandler(this.MinimizedPositionComboBox_SelectionChanged);
            this.addViewModelButton.Click += new RoutedEventHandler(this.AddViewModelButton_Click);
            this.removeViewModelButton.Click += new RoutedEventHandler(this.RemoveViewModelButton_Click);

            this.maxRows.TextChanged += new TextChangedEventHandler(this.MaxRows_TextChanged);
            this.maxColumns.TextChanged += new TextChangedEventHandler(this.MaxColumns_TextChanged);
        }

        /// <summary>
        /// Updates the max columns.
        /// </summary>
        /// <param name="sender">The max columns text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void MaxColumns_TextChanged(object sender, TextChangedEventArgs e)
        {
            int maxColumns = 0;
            int.TryParse(this.maxColumns.Text, out maxColumns);
            this.dragDockPanelHostWithItemTemplate.MaxColumns = maxColumns;
        }

        /// <summary>
        /// Updates the max rows.
        /// </summary>
        /// <param name="sender">The max rows text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void MaxRows_TextChanged(object sender, TextChangedEventArgs e)
        {
            int maxRows = 0;
            int.TryParse(this.maxRows.Text, out maxRows);
            this.dragDockPanelHostWithItemTemplate.MaxRows = maxRows;
        }

        /// <summary>
        /// Adds a viewmodel to the host.
        /// </summary>
        /// <param name="sender">The add button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void AddViewModelButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddViewModel();
        }

        /// <summary>
        /// REmoves a viewmodel from the host.
        /// </summary>
        /// <param name="sender">The add button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void RemoveViewModelButton_Click(object sender, RoutedEventArgs e)
        {
            this.RemoveViewModel();
        }


        /// <summary>
        /// Adds a ViewModel.
        /// </summary>
        private void AddViewModel()
        {
            //New ViewModel Support
            this.panels.Add(new Vm()
            {
                DisplayName = string.Format("{0} {1}", "Panel", this.dragDockPanelHostWithItemTemplate.Items.Count + 1),
                View = new TextBlock()
                {
                    Text = "C O N T E N T",
                    FontFamily = new FontFamily("Verdana"),
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(0x44, 255, 255, 255))
                },
            });
        }

        /// <summary>
        /// Removes a ViewModel.
        /// </summary>
        private void RemoveViewModel()
        {
            if (this.panels.Count > 0)
            {
                this.panels.RemoveAt(this.panels.Count - 1);
            }
        }

        /// <summary>
        /// Updates the minimised position.
        /// </summary>
        /// <param name="sender">The minimized combo box.</param>
        /// <param name="e">Selection changed event args.</param>
        private void MinimizedPositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.minimizedPositionComboBox.SelectedIndex)
            {
                case 0:
                    this.dragDockPanelHostWithItemTemplate.MinimizedPosition = Blacklight.Controls.Wpf.MinimizedPositions.Right;
                    break;
                case 1:
                    this.dragDockPanelHostWithItemTemplate.MinimizedPosition = Blacklight.Controls.Wpf.MinimizedPositions.Bottom;
                    break;
                case 2:
                    this.dragDockPanelHostWithItemTemplate.MinimizedPosition = Blacklight.Controls.Wpf.MinimizedPositions.Left;
                    break;
                case 3:
                    this.dragDockPanelHostWithItemTemplate.MinimizedPosition = Blacklight.Controls.Wpf.MinimizedPositions.Top;
                    break;
            }
        }
    }
}
