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
    using Blacklight.Controls;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The drag dock panel sample.
    /// </summary>
    public partial class DragDockPanelSample : UserControl
    {
        /// <summary>
        /// Stores a collection of the panels.
        /// </summary>
        private ObservableCollection<DragDockPanel> panels = new ObservableCollection<DragDockPanel>();

        /// <summary>
        /// Drag dock panel sample constructor.
        /// </summary>
        public DragDockPanelSample()
        {
            this.InitializeComponent();

            dragDockPanelHost.ItemsSource = this.panels;

            for (int i = 0; i < 6; i++)
            {
                this.AddPanel();
            }

            this.minimizedPositionComboBox.SelectionChanged += new SelectionChangedEventHandler(this.MinimizedPositionComboBox_SelectionChanged);
            this.addPanelButton.Click += new RoutedEventHandler(this.AddPanelButton_Click);
            this.removePanelButton.Click += new RoutedEventHandler(this.RemovePanelButton_Click);

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
            this.dragDockPanelHost.MaxColumns = maxColumns;
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
            this.dragDockPanelHost.MaxRows = maxRows;
        }

        /// <summary>
        /// Adds a panel to the host.
        /// </summary>
        /// <param name="sender">The add button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void AddPanelButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddPanel();
        }

        /// <summary>
        /// Adds a panel.
        /// </summary>
        private void AddPanel()
        {
            // Classic Panel adding
            this.panels.Add(new Blacklight.Controls.DragDockPanel()
            {
                Margin = new Thickness(15),
                Header = string.Format("{0} {1}", "Panel", this.dragDockPanelHost.Items.Count + 1),
                Content = new TextBlock()
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
        /// Removes a panel from the host.
        /// </summary>
        /// <param name="sender">Remove panel button.</param>
        /// <param name="e">Routed event args.</param>
        private void RemovePanelButton_Click(object sender, RoutedEventArgs e)
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
                    this.dragDockPanelHost.MinimizedPosition = Blacklight.Controls.MinimizedPositions.Right;
                    break;
                case 1:
                    this.dragDockPanelHost.MinimizedPosition = Blacklight.Controls.MinimizedPositions.Bottom;
                    break;
                case 2:
                    this.dragDockPanelHost.MinimizedPosition = Blacklight.Controls.MinimizedPositions.Left;
                    break;
                case 3:
                    this.dragDockPanelHost.MinimizedPosition = Blacklight.Controls.MinimizedPositions.Top;
                    break;
            }
        }
    }
}
