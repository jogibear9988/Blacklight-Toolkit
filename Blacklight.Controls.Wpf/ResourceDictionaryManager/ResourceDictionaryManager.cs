//-----------------------------------------------------------------------
// <copyright file="ResourceDictionaryManager.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>02-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>Class for managing multiple resource dictionaries at the application level.</summary>
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
    using System.IO;
    using System.Windows.Markup;

    /// <summary>
    /// Class for managing multiple resource dictionaries at the application level.
    /// </summary>
    public class ResourceDictionaryManager
    {
        /// <summary>
        /// Stores a collection of the resource dictionaries.
        /// </summary>
        private ResourceDictionaryCollection resourceDictionaries;

        /// <summary>
        /// Class for managing multiple resource dictionaries at the application level.
        /// </summary>
        public ResourceDictionaryManager()
        {
        }

        /// <summary>
        /// Gets or sets the resource dictionaries.
        /// </summary>
        public ResourceDictionaryCollection ResourceDictionaries
        {
            get 
            { 
                return this.resourceDictionaries; 
            }

            set 
            { 
                this.resourceDictionaries = value; 
            }
        }
    }
}
