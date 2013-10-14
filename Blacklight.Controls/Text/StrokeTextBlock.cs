//-----------------------------------------------------------------------
// <copyright file="StrokeTextBlock.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>24-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>A control that displays text, with a stroke.</summary>
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
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// A control that displays text, with a stroke.
    /// </summary>
    public class StrokeTextBlock : Control
    {
        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(TextBlockProperty_Changed)));

        /// <summary>
        /// The text decorations property.
        /// </summary>
        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(TextBlockProperty_Changed)));

        /// <summary>
        /// The text wrapping property.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(TextBlockProperty_Changed)));

        /// <summary>
        /// The line height property.
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(double), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(TextBlockProperty_Changed)));

        /// <summary>
        /// The stroke opacity property.
        /// </summary>
        public static readonly DependencyProperty StrokeOpacityProperty =
            DependencyProperty.Register("StrokeOpacity", typeof(double), typeof(StrokeTextBlock), null);

        /// <summary>
        /// The stroke thickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(StrokeThickness_Changed)));

        /// <summary>
        /// The stroke property,
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(Stroke_Changed)));

        /// <summary>
        /// Stores the stroke items control.
        /// </summary>
        private ItemsControl itemsControl;

        /// <summary>
        /// Stores the stroke text blocks.
        /// </summary>
        private ObservableCollection<TextBlock> strokeTextBlocks = new ObservableCollection<TextBlock>();

        /// <summary>
        /// StrokeTextBlock constructor.
        /// </summary>
        public StrokeTextBlock()
        {
            this.DefaultStyleKey = typeof(StrokeTextBlock);            
        }

        /// <summary>
        /// Gets or sets the drop shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The stroke opacity.")]
        public double StrokeOpacity
        {
            get { return (double)GetValue(StrokeOpacityProperty); }
            set { SetValue(StrokeOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Text"), System.ComponentModel.Description("The text line height.")]
        public double LineHeight
        {
            get { return (double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the link text.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text content.")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text decorations.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text decorations.")]
        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text wrapping.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Whether the text wraps.")]
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        [System.ComponentModel.Category("Brushes"), System.ComponentModel.Description("The text stroke brush.")]
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The text stroke brush.")]
        public double StrokeThickness
        {
            get 
            { 
                return (double)GetValue(StrokeThicknessProperty); 
            }
            
            set 
            { 
                SetValue(StrokeThicknessProperty, Math.Ceiling(value)); 
            }
        }

        /// <summary>
        /// Gets the template parts and prepares the control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.itemsControl = (ItemsControl)this.GetTemplateChild("PART_ItemsControl");

            if (this.itemsControl != null)
            {
                this.itemsControl.ItemsSource = this.strokeTextBlocks;
            }
        }

        /// <summary>
        /// Updates the stoke collection.
        /// </summary>
        internal void UpdateStroke()
        {
            foreach (TextBlock textBlock in this.strokeTextBlocks)
            {
                textBlock.Foreground = this.Stroke;
            }
        }

        /// <summary>
        /// Updates the stoke collection.
        /// </summary>
        internal void UpdateStrokeThickness()
        {
            if (this.StrokeThickness == 0)
            {
                for (int i = 0; i < this.strokeTextBlocks.Count; i++)
                {
                    this.strokeTextBlocks[i].Opacity = 0;
                }
            }
            else
            {
                int strokeTextBlockCount = (int)Math.Ceiling(360 / Math.Floor(45.0 / (this.StrokeThickness / 2)));

                if (this.strokeTextBlocks.Count < strokeTextBlockCount)
                {
                    for (int i = 0; i <= strokeTextBlockCount; i++)
                    {
                        this.AddStrokeTextBlock();
                    }
                }
                else if (this.strokeTextBlocks.Count > strokeTextBlockCount)
                {
                    for (int i = strokeTextBlockCount; i < this.strokeTextBlocks.Count; i++)
                    {
                        this.strokeTextBlocks[i].Opacity = 0;
                    }
                }

                int textBlockIndex = 0;
                for (int a = 0; a < 360; a = a + (int)Math.Floor(45.0 / (this.StrokeThickness / 2)))
                {
                    double x = Math.Cos(a * (Math.PI / 180)) * this.StrokeThickness;
                    double y = Math.Tan(a * (Math.PI / 180)) * x;

                    this.strokeTextBlocks[textBlockIndex].Opacity = 1;
                    this.strokeTextBlocks[textBlockIndex].RenderTransform = new TranslateTransform()
                    {
                        X = x,
                        Y = y
                    };

                    textBlockIndex++;
                }
            }
        }

        /// <summary>
        /// Updates the text blocks.
        /// </summary>
        internal void UpdateTextBlocks()
        {
            foreach (TextBlock textBlock in this.strokeTextBlocks)
            {
                textBlock.HorizontalAlignment = this.HorizontalAlignment;

                if (this.LineHeight > 0)
                {
                    textBlock.LineHeight = this.LineHeight;
                }

                textBlock.Text = this.Text;
                textBlock.TextWrapping = this.TextWrapping;
                textBlock.Foreground = this.Stroke;
            }
        }

        /// <summary>
        /// Updates the stroke thickness.
        /// </summary>
        /// <param name="dependencyObject">The stroke text block.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void StrokeThickness_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            StrokeTextBlock strokeTextBlock = (StrokeTextBlock)dependencyObject;
            strokeTextBlock.UpdateStrokeThickness();
        }

        /// <summary>
        /// Updates the stroke.
        /// </summary>
        /// <param name="dependencyObject">The stroke text block.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void Stroke_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            StrokeTextBlock strokeTextBlock = (StrokeTextBlock)dependencyObject;
            strokeTextBlock.UpdateStroke();
        }

        /// <summary>
        /// Updates the text blocks.
        /// </summary>
        /// <param name="dependencyObject">The stroke text block.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void TextBlockProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            StrokeTextBlock strokeTextBlock = (StrokeTextBlock)dependencyObject;
            strokeTextBlock.UpdateTextBlocks();
        }

        /// <summary>
        /// Adds a stroke text block.
        /// </summary>
        private void AddStrokeTextBlock()
        {
            TextBlock tb = new TextBlock();
            tb.RenderTransformOrigin = new Point(0.5, 0.5);
            tb.HorizontalAlignment = this.HorizontalAlignment;

            if (this.LineHeight > 0)
            {
                tb.LineHeight = this.LineHeight;
            }

            tb.Text = this.Text;
            tb.TextWrapping = this.TextWrapping;
            tb.Foreground = this.Stroke;
            this.strokeTextBlocks.Add(tb);
        }
    }
}
