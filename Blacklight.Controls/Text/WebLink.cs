//-----------------------------------------------------------------------
// <copyright file="WebLink.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>A control that displays text, and when clicked, navigates somehwere else on the web.</summary>
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
    /// A control that displays text, and when clicked, navigates somehwere else on the web.
    /// </summary>
    public class WebLink : Control
    {
        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WebLink), null);

        /// <summary>
        /// The text decorations property.
        /// </summary>
        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(WebLink), null);

        /// <summary>
        /// The text wrapping property.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(WebLink), null);

        /// <summary>
        /// The link Uri property.
        /// </summary>
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(WebLink), null);

        /// <summary>
        /// Stores the target.
        /// </summary>
        private string target = "_blank";

        /// <summary>
        /// Web link constructor.
        /// </summary>
        public WebLink()
        {
            this.DefaultStyleKey = typeof(WebLink);
            this.MouseEnter += new MouseEventHandler(this.WebLink_MouseEnter);
            this.MouseLeave += new MouseEventHandler(this.WebLink_MouseLeave);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(this.WebLink_MouseLeftButtonUp);
        }

        /// <summary>
        /// Gets or sets the link text.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Teh text content property.")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text decorations.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text decoration property.")]
        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text wrapping.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text wrapping property.")]
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The web Uri property.")]
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        /// <summary>
        /// Gets or sets the link target.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The web target property.")]
        public string Target
        {
            get { return this.target; }
            set { this.target = value; }
        }

        /// <summary>
        /// Navigates to the uri.
        /// </summary>
        /// <param name="sender">The web link.</param>
        /// <param name="e">Mouse button event args.</param>
        private void WebLink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Uri))
            {
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(this.Uri, UriKind.RelativeOrAbsolute), this.target);
            }
        }

        /// <summary>
        /// Removes the underline.
        /// </summary>
        /// <param name="sender">The web link.</param>
        /// <param name="e">Mouse event args.</param>
        private void WebLink_MouseLeave(object sender, MouseEventArgs e)
        {
            this.TextDecorations = null;
        }

        /// <summary>
        /// Underlines the text.
        /// </summary>
        /// <param name="sender">The web link.</param>
        /// <param name="e">Mouse event args.</param>
        private void WebLink_MouseEnter(object sender, MouseEventArgs e)
        {
            this.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
}
