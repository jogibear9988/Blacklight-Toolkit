//-----------------------------------------------------------------------
// <copyright file="DraggablePanel.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>02-Mar-2009</date>
// <author>Martin Grayson</author>
// <summary>Base class for draggable panels.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
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

    /// <summary>
    /// Base class for draggable panels.
    /// </summary>
    public class DraggablePanel : AnimatedHeaderedContentControl
    {
        /// <summary>
        /// The DraggingEnabled Dependency Property.
        /// </summary>
        public static readonly DependencyProperty DraggingEnabledProperty =
            DependencyProperty.Register("DraggingEnabled", typeof(bool), typeof(DraggablePanel), new PropertyMetadata(true));

        /// <summary>
        /// The GripBar Template Part Name.
        /// </summary>
        private const string ElementGripBarElement = "GripBarElement";
        
        /// <summary>
        /// Stores the current z-index
        /// </summary>
        private static int currentZIndex = 1;

        /// <summary>
        /// Stores the last drag position.
        /// </summary>
        private Point lastDragPosition;

        /// <summary>
        /// Is dragging flag.
        /// </summary>
        private bool dragging = false;
        #region Public events
        /// <summary>
        /// The drag started event.
        /// </summary>
        public event DragEventHander DragStarted;

        /// <summary>
        /// The drag moved event.
        /// </summary>
        public event DragEventHander DragMoved;

        /// <summary>
        /// The drag finished event.
        /// </summary>
        public event DragEventHander DragFinished;

        /// <summary>
        /// The Panel Focused event.
        /// </summary>
        public event EventHandler PanelFocused;
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether dragging is enabled.
        /// </summary>
        public bool DraggingEnabled
        {
            get { return (bool)GetValue(DraggingEnabledProperty); }
            set { SetValue(DraggingEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the panels current z-index.
        /// </summary>
        internal static int CurrentZIndex
        {
            get { return DraggablePanel.currentZIndex; }
            set { DraggablePanel.currentZIndex = value; }
        }

        /// <summary>
        /// Gets or sets the last drag position.
        /// </summary>
        internal Point LastDragPosition
        {
            get { return this.lastDragPosition; }
            set { this.lastDragPosition = value; }
        }

        /// <summary>
        /// Gets the template parts from the panel.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FrameworkElement gripBarElement = this.GetTemplateChild(DraggablePanel.ElementGripBarElement) as FrameworkElement;
            if (gripBarElement != null)
            {
                gripBarElement.MouseLeftButtonDown += new MouseButtonEventHandler(this.GripBarElement_MouseLeftButtonDown);
                gripBarElement.MouseMove += new MouseEventHandler(this.GripBarElement_MouseMove);
                gripBarElement.MouseLeftButtonUp += new MouseButtonEventHandler(this.GripBarElement_MouseLeftButtonUp);
            }
        }

        /// <summary>
        /// Base method for updating the panels position.
        /// </summary>
        /// <param name="pos">The new position.</param>
        public virtual void UpdatePosition(Point pos)
        {
            Canvas.SetLeft(this, Math.Max(0, pos.X));
            Canvas.SetTop(this, Math.Max(0, pos.Y));
        }

        /// <summary>
        /// Base method for updating the panel size.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public virtual void UpdateSize(double width, double height)
        {
            this.Width = Math.Max(this.MinWidth, width);
            this.Height = Math.Max(this.MinHeight, height);
        }

        /// <summary>
        /// Brings the panel to the front.
        /// </summary>
        /// <param name="e">Routed Event Args.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            // Bring the panel to the front
            Canvas.SetZIndex(this, CurrentZIndex++);

            if (this.PanelFocused != null)
            {
                this.PanelFocused(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Brings the panel to the front of the canvas.
        /// </summary>
        /// <param name="e">Mouse Button Event Args.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Bring the panel to the front
            Canvas.SetZIndex(this, CurrentZIndex++);

            if (this.PanelFocused != null)
            {
                this.PanelFocused(this, EventArgs.Empty);
            }
        }

        #region Dragging events
        /// <summary>
        /// Starts the dragging.
        /// </summary>
        /// <param name="sender">The grip bar.</param>
        /// <param name="e">Mouse button event args.</param>
        private void GripBarElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse
            ((FrameworkElement)sender).CaptureMouse();

            // Store the start position
            this.LastDragPosition = e.GetPosition(sender as UIElement);

            // Set dragging to true
            this.dragging = true;

            // Fire the drag started event
            if (this.DragStarted != null)
            {
                this.DragStarted(this, new DragEventArgs(0, 0, e));
            }
        }

        /// <summary>
        /// Ends the dragging.
        /// </summary>
        /// <param name="sender">The grip bar.</param>
        /// <param name="e">Mouse button event args.</param>
        private void GripBarElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse
            ((FrameworkElement)sender).ReleaseMouseCapture();

            // Set dragging to true
            this.dragging = false;

            Point position = e.GetPosition(sender as UIElement);

            // Fire the drag finished event
            if (this.DragFinished != null)
            {
                this.DragFinished(this, new DragEventArgs(position.X - this.LastDragPosition.X, position.Y - this.LastDragPosition.Y, e));
            }
        }

        /// <summary>
        /// Drags the panel (if the panel is being dragged).
        /// </summary>
        /// <param name="sender">The grip bar.</param>
        /// <param name="e">Mouse event args.</param>
        private void GripBarElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                Point position = e.GetPosition(sender as UIElement);

                // Move the panel
                this.UpdatePosition(new Point(Canvas.GetLeft(this) + position.X - this.LastDragPosition.X, Canvas.GetTop(this) + position.Y - this.LastDragPosition.Y));

                // Fire the drag moved event
                if (this.DragMoved != null)
                {
                    this.DragMoved(this, new DragEventArgs(position.X - this.LastDragPosition.X, position.Y - this.LastDragPosition.Y, e));
                }

#if SILVERLIGHT
                // Update the last mouse position
                this.LastDragPosition = e.GetPosition(sender as UIElement);
#endif
            }
        }
        #endregion
    }
}
