//-----------------------------------------------------------------------
// <copyright file="Sample.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>Class representing an application sample.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Showcase.Wpf
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
    /// Class representing an application sample.
    /// </summary>
    public class Sample
    {
        /// <summary>
        /// Sample constructor.
        /// </summary>
        public Sample()
        {
        }

        /// <summary>
        /// Gets or sets the sample name.
        /// </summary>
        public string Name 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets the link name. (e.g. 'Name' will be ':: Name')
        /// </summary>
        public string LinkName
        {
            get
            {
                return "::  " + this.Name;
            }
        }

        /// <summary>
        /// Gets the formatted name. (e.g. 'Name' will be 'N A M E')
        /// </summary>
        public string FormattedName
        {
            get
            {
                string formattedName = "";
                foreach (char c in this.Name.ToUpper().ToCharArray())
                {
                    formattedName += c;
                    formattedName += ' ';
                }

                return formattedName;
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets the description visibility.
        /// </summary>
        public Visibility DescriptionVisibility
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Description))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        public string Link
        { 
            get; set; 
        }

        /// <summary>
        /// Gets the link visibility.
        /// </summary>
        public Visibility LinkVisibility
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Link))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Gets the tab strip visibility.
        /// </summary>
        public Visibility TabVisibility
        {
            get
            {
                if (this.SamplePage != null && this.CodePage != null)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the sample tab visibility.
        /// </summary>
        public Visibility SampleVisibility
        {
            get
            {
                if (this.SamplePage != null)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Gets the code tab visibility.
        /// </summary>
        public Visibility CodeVisibility
        {
            get
            {
                if (this.CodePage != null)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Gets or sets the sample page.
        /// </summary>
        public FrameworkElement SamplePage 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the code page.
        /// </summary>
        public FrameworkElement CodePage
        {
            get; set;
        }
    }
}
