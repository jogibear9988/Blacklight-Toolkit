//-----------------------------------------------------------------------
// <copyright file="GlassBorder.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>03-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>A border that also shows a glass effect.</summary>
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
    /// A border that also shows a glass effect.
    /// </summary>
    public class GlassBorder : ContentControl
    {
        /// <summary>
        /// The flass opacity property.
        /// </summary>
        public static readonly DependencyProperty GlassOpacityProperty =
            DependencyProperty.Register("GlassOpacity", typeof(double), typeof(GlassBorder), null);

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(GlassBorder), null);

        /// <summary>
        /// The glass corner radius property.
        /// </summary>
        public static readonly DependencyProperty GlassCornerRadiusProperty =
            DependencyProperty.Register("GlassCornerRadius", typeof(CornerRadius), typeof(GlassBorder), null);

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(GlassBorder), null);

        /// <summary>
        /// The content z-index property.
        /// </summary>
        public static readonly DependencyProperty ContentZIndexProperty =
            DependencyProperty.Register("ContentZIndex", typeof(int), typeof(GlassBorder), null);

        /// <summary>
        /// GlassBorder constructor.
        /// </summary>
        public GlassBorder()
        {
            this.DefaultStyleKey = typeof(GlassBorder);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content is clipped.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool ClipContent
        {
            get { return (bool)GetValue(ClipContentProperty); }
            set { SetValue(ClipContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content z-index. 0 for behind the glass, 1 for in-front.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Set 0 for behind the shadow, 1 for in front.")]
        public int ContentZIndex
        {
            get { return (int)GetValue(ContentZIndexProperty); }
            set { SetValue(ContentZIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner glow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow opacity.")]
        public double GlassOpacity
        {
            get { return (double)GetValue(GlassOpacityProperty); }
            set { SetValue(GlassOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border corner radius.
        /// This is a thickness, as there is a problem parsing CornerRadius types.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }

            set
            {
                SetValue(CornerRadiusProperty, value);

                CornerRadius glassCornerRadius = new CornerRadius(
                        Math.Max(0, value.TopLeft - 1),
                        Math.Max(0, value.TopRight - 1),
                        0,
                        0);
                SetValue(GlassCornerRadiusProperty, glassCornerRadius);
            }
        }

        /// <summary>
        /// Gets the template parts out.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public double GlassCornerRadius
        {
            get { return (double)GetValue(GlassCornerRadiusProperty); }
        }
    }
}
