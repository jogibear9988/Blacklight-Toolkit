//-----------------------------------------------------------------------
// <copyright file="ResourceKey.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>02-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>Class for getting a key out of a resource dictionary.</summary>
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
    /// Class for getting a key out of a resource dictionary.
    /// </summary>
    public class ResourceKey : DependencyObject
    {
        /// <summary>
        /// The resource name property.
        /// </summary>
        public static readonly DependencyProperty ResourceNameProperty = DependencyProperty.Register("ResourceName", typeof(string), typeof(ResourceKey), null);

        /// <summary>
        /// Resource key constructor.
        /// </summary>
        public ResourceKey()
        {
        }

        /// <summary>
        /// Gets or sets the resource name.
        /// </summary>
        public string ResourceName
        {
            get { return (string)GetValue(ResourceNameProperty); }
            set { SetValue(ResourceNameProperty, value); }
        }
    }
}
