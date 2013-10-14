//-----------------------------------------------------------------------
// <copyright file="Contributor.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>20-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>Class representing an project contributor.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Showcase.Samples
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
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Class representing an project contributor.
    /// </summary>
    public class Contributor
    {
        /// <summary>
        /// Gets or sets the contributor name.
        /// </summary>
        public string Name 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the contributor job role.
        /// </summary>
        public string JobRole 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the contributor blacklight role.
        /// </summary>
        public string BlacklightRole 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the contributor image.
        /// </summary>
        public BitmapImage Image 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the contributor about text.
        /// </summary>
        public string About 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the contributor blog link.
        /// </summary>
        public string Blog 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets the contributor blog link visibility.
        /// </summary>
        public Visibility BlogVisibility
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Blog))
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }
    }
}
