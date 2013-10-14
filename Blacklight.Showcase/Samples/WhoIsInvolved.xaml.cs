//-----------------------------------------------------------------------
// <copyright file="WhoIsInvolved.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>20-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>The whos involved page.</summary>
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
    using System.Windows.Media.Imaging;

    /// <summary>
    /// The whos involved page.
    /// </summary>
    public partial class WhoIsInvolved : UserControl
    {
        /// <summary>
        /// WhoIsInvolved constructor.
        /// </summary>
        public WhoIsInvolved()
        {
            InitializeComponent();

            this.peopleItemsControl.ItemsSource = new List<Contributor>()
            {
                new Contributor()
                {
                    Name = "Martin Grayson",
                    JobRole = "User Experience Developer",
                    BlacklightRole = "Project Co-ordinator",
                    Blog = "http://blogs.msdn.com/mgrayson",
                    About = "I am a User Experience Developer for Microsoft Consulting Services UK. Over the last 3 years I have focused on Microsoft's latest UI technology offerings including Windows Presentation Foundation (WPF), Silverlight, ASP.NET Ajax and XNA. \r\n \r\nI have a development background, but have always been interested in UI design and development. I have started Blacklight as during my projects in the past, I was regularly re-creating visual effects and interactions for each project, and decided that having these things as re-usable components would be far more useful.\r\n\r\nBlacklight is really a set of controls for designers and everyone who uses Blend. We attempt to remove the need for any code to create effects and interactions that designers use daily in their work.",
                    Image = new BitmapImage(new Uri("/Images/mgrayson.jpg", UriKind.RelativeOrAbsolute))
                },
                new Contributor()
                {
                    Name = "Mike Parker",
                    JobRole = "User Experience Designer",
                    BlacklightRole = "Contributor",
                    Blog = "http://blogs.msdn.com/mparker ",
                    About = "I am a User Experience Designer for Microsoft Consulting Services UK. My background is in interaction design however I have had the privilege over the last couple of years to work with the latest Microsoft UI technologies such as WPF and Silverlight in both a design and development capacity.",
                    Image = new BitmapImage(new Uri("/Images/mparker.png", UriKind.RelativeOrAbsolute))
                },
                new Contributor()
                {
                    Name = "Dave Crawford",
                    JobRole = "User Experience Designer",
                    BlacklightRole = "Contributor",
                    Blog = "http://blogs.msdn.com/dave",
                    About = ""
                },
                new Contributor()
                {
                    Name = "Neil Ashley",
                    JobRole = "Creative Director",
                    BlacklightRole = "Contributor",
                    Blog = "",
                    About = ""
                },
            };
        }
    }
}
