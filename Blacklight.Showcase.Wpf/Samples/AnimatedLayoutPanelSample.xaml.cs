//-----------------------------------------------------------------------
// <copyright file="AnimatedLayoutPanelSample.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>01-Dec-2008</date>
// <author>Mike Parker</author>
// <summary>The animated layout sample.</summary>
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
    using System.Windows.Media.Imaging;
    using System.Text;

    /// <summary>
    /// The animated layout sample.
    /// </summary>
    public partial class AnimatedLayoutPanelSample : UserControl
    {
        /// <summary>
        /// AnimatedLayoutSample constructor.
        /// </summary>
        public AnimatedLayoutPanelSample()
        {
            InitializeComponent();

            // Set Control Type ComboBox Items
            this.controlTypeComboBox.Items.Clear();
            this.controlTypeComboBox.Items.Add("Image");
            this.controlTypeComboBox.Items.Add("Button");
            this.controlTypeComboBox.Items.Add("TextBox");
            this.controlTypeComboBox.Items.Add("Canvas");
            this.controlTypeComboBox.SelectedIndex = 0;

            // Set EntranceStartPosition ComboBox Items
            this.entranceStartPosition.Items.Clear();
            this.entranceStartPosition.Items.Add("TopLeft");
            this.entranceStartPosition.Items.Add("TopCentre");
            this.entranceStartPosition.Items.Add("TopRight");
            this.entranceStartPosition.Items.Add("MiddleLeft");
            this.entranceStartPosition.Items.Add("MiddleCentre");
            this.entranceStartPosition.Items.Add("MiddleRight");
            this.entranceStartPosition.Items.Add("BottomLeft");
            this.entranceStartPosition.Items.Add("BottomCentre");
            this.entranceStartPosition.Items.Add("BottomRight");
            this.entranceStartPosition.SelectedIndex = 8;

            // Set WrapDirection ComboBox Items
            this.wrapDirectionComboBox.Items.Clear();
            this.wrapDirectionComboBox.Items.Add("Horizontal");
            this.wrapDirectionComboBox.Items.Add("Vertical");
            this.wrapDirectionComboBox.SelectedIndex = 0;

            // Set the animation speed
            this.layoutPanel.TransitionDuration = TimeSpan.FromMilliseconds(200);
        }

        /// <summary>
        /// Adds a new control to the panel.
        /// </summary>
        /// <param name="sender">The add control button.</param>
        /// <param name="e">Button clicked event args.</param>
        private void AddControlButton_Click(object sender, RoutedEventArgs e)
        {
            // Add specified control if there are less than 20 items.
            if (this.layoutPanel.Children.Count < 20)
            {
                switch (this.controlTypeComboBox.SelectedItem.ToString())
                {
                    case "Image":
                        this.AddImage();
                        break;
                    case "Button":
                        this.AddButton();
                        break;
                    case "TextBox":
                        this.AddTextBox();
                        break;
                    case "Canvas":
                        this.AddCanvas();
                        break;
                    default:
                        break;
                }

                // Disable the add control button if there are 20 items in the panel.
                if (this.layoutPanel.Children.Count == 20)
                {
                    this.addControlButton.IsEnabled = false;
                }

                this.removeControlButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Removes the first control in the panel.
        /// </summary>
        /// <param name="sender">The remove control button.</param>
        /// <param name="e">Routed event args.</param>
        private void RemoveControlButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel.Children.Count > 0 && this.layoutPanel.Children.Count != 0)
            {
                this.layoutPanel.Children.RemoveAt(0);
                this.addControlButton.IsEnabled = true;
            }
            else
            {
                this.removeControlButton.IsEnabled = false;
            }
        }

        #region Add Items Methods

        /// <summary>
        /// Adds a new image instance to the panel.
        /// </summary>
        private void AddImage()
        {            
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage(new Uri("/Images/placeholder.jpg", UriKind.RelativeOrAbsolute));
            image.Source = bitmapImage;
            image.Width = double.Parse(this.originalWidth.Text);
            image.Height = double.Parse(this.originalHeight.Text);
            image.Margin = this.GetThickness(this.originalMargin.Text);
            image.Stretch = Stretch.Fill;
            this.layoutPanel.Children.Add(image);
        }

        /// <summary>
        /// Adds a new button instance to the panel.
        /// </summary>
        private void AddButton()
        {
            Button button = new Button();
            button.Width = double.Parse(this.originalWidth.Text);
            button.Height = double.Parse(this.originalHeight.Text);
            button.Margin = this.GetThickness(this.originalMargin.Text);
            this.layoutPanel.Children.Add(button);
        }

        /// <summary>
        /// Adds a new textBox instance to the panel.
        /// </summary>
        private void AddTextBox()
        {
            TextBox textBox = new TextBox();
            textBox.Width = double.Parse(this.originalWidth.Text);
            textBox.Height = double.Parse(this.originalHeight.Text);
            textBox.Margin = this.GetThickness(this.originalMargin.Text);
            this.layoutPanel.Children.Add(textBox);
        }

        /// <summary>
        /// Adds a new Canvas instance to the panel.
        /// </summary>
        private void AddCanvas()
        {
            Canvas canvas = new Canvas();
            canvas.Background = new SolidColorBrush(Colors.DarkGray);
            canvas.Width = double.Parse(this.originalWidth.Text);
            canvas.Height = double.Parse(this.originalHeight.Text);
            canvas.Margin = this.GetThickness(this.originalMargin.Text);
            this.layoutPanel.Children.Add(canvas);
        }

        /// <summary>
        /// Converts a string value into a Thickness type.
        /// </summary>
        /// <param name="margin">String representation of the margin.</param>
        /// <returns>Thickness value for the margin.</returns>
        private Thickness GetThickness(string margin)
        {
            Thickness originalMargin = new Thickness();
            double left = 0;
            double top = 0;            
            double right = 0;
            double bottom = 0;
            int index = 0;
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                for (int i = 0; i < margin.Length; i++)
                {
                    if (margin[i].ToString() != "," && i != margin.Length - 1)
                    {
                        stringBuilder.Append(margin[i].ToString());
                    }
                    else
                    {
                        if (i == margin.Length - 1)
                        {
                            stringBuilder.Append(margin[i].ToString());
                        }

                        switch (index)
                        {
                            case 0:
                                left = double.Parse(stringBuilder.ToString());
                                break;
                            case 1:
                                top = double.Parse(stringBuilder.ToString());
                                break;
                            case 2:
                                right = double.Parse(stringBuilder.ToString());
                                break;
                            case 3:
                                bottom = double.Parse(stringBuilder.ToString());
                                break;
                            default:
                                break;
                        }

                        index++;
                        stringBuilder = new StringBuilder();
                    }
                }

                originalMargin = new Thickness(left, top, right, bottom);
            }
            catch (Exception ex)
            {
                originalMargin = new Thickness(0);
                Console.WriteLine(ex.Message);
            }

            return originalMargin;
        }

        #endregion

        #region Sample Page Events

        /// <summary>
        /// Sets the ItemMarginEnabled property to true.
        /// </summary>
        /// <param name="sender">The margin override enabled checkbox.</param>
        /// <param name="e">Routed events args.</param>
        private void ItemMarginEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.layoutPanel.ItemMarginEnabled = true;
            }
        }        

        /// <summary>
        /// Sets the ItemMarginEnabled property to false.
        /// </summary>
        /// <param name="sender">The margin override enabled checkbox.</param>
        /// <param name="e">Routed events args.</param>
        private void ItemMarginEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.layoutPanel.ItemMarginEnabled = false;
            }
        }

        /// <summary>
        /// Updates the ItemMargin property.
        /// </summary>
        /// <param name="sender">The ItemMargin TextBox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void ItemMargin_LostFocus(object sender, RoutedEventArgs e)
        {
            this.layoutPanel.ItemMargin = this.GetThickness(this.itemMargin.Text);
        }

        /// <summary>
        /// Sets the ItemResizeEnabled property to true.
        /// </summary>
        /// <param name="sender">The item resize enabled checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void ItemResizeEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.layoutPanel.ItemResizeEnabled = true;
            }
        }

        /// <summary>
        /// Sets the ItemResizeEnabled property to true.
        /// </summary>
        /// <param name="sender">The item resize enabled checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void ItemResizeEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.layoutPanel.ItemResizeEnabled = false;
            }
        }

        /// <summary>
        /// Updates the ItemResizeWidth property.
        /// </summary>
        /// <param name="sender">The ItemResizeWidth TextBox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void ItemWidth_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                this.layoutPanel.ItemResizeWidth = double.Parse(this.itemWidth.Text);
            }
            catch (Exception ex)
            {
                if (this.layoutPanel != null)
                {
                    this.itemWidth.Text = this.layoutPanel.ItemResizeWidth.ToString();
                }
            }
        }

        /// <summary>
        /// Updates the ItemResizeHeight property.
        /// </summary>
        /// <param name="sender">The ItemResizeHeight TextBox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void ItemHeight_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                this.layoutPanel.ItemResizeHeight = double.Parse(this.itemHeight.Text);
            }
            catch (Exception ex)
            {
                if (this.layoutPanel != null)
                {
                    this.itemHeight.Text = this.layoutPanel.ItemResizeHeight.ToString();
                }
            }
        }

        /// <summary>
        /// Sets the EntranceAnimationEnabled property to true.
        /// </summary>
        /// <param name="sender">The entrance animation enabled checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void EntranceAnimationsEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.layoutPanel.EntranceAnimationEnabled = true;
            }
        }

        /// <summary>
        /// Sets the EntranceAnimationEnabled property to false.
        /// </summary>
        /// <param name="sender">The entrance animation enabled checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void EntranceAnimationsEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.layoutPanel.EntranceAnimationEnabled = false;
            }
        }

        /// <summary>
        /// Updates the EntranceStartPosition property.
        /// </summary>
        /// <param name="sender">The entrance start position combobox control.</param>
        /// <param name="e">Selection Changed Event Args.</param>
        private void EntranceStartPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (this.entranceStartPosition.SelectedItem.ToString())
                {
                    case "TopLeft":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.TopLeft;
                        break;
                    case "TopCentre":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.TopCentre;
                        break;
                    case "TopRight":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.TopRight;
                        break;
                    case "MiddleLeft":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.MiddleLeft;
                        break;
                    case "MiddleCentre":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.MiddleCentre;
                        break;
                    case "MiddleRight":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.MiddleRight;
                        break;
                    case "BottomLeft":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.BottomLeft;
                        break;
                    case "BottomCentre":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.BottomCentre;
                        break;
                    case "BottomRight":
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.BottomRight;
                        break;
                    default:
                        this.layoutPanel.EntranceStartPosition = Blacklight.Controls.Wpf.EntranceStartPosition.BottomRight;
                        break;
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Updates the WrapDirection property.
        /// </summary>
        /// <param name="sender">The wrap direction combobox control.</param>
        /// <param name="e">Selection Changed Event Args.</param>
        private void WrapDirectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (this.wrapDirectionComboBox.SelectedItem.ToString())
                {
                    case "Horizontal":
                        this.layoutPanel.WrapDirection = Blacklight.Controls.Wpf.WrapDirection.Horizontal;
                        break;
                    case "Vertical":
                        this.layoutPanel.WrapDirection = Blacklight.Controls.Wpf.WrapDirection.Vertical;
                        break;
                    default:
                        this.layoutPanel.WrapDirection = Blacklight.Controls.Wpf.WrapDirection.Horizontal;
                        break;
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Updates the width of the panel container.
        /// </summary>
        /// <param name="sender">The panel width slider control.</param>
        /// <param name="e">Routed Event Args.</param>
        private void PanelWidthSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.panelContainer.Width = this.panelWidthSlider.Value;                
            }
        }

        /// <summary>
        /// Updates the height of the panel container.
        /// </summary>
        /// <param name="sender">The panel height slider control.</param>
        /// <param name="e">Routed Event Args.</param>
        private void PanelHeightSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (this.layoutPanel != null)
            {
                this.panelContainer.Height = this.panelHeightSlider.Value;                
            }
        }                   

        /// <summary>
        /// Shows the horizontal scrollbar on the host scrollviewer control.
        /// </summary>
        /// <param name="sender">The horizontal scrollbars checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void HorizontalScrollbars_Checked(object sender, RoutedEventArgs e)
        {
            if (this.panelScrollViewer != null)
            {
                this.panelScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
        }

        /// <summary>
        /// Hides the horizontal scrollbar on the host scrollviewer control.
        /// </summary>
        /// <param name="sender">The horizontal scrollbars checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void HorizontalScrollbars_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.panelScrollViewer != null)
            {
                this.panelScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }

        /// <summary>
        /// Shows the vertical scrollbar on the host scrollviewer control.
        /// </summary>
        /// <param name="sender">The vertical scrollbars checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void VerticalScrollbars_Checked(object sender, RoutedEventArgs e)
        {
            if (this.panelScrollViewer != null)
            {
                this.panelScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
        }

        /// <summary>
        /// Hides the vertical scrollbar on the host scrollviewer control.
        /// </summary>
        /// <param name="sender">The vertical scrollbars checkbox.</param>
        /// <param name="e">Routed Event Args.</param>
        private void VerticalScrollbars_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.panelScrollViewer != null)
            {
                this.panelScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }

        #endregion
    }
}
