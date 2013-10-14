//-----------------------------------------------------------------------
// <copyright file="ResourceDictionary.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>02-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>Class representing a resource dictionary.</summary>
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
    /// Class representing a resource dictionary.
    /// </summary>
    public class ResourceDictionary
    {
        /// <summary>
        /// Stores the Silverlight resource dictionary.
        /// </summary>
        private System.Windows.ResourceDictionary sourceResourceDictionary;

        /// <summary>
        /// Stores the collection of resource keys.
        /// </summary>
        private ResourceKeyCollection resourceKeyCollection;

        /// <summary>
        /// Stores the path to the resource dictionary.
        /// </summary>
        private string path;

        /// <summary>
        /// Stores whether the keys have been added.
        /// </summary>
        private bool keysAdded;

        /// <summary>
        /// ResourceDictionary constructor.
        /// </summary>
        public ResourceDictionary()
        {
        }

        /// <summary>
        /// Gets or sets the path to the resource dictionary, and loads the resources.
        /// </summary>
        public string Path
        {
            get 
            { 
                return this.path; 
            }

            set 
            { 
                this.path = value;
                this.LoadResourceDictionary();
            }
        }

        /// <summary>
        /// Gets or sets the source resource dictionary.
        /// </summary>
        public System.Windows.ResourceDictionary SourceResourceDictionary
        {
            get 
            { 
                return this.sourceResourceDictionary; 
            }

            set 
            { 
                this.sourceResourceDictionary = value;
                this.ResourceKeys = this.resourceKeyCollection;
            }
        }       

        /// <summary>
        /// Gets or sets the resource key collection.
        /// </summary>
        public ResourceKeyCollection ResourceKeys
        {
            get 
            { 
                return this.resourceKeyCollection; 
            }

            set 
            {
                this.resourceKeyCollection = value;

                if (this.resourceKeyCollection != null)
                {
                    foreach (ResourceKey key in this.resourceKeyCollection)
                    {
                        if (this.sourceResourceDictionary != null)
                        {
                            if (!Application.Current.Resources.Contains(key.ResourceName) && this.sourceResourceDictionary.Contains(key.ResourceName))
                            {
                                Application.Current.Resources.Add(key.ResourceName, this.sourceResourceDictionary[key.ResourceName]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the resource dictionary.
        /// </summary>
        public void LoadResourceDictionary()
        {
            Stream stream = Application.GetResourceStream(new Uri(this.Path, UriKind.RelativeOrAbsolute)).Stream;
            StreamReader reader = new StreamReader(stream);
            string xaml = reader.ReadToEnd();
            this.SourceResourceDictionary = XamlReader.Load(xaml) as System.Windows.ResourceDictionary;
        }
    }
}
