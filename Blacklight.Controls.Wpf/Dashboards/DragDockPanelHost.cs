//-----------------------------------------------------------------------
// <copyright file="DragDockPanelHost.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>A draggable, dockable, expandable host class.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls.Wpf
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Collections.Generic;

    /// <summary>
    /// The dock panel docking positions.
    /// </summary>
    public enum MinimizedPositions
    {
        /// <summary>
        /// Docks the panels along the top.
        /// </summary>
        Top,

        /// <summary>
        /// Docks the panels along the bottom.
        /// </summary>
        Bottom,

        /// <summary>
        /// Docks the panels down the left side.
        /// </summary>
        Left,

        /// <summary>
        /// Docks the panels down the rights side,
        /// </summary>
        Right
    }

    /// <summary>
    /// A draggable, dockable, expandable host class.
    /// </summary>
    [StyleTypedProperty(Property = "DefaultPanelStyle", StyleTargetType = typeof(DragDockPanel))]
    public class DragDockPanelHost : ItemsControl
    {
        /// <summary>
        /// The default panel style dependency protperty.
        /// </summary>
        public static readonly DependencyProperty DefaultPanelStyleProperty =
            DependencyProperty.Register("DefaultPanelStyle", typeof(Style), typeof(DragDockPanelHost), new PropertyMetadata(null));

        #region Private members
        /// <summary>
        /// A local store of the number of rows
        /// </summary>
        private int rows = 1;

        /// <summary>
        /// A local store of the number of columns
        /// </summary>
        private int columns = 1;

        /// <summary>
        /// Stores the max columns (0 for no maximum). Max rows takes priority over max columns.
        /// </summary>
        private int maxColumns = 0;

        /// <summary>
        /// Stores the max rows (0 for no maximum). Max rows takes priority over max columns.
        /// </summary>
        private int maxRows = 0;

        /// <summary>
        /// The panel currently being dragged
        /// </summary>
        private DragDockPanel draggingPanel = null;

        /// <summary>
        /// The currently maxmised panel
        /// </summary>
        private DragDockPanel maximizedPanel = null;

        /// <summary>
        /// Stores the minimized column width.
        /// </summary>
        private double minimizedColumnWidth = 250.0;

        /// <summary>
        /// Stores the minimized row height.
        /// </summary>
        private double minimizedRowHeight = 75.0;

        /// <summary>
        /// Stores the dockiing position.
        /// </summary>
        private MinimizedPositions minimizedPosition = MinimizedPositions.Right;

        /// <summary>
        /// Stores the panels in the control.
        /// </summary>
        private List<DragDockPanel> panels = new List<DragDockPanel>();

        #endregion

        #region Constructors
        /// <summary>
        /// DragDockPanelHost Constructor
        /// </summary>
        public DragDockPanelHost()
        {
            this.DefaultStyleKey = typeof(DragDockPanelHost);
            this.SizeChanged += new SizeChangedEventHandler(this.DragDockPanelHost_SizeChanged);
            this.LayoutUpdated += new EventHandler(this.DragDockPanelHost_LayoutUpdated);
        }
        #endregion

        #region Public members
        /// <summary>
        /// Gets or sets the default panel style.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Sets the default panel style.")]
        public Style DefaultPanelStyle
        {
            get { return (Style)GetValue(DefaultPanelStyleProperty); }
            set { SetValue(DefaultPanelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width for the minimzed column on the right or left side.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the minimized column width.")]
        public double MinimizedColumnWidth
        {
            get 
            { 
                return this.minimizedColumnWidth; 
            }

            set 
            { 
                this.minimizedColumnWidth = value;
                this.UpdatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the height for the minimized row on the top or bottom side.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the minimized row height.")]
        public double MinimizedRowHeight
        {
            get 
            { 
                return this.minimizedRowHeight; 
            }

            set 
            { 
                this.minimizedRowHeight = value;
                this.UpdatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the docking position.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the minimized panels' position.")]
        public MinimizedPositions MinimizedPosition
        {
            get
            {
                return this.minimizedPosition;
            }

            set
            {
                this.minimizedPosition = value;
                this.AnimatePanelSizes();
                this.AnimatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the max rows. 0 for no maximum. Max rows takes priority over max columns.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the maximum rows in the host. Max rows takes priority over max columns.")]
        public int MaxRows
        {
            get 
            { 
                return this.maxRows; 
            }

            set
            {
                this.maxRows = value;
                this.SetRowsAndColumns(this.GetOrderedPanels());
                this.AnimatePanelSizes();
                this.AnimatePanelLayout();
            }
        }

        /// <summary>
        /// Gets or sets the max columns. 0 for no maximum. Max rows takes priority over max columns.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Sets the maximum columns in the host. Max rows takes priority over max columns.")]
        public int MaxColumns
        {
            get
            {
                return this.maxColumns;
            }

            set
            {
                this.maxColumns = value;
                this.SetRowsAndColumns(this.GetOrderedPanels());
                this.AnimatePanelSizes();
                this.AnimatePanelLayout();
            }
        }
        #endregion

        /// <summary>
        /// Returns true is item is a DragDockPanel.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True is item is a DragDockPanel.</returns>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is DragDockPanel;
		}

        /// <summary>
        /// Returns a new drag dock panel.
        /// </summary>
        /// <returns>A new drag dock panel.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DragDockPanel();
        }

        /// <summary>
        /// Pepares a drag dock panel.
        /// </summary>
        /// <param name="element">The drag dock panel.</param>
        /// <param name="item">The source item.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            DragDockPanel panel = element as DragDockPanel;

            if (panel.Style == null && this.DefaultPanelStyle != null)
            {
                panel.Style = this.DefaultPanelStyle;
            }

            Dictionary<int, DragDockPanel> orderedPanels = this.GetOrderedPanels();
            orderedPanels.Add(this.panels.Count, panel);
            this.panels.Add(panel);
            this.PreparePanel(panel);
            this.SetRowsAndColumns(orderedPanels);
            this.AnimatePanelSizes();
            this.AnimatePanelLayout();
        }

        /// <summary>
        /// Unprepares a drag dock panel.
        /// </summary>
        /// <param name="element">The source drag dock panel.</param>
        /// <param name="item">The source item.</param>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            DragDockPanel panel = element as DragDockPanel;
            this.UnpreparePanel(panel);
            this.panels.Remove(panel);
            this.SetRowsAndColumns(this.GetOrderedPanels());
            this.AnimatePanelSizes();
            this.AnimatePanelLayout();
        }

        #region Panel management methods
        /// <summary>
        /// Prepares a panel for the UI. Override for hooking custom events.
        /// </summary>
        /// <param name="panel">The panel to prepare.</param>
        protected virtual void PreparePanel(DragDockPanel panel)
        {
            // Hook up panel events
            panel.DragStarted +=
                new DragEventHander(this.DragDockPanel_DragStarted);
            panel.DragFinished +=
                new DragEventHander(this.DragDockPanel_DragFinished);
            panel.DragMoved +=
                new DragEventHander(this.DragDockPanel_DragMoved);
            panel.Maximized +=
                new EventHandler(this.DragDockPanel_Maximized);
            panel.Restored +=
                new EventHandler(this.DragDockPanel_Restored);

            if (panel.PanelState == PanelState.Maximized)
            {
                this.maximizedPanel = panel;

                foreach (DragDockPanel dragDockPanel in this.panels)
                {
                    if (panel != dragDockPanel)
                    {
                        dragDockPanel.Minimize();
                    }
                }
            }
        }

        /// <summary>
        /// Unprepares a panel for the UI. Override for hooking custom events.
        /// </summary>
        /// <param name="panel">The panel to prepare.</param>
        protected virtual void UnpreparePanel(DragDockPanel panel)
        {
            if (panel.PanelState == PanelState.Maximized)
            {
                this.DragDockPanel_Restored(null, null);
            }

            // Hook up panel events
            panel.DragStarted -=
                new DragEventHander(this.DragDockPanel_DragStarted);
            panel.DragFinished -=
                new DragEventHander(this.DragDockPanel_DragFinished);
            panel.DragMoved -=
                new DragEventHander(this.DragDockPanel_DragMoved);
            panel.Maximized -=
                new EventHandler(this.DragDockPanel_Maximized);
            panel.Restored -=
                new EventHandler(this.DragDockPanel_Restored);
        }
        #endregion

        #region Panel layout methods
        /// <summary>
        /// Sets the rows and columns on an ordered list of panels.
        /// </summary>
        /// <param name="orderedPanels">The ordered panels.</param>
        private void SetRowsAndColumns(Dictionary<int, DragDockPanel> orderedPanels)
        {
            if (orderedPanels.Count == 0)
            {
                return;
            }

            // Calculate the number of rows and columns required
            this.rows =
                (int)Math.Floor(Math.Sqrt((double)this.panels.Count));

            if (this.maxRows > 0)
            {
                if (this.rows > this.maxRows)
                {
                    this.rows = this.maxRows;
                }

                this.columns =
                    (int)Math.Ceiling((double)this.panels.Count / (double)this.rows);
            }
            else if (this.maxColumns > 0)
            {
                this.columns =
                (int)Math.Ceiling((double)this.panels.Count / (double)this.rows);

                if (this.columns > this.maxColumns)
                {
                    this.columns = this.maxColumns;
                    this.rows = (int)Math.Ceiling((double)this.panels.Count / this.columns);
                }
            }
            else
            {
                this.columns =
                    (int)Math.Ceiling((double)this.panels.Count / (double)this.rows);
            }

            int childCount = 0;

            // Loop through the rows and columns and assign to children
            for (int r = 0; r < this.rows; r++)
            {
                for (int c = 0; c < this.columns; c++)
                {
                    Grid.SetRow(orderedPanels[childCount], r);
                    Grid.SetColumn(orderedPanels[childCount], c);
                    childCount++;

                    // if we are on the last child, break out of the loop
                    if (childCount == this.panels.Count)
                    {
                        break;
                    }
                }

                // if we are on the last child, break out of the loop
                if (childCount == this.panels.Count)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the panels in order.
        /// </summary>
        /// <returns>The ordered panels.</returns>
        private Dictionary<int, DragDockPanel> GetOrderedPanels()
        {
            Dictionary<int, DragDockPanel> orderedPanels = new Dictionary<int, DragDockPanel>();
            List<DragDockPanel> addedPanels = new List<DragDockPanel>();
            for (int i = 0; i < this.panels.Count; i++)
            {
                DragDockPanel lowestPanel = null;
                foreach (DragDockPanel panel in this.panels)
                {
                    if (!addedPanels.Contains(panel) && (lowestPanel == null || ((Grid.GetRow(panel) * this.columns) + Grid.GetColumn(panel) < (Grid.GetRow(lowestPanel) * this.columns) + Grid.GetColumn(lowestPanel))))
                    {
                        lowestPanel = panel;
                    }
                }

                addedPanels.Add(lowestPanel);
                orderedPanels.Add(i, lowestPanel);
            }

            return orderedPanels;
        }
        #endregion

        #region DragDockPanelHost events
        /// <summary>
        /// Updates the panel layouts.
        /// </summary>
        /// <param name="sender">The drag dock panel host.</param>
        /// <param name="e">Size changed event args.</param>
        private void DragDockPanelHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdatePanelLayout();
        }

        /// <summary>
        /// Updates the layout when in design mode.
        /// </summary>
        /// <param name="sender">The drag dock panel host.</param>
        /// <param name="e">Event Args.</param>
        private void DragDockPanelHost_LayoutUpdated(object sender, EventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Dictionary<int, DragDockPanel> orderedPanels = new Dictionary<int, DragDockPanel>();

                for (int i = 0; i < this.panels.Count; i++)
                {
                    if (this.panels[i].GetType() == typeof(DragDockPanel))
                    {
                        DragDockPanel panel = (DragDockPanel)this.panels[i];
                        orderedPanels.Add(i, panel);
                    }
                }

                this.SetRowsAndColumns(orderedPanels);
                this.UpdatePanelLayout();
            }
        }

        #endregion

        #region Panel dragging events
        /// <summary>
        /// Keeps a reference to the dragging panel.
        /// </summary>
        /// <param name="sender">The dragging panel.</param>
        /// <param name="args">Drag event args.</param>
        private void DragDockPanel_DragStarted(object sender, DragEventArgs args)
        {
            DragDockPanel panel = sender as DragDockPanel;

            // Keep reference to dragging panel
            this.draggingPanel = panel;
        }

        /// <summary>
        /// Shuffles the panels around.
        /// </summary>
        /// <param name="sender">The dragging panel.</param>
        /// <param name="args">Drag event args.</param>
        private void DragDockPanel_DragMoved(object sender, DragEventArgs args)
        {
            Point mousePosInHost =
                args.MouseEventArgs.GetPosition(this);

            int currentRow =
                (int)Math.Floor(mousePosInHost.Y /
                (this.ActualHeight / (double)this.rows));

            int currentColumn =
                (int)Math.Floor(mousePosInHost.X /
                (this.ActualWidth / (double)this.columns));

            // Stores the panel we will swap with
            DragDockPanel swapPanel = null;

            // Loop through children to see if there is a panel to swap with
            foreach (UIElement child in this.panels)
            {
                DragDockPanel panel = child as DragDockPanel;

                // If the panel is not the dragging panel and is in the current row
                // or current column... mark it as the panel to swap with
                if (panel != this.draggingPanel &&
                    Grid.GetColumn(panel) == currentColumn &&
                    Grid.GetRow(panel) == currentRow)
                {
                    swapPanel = panel;
                    break;
                }
            }

            // If there is a panel to swap with
            if (swapPanel != null)
            {
                // Store the new row and column
                int draggingPanelNewColumn = Grid.GetColumn(swapPanel);
                int draggingPanelNewRow = Grid.GetRow(swapPanel);

                // Update the swapping panel row and column
                Grid.SetColumn(swapPanel, Grid.GetColumn(this.draggingPanel));
                Grid.SetRow(swapPanel, Grid.GetRow(this.draggingPanel));

                // Update the dragging panel row and column
                Grid.SetColumn(this.draggingPanel, draggingPanelNewColumn);
                Grid.SetRow(this.draggingPanel, draggingPanelNewRow);

                // Animate the layout to the new positions
                this.AnimatePanelLayout();
            }
        }

        /// <summary>
        /// Drops the dragging panel.
        /// </summary>
        /// <param name="sender">The dragging panel.</param>
        /// <param name="args">Drag event args.</param>
        private void DragDockPanel_DragFinished(object sender, DragEventArgs args)
        {
            // Set dragging panel back to null
            this.draggingPanel = null;

            // Update the layout (to reset all panel positions)
            this.UpdatePanelLayout();
        }
        #endregion

        #region Panel maximized / minimized events
        /// <summary>
        /// Puts all of the panel back to a grid view.
        /// </summary>
        /// <param name="sender">The minimising panel.</param>
        /// <param name="e">Event args.</param>
        private void DragDockPanel_Restored(object sender, EventArgs e)
        {
            // Set max'ed panel to null
            this.maximizedPanel = null;

            // Loop through children to disable dragging
            foreach (UIElement child in this.panels)
            {
                DragDockPanel panel =
                    child as DragDockPanel;
                panel.Restore();
                panel.DraggingEnabled = true;
            }

            // Update sizes and layout
            this.AnimatePanelSizes();
            this.AnimatePanelLayout();
        }

        /// <summary>
        /// Maximises a panel.
        /// </summary>
        /// <param name="sender">the panel to maximise.</param>
        /// <param name="e">Event args.</param>
        private void DragDockPanel_Maximized(object sender, EventArgs e)
        {
            DragDockPanel maximizedPanel =
                sender as DragDockPanel;

            // Store max'ed panel
            this.maximizedPanel = maximizedPanel;

            // Loop through children to disable dragging
            foreach (UIElement child in this.panels)
            {
                DragDockPanel panel =
                    child as DragDockPanel;

                panel.DraggingEnabled = false;

                if (panel != this.maximizedPanel)
                {
                    panel.Minimize();
                }
            }

            // Update sizes and layout
            this.AnimatePanelSizes();
            this.AnimatePanelLayout();
        }
        #endregion

        #region Private layout methods
        /// <summary>
        /// Updates the panel layout without animation
        /// This does size and position without animation
        /// </summary>
        private void UpdatePanelLayout()
        {
            if (double.IsInfinity(this.ActualWidth) || double.IsNaN(this.ActualWidth) || this.ActualWidth == 0)
            {
                return;
            }

            // If we are not in max'ed panel mode...
            if (this.maximizedPanel == null)
            {
                // Layout children as per rows and columns
                foreach (UIElement child in this.panels)
                {
                    DragDockPanel panel = (DragDockPanel)child;

                    Canvas.SetLeft(
                        panel,
                        (Grid.GetColumn(panel) * (this.ActualWidth / (double)this.columns)));

                    Canvas.SetTop(
                        panel,
                        (Grid.GetRow(panel) * (this.ActualHeight / (double)this.rows)));

                    double width = (this.ActualWidth / (double)this.columns) - panel.Margin.Left - panel.Margin.Right;
                    double height = (this.ActualHeight / (double)this.rows) - panel.Margin.Top - panel.Margin.Bottom;

                    if (width < 0)
                    {
                        width = 0;
                    }

                    if (height < 0)
                    {
                        height = 0;
                    }

                    panel.Width = width;
                    panel.Height = height;
                }
            }
            else
            {
                Dictionary<int, DragDockPanel> orderedPanels = new Dictionary<int, DragDockPanel>();

                // Loop through children to order them according to their
                // current row and column...
                foreach (UIElement child in this.panels)
                {
                    DragDockPanel panel = (DragDockPanel)child;

                    orderedPanels.Add(
                        (Grid.GetRow(panel) * this.columns) + Grid.GetColumn(panel),
                        panel);
                }

                // Set initial top of minimized panels to 0
                double currentOffset = 0.0;

                // For each of the panels (as ordered in the grid)
                for (int i = 0; i < orderedPanels.Count; i++)
                {
                    // If the current panel is not the maximized panel
                    if (orderedPanels[i] != this.maximizedPanel)
                    {
                        #region determine new width & height depending on docking axis
                        double newWidth = this.minimizedColumnWidth - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                        double newHeight = (this.ActualHeight / (double)(this.panels.Count - 1)) - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        if (this.minimizedPosition.Equals(MinimizedPositions.Bottom) || this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newWidth = (this.ActualWidth / (double)(this.panels.Count - 1)) - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                            newHeight = this.minimizedRowHeight - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        }
                        #endregion

                        if (newHeight < 0)
                        {
                            newHeight = 0;
                        }

                        if (newWidth < 0)
                        {
                            newWidth = 0;
                        }

                        // Set the size of the panel
                        orderedPanels[i].Width = newWidth;
                        orderedPanels[i].Height = newHeight;

                        double newX = 0;
                        double newY = currentOffset;
                        #region determin new docking coordinates
                        if (this.minimizedPosition.Equals(MinimizedPositions.Right))
                        {
                            newX = this.ActualWidth - this.minimizedColumnWidth;
                            newY = currentOffset;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Left))
                        {
                            newX = 0;
                            newY = currentOffset;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Bottom))
                        {
                            newX = currentOffset;
                            newY = this.ActualHeight - this.minimizedRowHeight;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newX = currentOffset;
                            newY = 0;
                        }
                        #endregion

                        // Set the position of the panel
                        Canvas.SetLeft(orderedPanels[i], newX);
                        Canvas.SetTop(orderedPanels[i], newY);

                        if (this.minimizedPosition.Equals(MinimizedPositions.Left) || this.minimizedPosition.Equals(MinimizedPositions.Right))
                        {
                            // Increment current top
                            currentOffset += this.ActualHeight / (double)(this.panels.Count - 1);
                        }
                        else
                        {
                            // Increment current left
                            currentOffset += this.ActualWidth / (double)(this.panels.Count - 1);
                        }
                    }
                    else
                    {
                        #region determine new width & height depending on docking axis
                        double newWidth = this.ActualWidth - this.minimizedColumnWidth - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                        double newHeight = this.ActualHeight - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        if (this.minimizedPosition.Equals(MinimizedPositions.Bottom) || this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newWidth = this.ActualWidth - orderedPanels[i].Margin.Left - orderedPanels[i].Margin.Right;
                            newHeight = this.ActualHeight - this.minimizedRowHeight - orderedPanels[i].Margin.Top - orderedPanels[i].Margin.Bottom;
                        }
                        #endregion

                        if (newHeight < 0)
                        {
                            newHeight = 0;
                        }

                        if (newWidth < 0)
                        {
                            newWidth = 0;
                        }

                        // Set the size of the panel
                        orderedPanels[i].Width = newWidth;
                        orderedPanels[i].Height = newHeight;

                        #region determine new docking position
                        double newX = 0;
                        double newY = 0;
                        if (this.minimizedPosition.Equals(MinimizedPositions.Right))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Left))
                        {
                            newX = this.minimizedColumnWidth;
                            newY = 0;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Bottom))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newX = 0;
                            newY = this.minimizedRowHeight;
                        }
                        #endregion

                        // Set the position of the panel
                        Canvas.SetLeft(orderedPanels[i], newX);
                        Canvas.SetTop(orderedPanels[i], newY);
                    }
                }
            }
        }

        /// <summary>
        /// Animates the panel sizes
        /// </summary>
        private void AnimatePanelSizes()
        {
            if (double.IsInfinity(this.ActualWidth) || double.IsNaN(this.ActualWidth) || this.ActualWidth == 0)
            {
                return;
            }

            // If there is not a maxmized panel...
            if (this.maximizedPanel == null)
            {
                // Animate the panel sizes to row / column sizes
                foreach (UIElement child in this.panels)
                {
                    DragDockPanel panel = (DragDockPanel)child;

                    double width = (this.ActualWidth / (double)this.columns) - panel.Margin.Left - panel.Margin.Right;
                    double height = (this.ActualHeight / (double)this.rows) - panel.Margin.Top - panel.Margin.Bottom;

                    if (width < 0)
                    {
                        width = 0;
                    }

                    if (height < 0)
                    {
                        height = 0;
                    }

                    panel.AnimateSize(
                        width,
                        height);
                }
            }
            else
            {
                // Loop through the children
                foreach (UIElement child in this.panels)
                {
                    DragDockPanel panel =
                        (DragDockPanel)child;

                    // Set the size of the non 
                    // maximized children
                    if (panel != this.maximizedPanel)
                    {
                        #region determine new width & height depending on docking axis
                        double newWidth = this.minimizedColumnWidth - panel.Margin.Left - panel.Margin.Right;
                        double newHeight = (this.ActualHeight / (double)(this.panels.Count - 1)) - panel.Margin.Top - panel.Margin.Bottom;
                        if (this.minimizedPosition.Equals(MinimizedPositions.Bottom) || this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newWidth = (this.ActualWidth / (double)(this.panels.Count - 1)) - panel.Margin.Left - panel.Margin.Right;
                            newHeight = this.minimizedRowHeight - panel.Margin.Top - panel.Margin.Bottom;
                        }
                        #endregion

                        if (newHeight < 0)
                        {
                            newHeight = 0;
                        }

                        if (newWidth < 0)
                        {
                            newWidth = 0;
                        }

                        panel.AnimateSize(
                            newWidth,
                            newHeight);
                    }
                    else
                    {
                        #region determine new width & height depending on docking axis
                        double newWidth = this.ActualWidth - this.minimizedColumnWidth - panel.Margin.Left - panel.Margin.Right;
                        double newHeight = this.ActualHeight - panel.Margin.Top - panel.Margin.Bottom;
                        if (this.minimizedPosition.Equals(MinimizedPositions.Bottom) || this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newWidth = this.ActualWidth - panel.Margin.Left - panel.Margin.Right;
                            newHeight = this.ActualHeight - this.minimizedRowHeight - panel.Margin.Top - panel.Margin.Bottom;
                        }
                        #endregion

                        if (newHeight < 0)
                        {
                            newHeight = 0;
                        }

                        if (newWidth < 0)
                        {
                            newWidth = 0;
                        }

                        panel.AnimateSize(
                            newWidth,
                            newHeight);
                    }
                }
            }
        }

        /// <summary>
        /// Animate the panel positions
        /// </summary>
        private void AnimatePanelLayout()
        {
            if (double.IsInfinity(this.ActualWidth) || double.IsNaN(this.ActualWidth) || this.ActualWidth == 0)
            {
                return;
            }

            // If we are not in max'ed panel mode...
            if (this.maximizedPanel == null)
            {
                // Loop through children and size to row and columns
                foreach (UIElement child in this.panels)
                {
                    DragDockPanel panel = (DragDockPanel)child;

                    if (panel != this.draggingPanel)
                    {
                        panel.AnimatePosition(
                            (Grid.GetColumn(panel) * (this.ActualWidth / (double)this.columns)),
                            (Grid.GetRow(panel) * (this.ActualHeight / (double)this.rows)));
                    }
                }
            }
            else
            {
                Dictionary<int, DragDockPanel> orderedPanels = new Dictionary<int, DragDockPanel>();

                // Loop through children to order them according to their
                // current row and column...
                foreach (UIElement child in this.panels)
                {
                    DragDockPanel panel = (DragDockPanel)child;

                    orderedPanels.Add(
                        (Grid.GetRow(panel) * this.columns) + Grid.GetColumn(panel),
                        panel);
                }

                // Set initial top of minimized panels to 0
                double currentOffset = 0.0;

                // For each of the panels (as ordered in the grid)
                for (int i = 0; i < orderedPanels.Count; i++)
                {
                    // If the current panel is not the maximized panel
                    if (orderedPanels[i] != this.maximizedPanel)
                    {
                        double newX = 0;
                        double newY = currentOffset;
                        #region determin new docking coordinates
                        if (this.minimizedPosition.Equals(MinimizedPositions.Right))
                        {
                            newX = this.ActualWidth - this.minimizedColumnWidth;
                            newY = currentOffset;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Left))
                        {
                            newX = 0;
                            newY = currentOffset;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Bottom))
                        {
                            newX = currentOffset;
                            newY = this.ActualHeight - this.minimizedRowHeight;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newX = currentOffset;
                            newY = 0;
                        }
                        #endregion

                        // Animate the position
                        orderedPanels[i].AnimatePosition(
                            newX,
                            newY);

                        if (this.minimizedPosition.Equals(MinimizedPositions.Left) || this.minimizedPosition.Equals(MinimizedPositions.Right))
                        {
                            // Increment current top
                            currentOffset += this.ActualHeight / (double)(this.panels.Count - 1);
                        }
                        else
                        {
                            // Increment current left
                            currentOffset += this.ActualWidth / (double)(this.panels.Count - 1);
                        }
                    }
                    else
                    {
                        #region determine new docking position
                        double newX = 0;
                        double newY = 0;
                        if (this.minimizedPosition.Equals(MinimizedPositions.Right))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Left))
                        {
                            newX = this.minimizedColumnWidth;
                            newY = 0;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Bottom))
                        {
                            newX = 0;
                            newY = 0;
                        }
                        else if (this.minimizedPosition.Equals(MinimizedPositions.Top))
                        {
                            newX = 0;
                            newY = this.minimizedRowHeight;
                        }
                        #endregion
                        // Animate maximized panel                        
                        orderedPanels[i].AnimatePosition(newX, newY);
                    }
                }
            }
        }
        #endregion
    }
}
