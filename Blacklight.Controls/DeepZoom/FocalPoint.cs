//-----------------------------------------------------------------------
// <copyright file="FocalPoint.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>17-Nov-2008</date>
// <summary>Class for representing a focal point.</summary>
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
    using System.ComponentModel;

    /// <summary>
    /// Class for representing a focal point.
    /// </summary>
    public class FocalPoint : INotifyPropertyChanged
    {
        /// <summary>
        /// Stores the focal point text.
        /// </summary>
        private string text;

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text 
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        /// <summary>
        /// Gets or sets the rect.
        /// </summary>
        public Rect Area 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the sub image index.
        /// </summary>
        public int SubImageIndex 
        { 
            get; set; 
        }

        /// <summary>
        /// Gets or sets the focal point id.
        /// </summary>
        public int Id
        {
            get;
            set;
        }
    }
}
