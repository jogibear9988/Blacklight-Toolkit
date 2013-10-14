//-----------------------------------------------------------------------
// <copyright file="Page.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>The demo main page.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Showcase
{
    using Blacklight.Showcase.Samples;
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
    using System.Collections.ObjectModel;


    /// <summary>
    /// The demo main page.
    /// </summary>
    public partial class Page : UserControl
    {
        /// <summary>
        /// Stores a list of menu radio buttons.
        /// </summary>
        private List<RadioButton> menuRadioButtons = new List<RadioButton>();

        /// <summary>
        /// Stores a list of the naviation radio buttons.
        /// </summary>
        private List<RadioButton> navigationRadioButtons = new List<RadioButton>();

        /// <summary>
        /// Flag for whether this is the first time the navigation has loaded.
        /// </summary>
        private bool firstNavigationLoad;

        /// <summary>
        /// Flag for whether this is the first time a menu has loaded.
        /// </summary>
        private bool firstMenuLoad;

        /// <summary>
        /// The page constructor.
        /// </summary>
        public Page()
        {
            InitializeComponent();
            this.GenerateShowcaseNavigation();
            this.SizeChanged += new SizeChangedEventHandler(this.Page_SizeChanged);
        }

        /// <summary>
        /// Stores each radio button and checks the 'ABOUT' option.
        /// </summary>
        /// <param name="sender">The loaded radio button.</param>
        /// <param name="e">Routed event args.</param>
        private void NavigationItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.navigationRadioButtons.Add((RadioButton)sender);
            NavigationItem navigationItem = (NavigationItem)((FrameworkElement)sender).DataContext;
            if (navigationItem.Name == "ABOUT" && !this.firstNavigationLoad)
            {
                this.firstNavigationLoad = true;
                ((RadioButton)sender).IsChecked = true;
            }
        }

        /// <summary>
        /// Selects the navigation and presents the menu. Also unchecks the other navigation items.
        /// </summary>
        /// <param name="sender">The checked radio button.</param>
        /// <param name="e">Routed event args.</param>
        private void NavigationItem_Checked(object sender, RoutedEventArgs e)
        {
            NavigationItem navigationItem = (NavigationItem)((FrameworkElement)sender).DataContext;

            this.menuItems.ItemsSource = navigationItem.Menus;
        }

        /// <summary>
        /// Selects the menu item.
        /// </summary>
        /// <param name="sender">The source menu item.</param>
        /// <param name="e">Routed event args.</param>
        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            this.sampleRadioButton.IsChecked = true;
            RadioButton senderRadioButton = (RadioButton)sender;

            foreach (RadioButton menuRadioButton in this.menuRadioButtons)
            {
                if (senderRadioButton != menuRadioButton)
                {
                    ((Sample)menuRadioButton.DataContext).SamplePage.Visibility = Visibility.Collapsed;
                }
            }

            Sample sample = (Sample)senderRadioButton.DataContext;
            sample.SamplePage.Visibility = Visibility.Visible;
            this.selectedSampleGrid.DataContext = sample;
        }

        /// <summary>
        /// Hides the sample.
        /// </summary>
        /// <param name="sender">The menu item.</param>
        /// <param name="e">Routed event args.</param>
        private void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            Sample sample = (Sample)((FrameworkElement)sender).DataContext;
            if (sample != null)
            {
                if (sample.SamplePage != null)
                {
                    sample.SamplePage.Visibility = Visibility.Collapsed;
                }

                if (sample.CodePage != null)
                {
                    sample.CodePage.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// If its the introdution button, check it.
        /// </summary>
        /// <param name="sender">A menu item button.</param>
        /// <param name="e">Routed event args.</param>
        private void MenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            RadioButton senderRadioButton = (RadioButton)sender;

            bool alreadyAdded = false;
            for (int i = 0; i < this.menuRadioButtons.Count; i++)
            {
                if (this.menuRadioButtons[i].DataContext == senderRadioButton.DataContext)
                {
                    this.menuRadioButtons[i] = senderRadioButton;
                    alreadyAdded = true;
                }
            }

            if (!alreadyAdded)
            {
                this.menuRadioButtons.Add(senderRadioButton);
            }

            Sample sample = (Sample)senderRadioButton.DataContext;
            if (sample.Name == "Introduction" && !this.firstMenuLoad)
            {
                this.firstMenuLoad = true;
                senderRadioButton.IsChecked = true;
            }
        }

        /// <summary>
        /// Stores the sample in a list.
        /// </summary>
        /// <param name="sender">The sample page.</param>
        /// <param name="e">Routed event args.</param>
        private void Sample_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement sample = (FrameworkElement)sender;
            sample.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows the current sample page.
        /// </summary>
        /// <param name="sender">The sample radio button.</param>
        /// <param name="e">Routed event args.</param>
        private void SampleRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Sample sample = (Sample)((FrameworkElement)sender).DataContext;

            if (sample != null)
            {
                if (sample.SamplePage != null)
                {
                    sample.SamplePage.Visibility = Visibility.Visible;
                }

                if (sample.CodePage != null)
                {
                    sample.CodePage.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Shows the code page.
        /// </summary>
        /// <param name="sender">The code radio button.</param>
        /// <param name="e">Routed event args.</param>
        private void CodeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Sample sample = (Sample)((FrameworkElement)sender).DataContext;

            if (sample != null)
            {
                if (sample.SamplePage != null)
                {
                    sample.SamplePage.Visibility = Visibility.Collapsed;
                }

                if (sample.CodePage != null)
                {
                    sample.CodePage.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Updates the menu scrollers max size.
        /// </summary>
        /// <param name="sender">The page sender.</param>
        /// <param name="e">Size Changed Event args</param>
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.menuItemsScrollViewer.MaxHeight = e.NewSize.Height - 200;
        }

        /// <summary>
        /// Generates the navigation for the showcase.
        /// </summary>
        private void GenerateShowcaseNavigation()
        {
            Collection<NavigationItem> navigationItems = new Collection<NavigationItem>();

            navigationItems.Add(new NavigationItem()
            {
                Name = "ABOUT",
                Menus = new Collection<NavigationMenu>()
                {
                    new NavigationMenu()
                    {
                        Name = "M E N U",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Introduction",
                                Description = "What is Blacklight? Who is it for? How do I get it?",
                                SamplePage = this.introduction
                            },
                            new Sample()
                            {
                                Name = "Release Notes",
                                Description = "A list of the changes in Blacklight since release v4.0 (Jul09).",
                                SamplePage = this.releaseNotes
                            },
                            new Sample()
                            {
                                Name = "Who Is Involved?",
                                Description = "Meet the people behind Blacklight!",
                                SamplePage = this.whoIsInvolved
                            },
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "G U I D E S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Blacklight + Blend",
                                Description = "A guide for using Blacklight in Expression Blend!",
                                SamplePage = this.blacklightBlend
                            },
                            new Sample()
                            {
                                Name = "Blacklight + VS 2008",
                                Description = "A guide for using Blacklight in Visual Studio 2008!",
                                SamplePage = this.blacklightVs
                            }
                        }
                    }
                }
            });

            navigationItems.Add(new NavigationItem()
            {
                Name = "VISUAL CONTROLS",
                Menus = new Collection<NavigationMenu>()
                {
                    new NavigationMenu()
                    {
                        Name = "B O R D E R S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Clipping Border",
                                Description = "The clipping border is a border, similar to the Silverlight Border, that allows you to clip the content. With a regular border, the content inside can leak out of the edges, particularly when you have rounded corners. The clipping border allows to you clip the content inside the bounds of the border.",
                                SamplePage = this.clippingBorderSample
                            },
                            new Sample()
                            {
                                Name = "Drop Shadow Border",
                                Description = "The drop shadow border is a content control similar to the Border, however, this one has a drop shadow effect. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.dropShadowBorderSample
                            },
                            new Sample()
                            {
                                Name = "Glass Border",
                                Description = "The glass border is a content control similar to the Border, however, this one has an glassy effect in the top half. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.glassBorderBlockSample
                            },
                            new Sample()
                            {
                                Name = "Inner Glow Border",
                                Description = "The inner glow border is a content control similar to the Border, however, this one has an inner glow effect around the edge. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.innerGlowBorderSample
                            },
                            new Sample()
                            {
                                Name = "Outer Glow Border",
                                Description = "The outer glow border is a content control similar to the Border, however, this one has an outer glow effect around the edge. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.outerGlowBorderSample
                            },
                            new Sample()
                            {
                                Name = "Perspective Shadow Border",
                                Description = "The perspective shadow border is a content control similar to the Border, however, this one has a drop shadow effect with perspective. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.perspectiveShadowBorderBlockSample
                            },
                            new Sample()
                            {
                                Name = "Radial Shadow Border",
                                Description = "The radial shadow border is a content control similar to the Border, however, this one has a radial drop shadow effect. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.radialShadowBorderSample
                            },
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "T E X T",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Drop Shadow Text Block",
                                Description = "The drop shadow text block is a control, similar to TextBlock, that shows a drop shadow. You can control the angle, distance, opacity and color of the drop shadow. Use the controls in the sample to adjust some of the values to see the result.",
                                SamplePage = this.dropShadowTextBlockSample
                            },
                            new Sample()
                            {
                                Name = "Stroke Text Block (Alpha)",
                                Description = "The stroke text block is a control, similar to TextBlock, that shows a stroke around the text. This is an early version of this control (an Alpha release), and should not be used heavily, otherwise application performance can be affected. The lower the stroke thickness, the better. You can control the stroke brush, opacity and thickness. Use the controls in the sample to adjust some of the values to see the result.",
                                SamplePage = this.strokeTextBlockSample
                            },
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "S T Y L E S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Dave's Glossy Controls",
                                Description = "Dave's pleased to announce the availability of his first Silverlight controls set: 'Dave's Glossy Controls for Silverlight'.",
                                Link = "http://blogs.msdn.com/dave/archive/2008/10/06/dave-s-glossy-controls-for-silverlight-2-released.aspx",
                                SamplePage = this.daveGlossyControlsSample
                            }
                        }
                    },
                }
            });

            navigationItems.Add(new NavigationItem()
            {
                Name = "INTERACTIVE CONTROLS",
                Menus = new Collection<NavigationMenu>()
                {
                    new NavigationMenu()
                    {
                        Name = "D A S H B O A R D S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Drag Dock Panel",
                                Description = "The DragDockPanel and DragDockPanelHost are two controls that allow you to create a customisable dashboard. You can place panels on the page, and they will become draggable and dockable. You can also 'maximise' a panel which will stack all the other panels on the right hand size, allowing you to focus on the selected panel. You can also dynamically add and remove panels from the host.",
                                Link = "http://blogs.msdn.com/mgrayson/archive/2008/08/29/silverlight-2-samples-dragging-docking-expanding-panels-part-3.aspx",
                                SamplePage = this.dragDockPanelSample
                            },
                             new Sample()
                            {
                                Name = "Drag Dock Panel From ViewModel",
                                Description = "This is the same as above, however using a VM and a Item Template to create the Panel",
                                Link = "http://blacklight.codeplex.com",
                                SamplePage = this.dragDockPanelWithViewModelSample
                            }
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "M E D I A",
                        Samples = new Collection<Sample>()
                        {
                             new Sample()
                            {
                                Name = "Simple Media Player",
                                Description = "This is the media player I use for my blog. I wanted something really light weight, very simple controls, and that would look cool embedded into a page. There is a 'click to play' button at the start which prevents the media from downloading (unless the user wants it!) and when you wiggle your mouse over the video, you will see some more controls. Double clicking on the video will toggle fullscreen.",
                                Link = "http://blogs.msdn.com/mgrayson/archive/2008/08/20/my-new-silverlight-media-player-for-my-blog.aspx",
                                SamplePage = this.mediaPlayerSample
                            }
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "P A N E L S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Animated Layout Panel",
                                Description = "The AnimatedLayoutPanel dynamically repositions its child elements sequentially either from left to right or top to bottom depending on the value of the WrapDirection property. Content will be rendered on a new row or column when the content reaches the edge of the panel. When the layout is updated a subtle animation is used to move the element to its new position.",
                                Link = "http://blogs.msdn.com/mgrayson/archive/2009/02/20/animated-layout-panel-from-the-blacklight-controls.aspx",
                                SamplePage = this.animatedLayoutPanel,
                                CodePage = this.animatedLayoutPanelCodePage
                            }
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "O T H E R",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Animated Expander",
                                Description = "The Animated Expander control is ideal for lists that contain a lot of information. The control presents a header that when clicked on, expands to reveal more content. Click on the headers below to see.",
                                SamplePage = this.animatedExpanderSample
                            },
                            new Sample()
                            {
                                Name = "Loading Animation",
                                Description = "The Loading Animation is a simple animation that you can display whilst some thing is loading, or you are waiting for data to be retrieved, or a video is buffering etc. You can also place any content you wish below the animation.",
                                SamplePage = this.loadingAnimationSample
                            },
                            new Sample()
                            {
                                Name = "Range Slider",
                                Description = "The Range Slider control is a 'double headed' slider that allows you to select a range of values rather than just one value. Use the thumbs at either end of the slider to select the sie of the range. Click and drag the center of the slider to move the range.",
                                SamplePage = this.rangeSliderSample
                            },
                            new Sample()
                            {
                                Name = "Watermarked Text Box",
                                Description = "The Watermarked Text Box control is a text box that displays a watermark when there is no content. The watermark can be any object, and the display can be customised by providing a Watermark Template. Some example watermarks have been provided below.",
                                SamplePage = this.watermarkedTextBoxSample
                            }
                        }
                    }
                }
            });

            navigationItems.Add(new NavigationItem()
            {
                Name = "CONNECTED CONTROLS",
                Menus = new Collection<NavigationMenu>()
                {
                    new NavigationMenu()
                    {
                        Name = "L I S T   B O X E S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Syndication Feed List Box",
                                Description = "The Syndication Feed List Box is a ListBox control that allows binding to an RSS feed. The control will parse the feed and display the contents. The look and feel of the results is completed customisable by editing the ItemTemplate and the ItemContainerStyle. No C# is required to use this control!",
                                SamplePage = this.syndicationFeedListBoxSample
                            },
                            new Sample()
                            {
                                Name = "Live Search List Box",
                                Description = "The Live Search List Box allows you to create any kind of Live search (Web, Image, News etc) and display the results. All that is required is a developer key, from dev.live.com. The look and feel of the results is completed customisable by editing the ItemTemplate and the ItemContainerStyle. No C# is required to use this control!",
                                SamplePage = this.liveSearchListBoxSample
                            },
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "D E E P   Z O O M",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Deep Zoom Viewer",
                                Description = "The Deep Zoom Viewer control allows easy integration of Deep Zoom into an application. The control provides simple mouse navigation including clicking on an image to zoom and mouse wheel interactivity.",
                                SamplePage = this.deepZoomViewerSample
                            },
                        }
                    }
                }
            });

            navigationItems.Add(new NavigationItem()
            {
                Name = "WPF",
                Menus = new Collection<NavigationMenu>()
                {
                    new NavigationMenu()
                    {
                        Name = "B O R D E R S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Clipping Border",
                                Description = "The clipping border is a border, similar to the Silverlight Border, that allows you to clip the content. With a regular border, the content inside can leak out of the edges, particularly when you have rounded corners. The clipping border allows to you clip the content inside the bounds of the border.",
                                SamplePage = this.clippingBorderSample
                            },
                            new Sample()
                            {
                                Name = "Drop Shadow Border",
                                Description = "The drop shadow border is a content control similar to the Border, however, this one has a drop shadow effect. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.dropShadowBorderSample
                            },
                            new Sample()
                            {
                                Name = "Glass Border",
                                Description = "The glass border is a content control similar to the Border, however, this one has an glassy effect in the top half. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.glassBorderBlockSample
                            },
                            new Sample()
                            {
                                Name = "Inner Glow Border",
                                Description = "The inner glow border is a content control similar to the Border, however, this one has an inner glow effect around the edge. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.innerGlowBorderSample
                            },
                            new Sample()
                            {
                                Name = "Outer Glow Border",
                                Description = "The outer glow border is a content control similar to the Border, however, this one has an outer glow effect around the edge. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.outerGlowBorderSample
                            },
                            new Sample()
                            {
                                Name = "Perspective Shadow Border",
                                Description = "The perspective shadow border is a content control similar to the Border, however, this one has a drop shadow effect with perspective. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.perspectiveShadowBorderBlockSample
                            },
                            new Sample()
                            {
                                Name = "Radial Shadow Border",
                                Description = "The radial shadow border is a content control similar to the Border, however, this one has a radial drop shadow effect. The sample below allows you to adjust the various properties to see how the border's appearance can be changed.",
                                SamplePage = this.radialShadowBorderSample
                            },
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "T E X T",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Drop Shadow Text Block",
                                Description = "The drop shadow text block is a control, similar to TextBlock, that shows a drop shadow. You can control the angle, distance, opacity and color of the drop shadow. Use the controls in the sample to adjust some of the values to see the result.",
                                SamplePage = this.dropShadowTextBlockSample
                            },
                        }
                    },
                    new NavigationMenu()
                    {
                        Name = "D A S H B O A R D S",
                        Samples = new Collection<Sample>()
                        {
                            new Sample()
                            {
                                Name = "Drag Dock Panel",
                                Description = "The DragDockPanel and DragDockPanelHost are two controls that allow you to create a customisable dashboard. You can place panels on the page, and they will become draggable and dockable. You can also 'maximise' a panel which will stack all the other panels on the right hand size, allowing you to focus on the selected panel. You can also dynamically add and remove panels from the host.",
                                Link = "http://blogs.msdn.com/mgrayson/archive/2008/08/29/silverlight-2-samples-dragging-docking-expanding-panels-part-3.aspx",
                                SamplePage = this.dragDockPanelSample
                            },
                        }
                    },
                }
            });

            this.navigationItems.ItemsSource = navigationItems;
        }
    }
}
