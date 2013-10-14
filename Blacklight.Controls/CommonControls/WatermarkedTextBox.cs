//-----------------------------------------------------------------------
// <copyright file="WatermarkedTextBox.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>25-June-2009</date>
// <author>Mike Parker</author>
// <summary>Represents a control that can be used to display or edit unformatted text whilst providing subtle help text where no text has been entered.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
{
    #region Using...

    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;    

    #endregion

    /// <summary>
    /// A TextBox control that provides subtle help text when no text is currently entered.
    /// </summary>    
    [TemplatePart(Name = WatermarkedTextBox.PARTWatermark, Type = typeof(ContentControl))]
    public class WatermarkedTextBox : TextBox
    {
        #region Dependency Properties      

        /// <summary>
        /// The Watermark Property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(object), typeof(WatermarkedTextBox), null);

        /// <summary>
        /// WatermarkTemplate Property.
        /// </summary>
        public static readonly DependencyProperty WatermarkTemplateProperty =
            DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(WatermarkedTextBox), new PropertyMetadata(null));

        #endregion

        #region Private Members

        /// <summary>
        /// Determines the name of the WatermarkedTextBox templated part.
        /// </summary>
        private const string PARTWatermark = "Watermark";
        
        /// <summary>
        /// Indicates whether the control currently has focus.
        /// </summary>
        private bool hasFocus;

        /// <summary>
        /// The templated part that shows the WatermarkContent.
        /// </summary>
        private ContentControl watermarkPresenter;        

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the WatermarkedTextBox class.
        /// </summary>
        public WatermarkedTextBox()
        {
            // Set default values
            this.DefaultStyleKey = typeof(WatermarkedTextBox);            
            this.Loaded += new RoutedEventHandler(this.WatermarkedTextBox_Loaded);
        }

        #endregion

        #region Public Properties         

        /// <summary>
        /// Gets or sets the WatermarkTemplate (this defines how the Watermark will appear when no text is entered and the control does not have focus).
        /// </summary>
        [System.ComponentModel.Category("Miscellaneous"), System.ComponentModel.Description("Gets or sets the WatermarkTemplate (this defines how the Watermark will appear when no text is entered and the control does not have focus).")]
        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Watermark that will appear when no text is entered and the control does not have focus.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Gets or sets the Watermark that will appear when no text is entered and the control does not have focus.")]
        public object Watermark
        {
            get { return (object)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        #endregion      

        #region Override Methods

        /// <summary>
        /// Retrieves the specific Templated components that are required once a Template has been applied to the control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.watermarkPresenter = GetTemplateChild(PARTWatermark) as ContentControl;            
            this.CheckText();
        }

        #endregion

        #region Private Methods  
        /// <summary>
        /// Checks to determine if the Watermark should be displayed.
        /// </summary>
        private void CheckText()
        {
            if (this.Text.Length > 0)
            {
                this.watermarkPresenter.Visibility = Visibility.Collapsed;
            }
            else if (!this.hasFocus)
            {
                this.watermarkPresenter.Visibility = Visibility.Visible;
            }
            else
            {
                this.watermarkPresenter.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Initializes event handlers for the control once it has loaded.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void WatermarkedTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextChanged += new TextChangedEventHandler(this.WatermarkedTextBox_TextChanged);
            this.GotFocus += new RoutedEventHandler(this.WatermarkedTextBox_GotFocus);
            this.LostFocus += new RoutedEventHandler(this.WatermarkedTextBox_LostFocus);            
        }       

        /// <summary>
        /// Checks whether the WatermarkContent should appear following an update to the Text.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">TextChanged Event Arguments.</param>
        private void WatermarkedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.CheckText();
        }

        /// <summary>
        /// Checks whether the WatermarkContent should appear once the control has lost focus.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void WatermarkedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.hasFocus = false;
            this.CheckText();
        }

        /// <summary>
        /// Checks whether the WatermarkContent should appear once the control has received focus.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void WatermarkedTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.hasFocus = true;
            this.CheckText();
        }

        #endregion       
    }
}