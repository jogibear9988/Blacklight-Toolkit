//-----------------------------------------------------------------------
// <copyright file="AnimatedLayoutPanel.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>01-Nov-2008</date>
// <author>Mike Parker</author>
// <summary>A panel that wraps content and animates items as part of the arrange.</summary>
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
    using System.ComponentModel;

    /// <summary>
    /// The AnimatedLayoutPanel item entrance start positions.
    /// </summary>
    public enum EntranceStartPosition
    {
        /// <summary>
        /// Begins the entrance animation from the top left of the panel.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Begins the entrance animation from the top centre of the panel.
        /// </summary>
        TopCentre,

        /// <summary>
        /// Begins the entrance animation from the top right of the panel.
        /// </summary>
        TopRight,

        /// <summary>
        /// DBegins the entrance animation from the middle left of the panel.
        /// </summary>
        MiddleLeft,

        /// <summary>
        /// Begins the entrance animation from the middle centre of the panel.
        /// </summary>
        MiddleCentre,

        /// <summary>
        /// Begins the entrance animation from the middle right of the panel.
        /// </summary>
        MiddleRight,

        /// <summary>
        /// Begins the entrance animation from the bottom left of the panel.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Begins the entrance animation from the bottom centre of the panel.
        /// </summary>
        BottomCentre,

        /// <summary>
        /// Begins the entrance animation from the bottom right of the panel.
        /// </summary>
        BottomRight,
    }

    /// <summary>
    /// Determines which way the panel items are rendered.
    /// </summary>
    public enum WrapDirection
    {
        /// <summary>
        /// Wraps the items left to right.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Wraps the items top to bottom.
        /// </summary>
        Vertical,
    }

    /// <summary>
    /// A panel that wraps content and animates items as part of the arrange.
    /// </summary>
    public class AnimatedLayoutPanel : Panel
    {
        #region Dependency Properties

        /// <summary>
        /// The Transition Duration Property.
        /// </summary>
        public static readonly DependencyProperty TransitionDurationProperty =
            DependencyProperty.Register("TransitionDuration", typeof(TimeSpan), typeof(AnimatedLayoutPanel), null);

        /// <summary>
        /// The AnimateOnInitialise Property.
        /// </summary>
        public static readonly DependencyProperty AnimateOnInitialiseProperty =
            DependencyProperty.Register("AnimateOnInitialise", typeof(bool), typeof(AnimatedLayoutPanel), null);

        /// <summary>
        /// The EntranceAnimationEnabled Property.
        /// </summary>
        public static readonly DependencyProperty EntranceAnimationEnabledProperty =
            DependencyProperty.Register("EntranceAnimationEnabled", typeof(bool), typeof(AnimatedLayoutPanel), null);

        /// <summary>
        /// The ItemResizeEnabled Property.
        /// </summary>
        public static readonly DependencyProperty ItemResizeEnabledProperty =
            DependencyProperty.Register("ItemResizeEnabled", typeof(bool), typeof(AnimatedLayoutPanel), new PropertyMetadata(new PropertyChangedCallback(ItemResizeEnabled_Changed)));

        /// <summary>
        /// The ItemHeight Property.
        /// </summary>
        public static readonly DependencyProperty ItemResizeHeightProperty =
            DependencyProperty.Register("ItemResizeHeight", typeof(double), typeof(AnimatedLayoutPanel), new PropertyMetadata(new PropertyChangedCallback(ItemHeight_Changed)));

        /// <summary>
        /// The ItemWidth Property.
        /// </summary>
        public static readonly DependencyProperty ItemResizeWidthProperty =
            DependencyProperty.Register("ItemResizeWidth", typeof(double), typeof(AnimatedLayoutPanel), new PropertyMetadata(new PropertyChangedCallback(ItemWidth_Changed)));

        /// <summary>
        /// The ItemMarginEnabled Property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginEnabledProperty =
            DependencyProperty.Register("ItemMarginEnabled", typeof(bool), typeof(AnimatedLayoutPanel), new PropertyMetadata(new PropertyChangedCallback(ItemMarginEnabled_Changed)));

        /// <summary>
        /// The ItemMargin Property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(AnimatedLayoutPanel), new PropertyMetadata(new PropertyChangedCallback(ItemMargin_Changed)));

        /// <summary>
        /// The DisableEntrance Property.
        /// </summary>
        public static readonly DependencyProperty DisableEntranceProperty =
            DependencyProperty.RegisterAttached("DisableEntrance", typeof(bool), typeof(AnimatedLayoutPanel), null);

        /// <summary>
        /// The OverrideItemResize Property.
        /// </summary>
        public static readonly DependencyProperty OverrideItemResizeProperty =
            DependencyProperty.RegisterAttached("OverrideItemResize", typeof(bool), typeof(AnimatedLayoutPanel), null);

        /// <summary>
        /// The OverrideItemMargin Property.
        /// </summary>             
        public static readonly DependencyProperty OverrideItemMarginProperty =
            DependencyProperty.RegisterAttached("OverrideItemMargin", typeof(bool), typeof(AnimatedLayoutPanel), null);

        #endregion

        #region Private Members

        /// <summary>
        /// Identifies whether the panel has performed its initial layout pass.
        /// </summary>
        private bool performedInitialLayout;

        /// <summary>
        /// Stores the actual height of the control to account for infinity and NaN.
        /// </summary>
        private double internalHeight;

        /// <summary>
        /// Stores the actual width of the control to account for infinity and NaN.
        /// </summary>
        private double internalWidth;

        /// <summary>
        /// Determines the starting position for each panel item for its entrance.
        /// </summary>
        private EntranceStartPosition startPosition;

        /// <summary>
        /// Determines the direction in which the panel items are rendered.
        /// </summary>
        private WrapDirection wrapDirection;

        #endregion

        #region Constructor

        /// <summary>
        /// Animated Layout Panel constructor.
        /// </summary>
        public AnimatedLayoutPanel()
        {
            // Create internal event handlers
            this.SizeChanged += new SizeChangedEventHandler(this.AnimatedLayoutPanel_SizeChanged);

            // Set initial property values
            this.wrapDirection = WrapDirection.Horizontal;
            this.startPosition = EntranceStartPosition.TopLeft;
            this.TransitionDuration = TimeSpan.FromMilliseconds(200);
            this.AnimateOnInitialise = true;
            this.EntranceAnimationEnabled = true;
            this.ItemResizeHeight = 50;
            this.ItemResizeWidth = 50;
            this.ItemMargin = new Thickness(5);
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets the entrance starting position for the panel items.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets the entrance starting position for the panel items.")]
        public EntranceStartPosition EntranceStartPosition
        {
            get
            {
                return this.startPosition;
            }

            set
            {
                this.startPosition = value;

#if SILVERLIGHT
                // Provide design-time preview of entrance
                if (Application.Current.RootVisual != null)
                {
                    if (DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual))
                    {
                        if (this.performedInitialLayout && this.EntranceAnimationEnabled)
                        {
                            this.RemoveEntrancePreviewItem();
                            this.PreviewItemEntrance();
                        }
                    }
                }
#endif
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether new panel items should animate into position.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets a value indicating whether new panel items should animate into position.")]
        public bool EntranceAnimationEnabled
        {
            get { return (bool)GetValue(EntranceAnimationEnabledProperty); }
            set { SetValue(EntranceAnimationEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the wrap direction for the panel items.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets the wrap direction for the panel items.")]
        public WrapDirection WrapDirection
        {
            get
            {
                return this.wrapDirection;
            }

            set
            {
                this.wrapDirection = value;
                this.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the panel is allowed to resize its items.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets a value indicating whether the panel is allowed to resize its items.")]
        public bool ItemResizeEnabled
        {
            get { return (bool)GetValue(ItemResizeEnabledProperty); }
            set { SetValue(ItemResizeEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of each item in the panel.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets the height of each item in the panel.")]
        public double ItemResizeHeight
        {
            get { return (double)GetValue(ItemResizeHeightProperty); }
            set { SetValue(ItemResizeHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of each item in the panel.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets the width of each item in the panel.")]
        public double ItemResizeWidth
        {
            get { return (double)GetValue(ItemResizeWidthProperty); }
            set { SetValue(ItemResizeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the panel is allowed to override the Margin Property for its items.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets a value indicating whether the panel is allowed to override the Margin Property for its items.")]
        public bool ItemMarginEnabled
        {
            get { return (bool)GetValue(ItemMarginEnabledProperty); }
            set { SetValue(ItemMarginEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Margin that is to be applied to each panel item (this overrides any existing Margin that is applied).
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets the Margin that is to be applied to each panel item (this overrides any existing Margin that is applied).")]
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets how long the item transition animation takes to complete.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets how long the item transition animation takes to complete.")]
        public TimeSpan TransitionDuration
        {
            get { return (TimeSpan)GetValue(TransitionDurationProperty); }
            set { SetValue(TransitionDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the panel items should animate into position on start-up.
        /// </summary>
        [Category("Panel Behaviours"), Description("Gets or sets a value indicating whether the panel items should animate into position on start-up.")]
        public bool AnimateOnInitialise
        {
            get { return (bool)GetValue(AnimateOnInitialiseProperty); }
            set { SetValue(AnimateOnInitialiseProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the panel item needs to have its entrance animated.
        /// </summary>
        /// <param name="obj">The panel item.</param>
        /// <returns>Boolean indicator.</returns>
        public static bool GetDisableEntrance(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisableEntranceProperty);
        }

        /// <summary>
        /// Sets a value indicating whether the panel item needs to have its entrance animated.
        /// </summary>
        /// <param name="obj">The panel item.</param>
        /// <param name="value">Boolean indicator.</param>
        public static void SetDisableEntrance(DependencyObject obj, bool value)
        {
            obj.SetValue(DisableEntranceProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the panel item will ignore the panel resize behaviours.
        /// </summary>
        /// <param name="obj">The panel item.</param>
        /// <returns>Boolean indicator.</returns>            
        public static bool GetOverrideItemResize(DependencyObject obj)
        {
            return (bool)obj.GetValue(OverrideItemResizeProperty);
        }

        /// <summary>
        /// Sets a value indicating whether the panel item will ignore the panel resize behaviours.
        /// </summary>
        /// <param name="obj">The panel item.</param>
        /// <param name="value">Boolean indicator.</param>        
        public static void SetOverrideItemResize(DependencyObject obj, bool value)
        {
            obj.SetValue(OverrideItemResizeProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the panel item will ignore the item margin behaviours.
        /// </summary>
        /// <param name="obj">The panel item.</param>
        /// <returns>Boolean indicator.</returns>        
        public static bool GetOverrideItemMargin(DependencyObject obj)
        {
            return (bool)obj.GetValue(OverrideItemMarginProperty);
        }

        /// <summary>
        /// Sets a value indicating whether the panel item will ignore the item margin behaviours.
        /// </summary>
        /// <param name="obj">The panel item.</param>
        /// <param name="value">Boolean indicator.</param>
        public static void SetOverrideItemMargin(DependencyObject obj, bool value)
        {
            obj.SetValue(OverrideItemMarginProperty, value);
        }

        #endregion

        #region Panel Override Members

        /// <summary>
        /// Determines the layout of the Child elements.
        /// </summary>
        /// <param name="availableSize">The space available for the panel to display its child objects.</param>
        /// <returns>Size of the panel.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            base.MeasureOverride(availableSize);

            double top = 0;
            double left = 0;
            bool widthInfinity = false;
            bool heightInfinity = false;
            double greatestHeight = 0;
            double greatestWidth = 0;

            // Check finalSize does not return infinity
            if (double.IsInfinity(availableSize.Width))
            {
                widthInfinity = true;
                this.internalWidth = 0;
            }
            else
            {
                this.internalWidth = availableSize.Width;
            }

            if (double.IsInfinity(availableSize.Height))
            {
                heightInfinity = true;
                this.internalHeight = 0;
            }
            else
            {
                this.internalHeight = availableSize.Height;
            }

            foreach (FrameworkElement child in this.Children)
            {
                // Check if ItemMargin override is enabled on the panel
                // Check it is not overridden by the item
                if (this.ItemMarginEnabled && !GetOverrideItemMargin(child))
                {
                    child.Margin = this.ItemMargin;
                }

                // Check if ItemResize override is enabled on the panel
                // Check it is not overridden by the item
                if (this.ItemResizeEnabled && !GetOverrideItemResize(child))
                {
                    if (this.ItemResizeHeight != double.NaN)
                    {
                        child.Height = this.ItemResizeHeight;
                    }

                    if (this.ItemResizeWidth != double.NaN)
                    {
                        child.Width = this.ItemResizeWidth;
                    }
                }

                // Set DisableEntrance to true on all existing items if AnimateOnInitialise is set to true
                if (!this.AnimateOnInitialise && !this.performedInitialLayout)
                {
                    SetDisableEntrance(child, true);
                }

                child.Measure(availableSize);

                if (this.wrapDirection == WrapDirection.Horizontal)
                {
                    // If infinite space is available, keep positioning left to right
                    // If not, check to see if we have reached our furthest position
                    if (!widthInfinity)
                    {
                        if (left + child.DesiredSize.Width > availableSize.Width)
                        {
                            // Set top for new line and reset left and greatestHeight properties
                            top += greatestHeight;

                            left = 0;
                            greatestHeight = 0;
                        }
                    }
                }
                else
                {
                    // If infinite space is available, keep positioning top to bottom
                    // If not, check to see if we have reached our furthest position
                    if (!heightInfinity)
                    {
                        if (top + child.DesiredSize.Height > availableSize.Height)
                        {
                            // Set top for new line and reset left and greatestHeight properties
                            left += greatestWidth;

                            top = 0;
                            greatestWidth = 0;
                        }
                    }
                }                

                if (this.wrapDirection == WrapDirection.Horizontal)
                {
                    left += child.DesiredSize.Width;
                }
                else
                {
                    top += child.DesiredSize.Height;
                }

                // Keep track of greatest height and width on this line of items
                if (child.DesiredSize.Width > greatestWidth)
                {
                    greatestWidth = child.DesiredSize.Width;
                }

                if (child.DesiredSize.Height > greatestHeight)
                {
                    greatestHeight = child.DesiredSize.Height;
                }

                // Maintain internal measurements     
                if (this.wrapDirection == WrapDirection.Horizontal)
                {
                    if (left > this.internalWidth)
                    {
                        this.internalWidth = left;
                    }

                    if (top + child.DesiredSize.Height > this.internalHeight)
                    {
                        this.internalHeight = top + child.DesiredSize.Height;
                    }
                }
                else
                {
                    if (left + child.DesiredSize.Width > this.internalWidth)
                    {
                        this.internalWidth = left + child.DesiredSize.Width;
                    }

                    if (top > this.internalHeight)
                    {
                        this.internalHeight = top;
                    }
                }
            }

            // Do not return Infinity as a value
            if (heightInfinity)
            {
                availableSize.Height = this.internalHeight;
                this.internalHeight = availableSize.Height;
            }

            if (widthInfinity)
            {
                availableSize.Width = this.internalWidth;
                this.internalWidth = availableSize.Width;
            }

            return availableSize;
        }

        /// <summary>
        /// Performs the layout for all child controls.
        /// </summary>
        /// <param name="finalSize">The space available for the panel to display its child objects.</param>
        /// <returns>Size of the panel.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);

            double top = 0;
            double left = 0;
            bool widthInfinity = false;
            bool heightInfinity = false;
            double greatestHeight = 0;
            double greatestWidth = 0;

            // Check finalSize does not return infinity
            if (double.IsInfinity(finalSize.Width))
            {
                widthInfinity = true;
            }

            if (double.IsInfinity(finalSize.Height))
            {
                heightInfinity = true;
            }

            foreach (FrameworkElement child in this.Children)
            {
                if (this.wrapDirection == WrapDirection.Horizontal)
                {
                    // If infinite space is available, keep positioning left to right
                    // If not, check to see if we have reached our furthest position
                    if (!widthInfinity)
                    {
                        if (left + child.DesiredSize.Width > finalSize.Width)
                        {
                            // Set top for new line and reset left and greatestHeight properties
                            top += greatestHeight;

                            left = 0;
                            greatestHeight = 0;
                        }
                    }
                }
                else
                {
                    // If infinite space is available, keep positioning top to bottom
                    // If not, check to see if we have reached our furthest position
                    if (!heightInfinity)
                    {
                        if (top + child.DesiredSize.Height > finalSize.Height)
                        {
                            // Set top for new line and reset left and greatestHeight properties
                            left += greatestWidth;

                            top = 0;
                            greatestWidth = 0;
                        }
                    }
                }

                // Create child within panel
                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                // Ensure the child item has a unique name
                this.ConfirmItemName(child);

                // Get the information about the Transform applied to the item
                TransformInformation information = this.ConfirmTransform(child);

                TranslateTransform translate = information.TranslateTransform;

                // Set child element position to where it is currently
                if (!this.AnimateOnInitialise && !this.performedInitialLayout)
                {
                    // Set in correct position
                    translate.X = left;
                    translate.Y = top;
                }

                // Get the Storyboard for the child item
                Storyboard storyboard = this.ConfirmStoryboard(child, information.PositionInCollection);

                // Configure animation properties for translateX
                DoubleAnimation leftAnim = (DoubleAnimation)storyboard.Children[0];
                leftAnim.To = left;

                // Configure animation properties for translateX
                DoubleAnimation topAnim = (DoubleAnimation)storyboard.Children[1];
                topAnim.To = top;

                // Check if the item is new and set FROM property accordingly
                if (!GetDisableEntrance(child) && this.EntranceAnimationEnabled)
                {
                    // Set FROM Property
                    Point enterFrom = this.GetEntryPosition(child);
                    leftAnim.From = enterFrom.X;
                    topAnim.From = enterFrom.Y;
                    translate.X = enterFrom.X;
                    translate.Y = enterFrom.Y;
                    SetDisableEntrance(child, true);
                }
                else if (!GetDisableEntrance(child) && !this.EntranceAnimationEnabled)
                {
                    leftAnim.From = left;
                    topAnim.From = top;
                    SetDisableEntrance(child, true);
                }
                else
                {
                    leftAnim.From = null;
                    topAnim.From = null;
                    SetDisableEntrance(child, true);
                }

                // Begin the Storyboard
                storyboard.Begin();

#if SILVERLIGHT
                // Check if the item is a design-time entrance preview
                // Provide design-time preview of entrance
                if (DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual))
                {
                    storyboard.Completed += new EventHandler(this.EntrancePreviewStoryboard_Completed);
                }
#endif

                // Increment the positioning variables based on orientation
                if (this.wrapDirection == WrapDirection.Horizontal)
                {
                    left += child.DesiredSize.Width;
                }
                else
                {
                    top += child.DesiredSize.Height;
                }

                // Keep track of greatest height and width on this line of items
                if (child.DesiredSize.Width > greatestWidth)
                {
                    greatestWidth = child.DesiredSize.Width;
                }

                if (child.DesiredSize.Height > greatestHeight)
                {
                    greatestHeight = child.DesiredSize.Height;
                }
            }

            // Indicate that the panel has completed the first layout pass
            this.performedInitialLayout = true;

            // Clip out of bound content
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            Rect clip = new Rect(new Point(0, 0), new Size(finalSize.Width, finalSize.Height));
            rectangleGeometry.Rect = clip;
            this.Clip = rectangleGeometry;

            return finalSize;
        }

        #endregion

        #region Callback Methods

        /// <summary>
        /// Sets the Height property for all child items in the panel.
        /// </summary>
        /// <param name="dependencyObject">The AnimatedLayoutPanel.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args.</param>
        private static void ItemHeight_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            // Do not allow Auto or infinity
            double newValue = (double)eventArgs.NewValue;
            if (double.IsNaN(newValue) || double.IsInfinity(newValue))
            {
                (dependencyObject as AnimatedLayoutPanel).ItemResizeHeight = (double)eventArgs.OldValue;
            }
            else
            {
                (dependencyObject as AnimatedLayoutPanel).InvalidateMeasure();
            }
        }

        /// <summary>
        /// Sets the Width property for all child items in the panel.
        /// </summary>
        /// <param name="dependencyObject">The AnimatedLayoutPanel.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args.</param>
        private static void ItemWidth_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            // Do not allow Auto or infinity
            double newValue = (double)eventArgs.NewValue;
            if (double.IsNaN(newValue) || double.IsInfinity(newValue))
            {
                (dependencyObject as AnimatedLayoutPanel).ItemResizeWidth = (double)eventArgs.OldValue;
            }
            else
            {
                (dependencyObject as AnimatedLayoutPanel).InvalidateMeasure();
            }
        }

        /// <summary>
        /// Enables or disables the resizing of all child items in the panel.
        /// </summary>
        /// <param name="dependencyObject">The AnimatedLayoutPanel.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args.</param>
        private static void ItemResizeEnabled_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            (dependencyObject as AnimatedLayoutPanel).InvalidateMeasure();
        }

        /// <summary>
        /// Enables or disables the ability to set a common Margin for all child items in the panel.
        /// </summary>
        /// <param name="dependencyObject">The AnimatedLayoutPanel.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args.</param>
        private static void ItemMarginEnabled_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            (dependencyObject as AnimatedLayoutPanel).InvalidateMeasure();
        }

        /// <summary>
        /// Sets a consistent Margin for each of the panel items.
        /// </summary>
        /// <param name="dependencyObject">Rhe AnimatedLayoutPanel.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args.</param>
        private static void ItemMargin_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            (dependencyObject as AnimatedLayoutPanel).InvalidateMeasure();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Performs a check to determine if there is available space within the bounds of the panel to continue to render the child items.
        /// </summary>
        /// <param name="child">The item to be rendered next.</param>
        /// <param name="availableSpace">The remaining space available.</param>
        /// <param name="left">The current left position.</param>
        /// <param name="top">The current top position.</param>
        /// <returns>Boolean value.</returns>
        private bool ExceedsBounds(FrameworkElement child, Size availableSpace, double left, double top)
        {
            // Check for RenderingSpace
            if (left + child.DesiredSize.Width > availableSpace.Width && top + child.DesiredSize.Height > availableSpace.Height)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Provides a design-time preview of how a new item will enter the panel.
        /// </summary>
        private void PreviewItemEntrance()
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 30;
            ellipse.Width = 30;
            ellipse.Fill = new SolidColorBrush(Colors.LightGray);
            string previewIdentifier = "PREVIEW";
            ellipse.Tag = previewIdentifier;
            SetOverrideItemResize(ellipse, true);
            SetOverrideItemMargin(ellipse, true);
            this.Children.Add(ellipse);
        }

        /// <summary>
        /// Remove the preview item the panel children when preview is complete.
        /// </summary>
        /// <param name="sender">Storyboard object.</param>
        /// <param name="e">Event Arguments.</param>
        private void EntrancePreviewStoryboard_Completed(object sender, EventArgs e)
        {
            this.RemoveEntrancePreviewItem();
        }

        /// <summary>
        /// Remove the preview item the panel children.
        /// </summary>
        private void RemoveEntrancePreviewItem()
        {
            // Remove any preview elements from Children            
            foreach (FrameworkElement child in this.Children)
            {
                if (child.Tag != null && child.Tag.GetType() == typeof(string))
                {
                    if ((child.Tag as string) == "PREVIEW")
                    {
                        this.Children.Remove(child);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Ensures the child item has a unique name assigned to it.
        /// </summary>
        /// <param name="child">The child item.</param>
        /// <returns>The Name property for the child item.</returns>
        private string ConfirmItemName(FrameworkElement child)
        {
            if (String.IsNullOrEmpty(child.Name))
            {
                // Create new Name for item
                // Name can not start with number in WPF
                child.Name = "a" + Guid.NewGuid().ToString().Replace('-','_');
            }

            return child.Name;
        }

        /// <summary>
        /// Ensures the child item has the appropriate TranslateTransform to perform the animation.
        /// </summary>
        /// <param name="child">the child item.</param>
        /// <returns>Transform Information.</returns>
        private TransformInformation ConfirmTransform(FrameworkElement child)
        {
            TransformInformation transformInformation = new TransformInformation();

            // Check if TransformGroup already exists
            if (child.RenderTransform.GetType() != typeof(TransformGroup))
            {
                // Create TranslateTransform and TransformGroup
                TransformGroup transformGroup = new TransformGroup();
                TranslateTransform translate = new TranslateTransform();
                transformGroup.Children.Add(translate);
                child.RenderTransform = transformGroup;
                child.RenderTransformOrigin = new Point(0.5, 0.5);

                // Initialise the TransformInformation class
                transformInformation = new TransformInformation(translate, 0);
            }
            else
            {
                // Check that a RenderTransform object exists within the TransformGroup
                TransformGroup transformGroup = (TransformGroup)child.RenderTransform;
                TranslateTransform translate = new TranslateTransform();

                bool needsGenerating = true;
                int positionInCollection = 0;

                for (int i = 0; i < transformGroup.Children.Count; i++)
                {
                    if (transformGroup.Children[i].GetType() == typeof(TranslateTransform))
                    {
                        translate = (TranslateTransform)transformGroup.Children[i];
                        positionInCollection = i;
                        needsGenerating = false;
                        break;
                    }
                }

                if (needsGenerating)
                {
                    // Add new TranslateTransform and add to current TransformGroup
                    transformGroup.Children.Add(translate);
                    positionInCollection = transformGroup.Children.Count - 1;
                }

                // Set Transform Properties
                child.RenderTransform = transformGroup;
                child.RenderTransformOrigin = new Point(0.5, 0.5);

                // Create TransformInformation
                transformInformation = new TransformInformation(translate, positionInCollection);
            }

            return transformInformation;
        }

        /// <summary>
        /// Ensures the child item has the appropriate Storyboard resource to perform the animation.
        /// </summary>
        /// <param name="child">The child item.</param>
        /// <param name="targetTransformIndex">The position of the RenderTransform child item that is to be animated.</param>
        /// <returns>The Storyboard.</returns>
        private Storyboard ConfirmStoryboard(FrameworkElement child, int targetTransformIndex)
        {
            Storyboard storyboard = new Storyboard();

            if ((child.Resources[child.Name + "PositionStoryboard"] as Storyboard) == null)
            {
                // Add animation for X axis
                DoubleAnimation leftAnim = new DoubleAnimation();
                leftAnim.Duration = new Duration(this.TransitionDuration);
                Storyboard.SetTargetName(leftAnim, child.Name);
                Storyboard.SetTargetProperty(leftAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[" + targetTransformIndex.ToString() + "].(TranslateTransform.X)"));

                // Add animation for Y axis
                DoubleAnimation topAnim = new DoubleAnimation();
                topAnim.Duration = new Duration(this.TransitionDuration);
                Storyboard.SetTargetName(topAnim, child.Name);
                Storyboard.SetTargetProperty(topAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[" + targetTransformIndex.ToString() + "].(TranslateTransform.Y)"));

                storyboard.Children.Add(leftAnim);
                storyboard.Children.Add(topAnim);

                child.Resources.Add(child.Name + "PositionStoryboard", storyboard);
            }
            else
            {
                // Get current storyboard
                storyboard = (Storyboard)child.Resources[child.Name + "PositionStoryboard"];
            }

            return storyboard;
        }

        /// <summary>
        /// Calculates the starting coordinates for an items entrance.
        /// </summary>        
        /// <param name="child">The item to be added to the panel.</param>
        /// <returns>Point from which the items is inserted from.</returns>
        private Point GetEntryPosition(FrameworkElement child)
        {
            Point entryPoint = new Point(0, 0);

            switch (this.startPosition)
            {
                default:
                    entryPoint = new Point(0, 0);
                    break;
                case EntranceStartPosition.TopLeft:
                    entryPoint = new Point(0, 0);
                    break;
                case EntranceStartPosition.TopCentre:
                    entryPoint = new Point((this.internalWidth / 2) - (child.DesiredSize.Width / 2), 0);
                    break;
                case EntranceStartPosition.TopRight:
                    entryPoint = new Point((this.internalWidth - child.DesiredSize.Width), 0);
                    break;
                case EntranceStartPosition.MiddleLeft:
                    entryPoint = new Point(0, (this.internalHeight / 2) - (child.DesiredSize.Height / 2));
                    break;
                case EntranceStartPosition.MiddleCentre:
                    entryPoint = new Point((this.internalWidth / 2) - (child.DesiredSize.Width / 2), (this.internalHeight / 2) - (child.DesiredSize.Height / 2));
                    break;
                case EntranceStartPosition.MiddleRight:
                    entryPoint = new Point((this.internalWidth - child.DesiredSize.Width), (this.internalHeight / 2) - (child.DesiredSize.Height / 2));
                    break;
                case EntranceStartPosition.BottomLeft:
                    entryPoint = new Point(0, (this.internalHeight + child.DesiredSize.Height));
                    break;
                case EntranceStartPosition.BottomCentre:
                    entryPoint = new Point((this.internalWidth / 2) - (child.DesiredSize.Width / 2), (this.internalHeight - child.DesiredSize.Height));
                    break;
                case EntranceStartPosition.BottomRight:
                    entryPoint = new Point((this.internalWidth - child.DesiredSize.Width), (this.internalHeight - child.DesiredSize.Height));
                    break;
            }

            return entryPoint;
        }

        /// <summary>
        /// Invalidates the layout of the panel when the panel size has been changed.
        /// </summary>
        /// <param name="sender">The AnimatedLayoutPanel.</param>
        /// <param name="e">SizeChanged Event Arguments.</param>
        private void AnimatedLayoutPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion
    }
}