//-----------------------------------------------------------------------
// <copyright file="TransformInformation.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>01-Nov-2008</date>
// <author>Mike Parker</author>
// <summary>A class that contains an instance of a translate transform and its index within a transform group</summary>
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
    /// A class that contains an instance of a translate transform and its index within a transform group.
    /// </summary>
    public class TransformInformation
    {
        #region Private Members

        /// <summary>
        /// Stores the TranslateTransform instance that is used to set the FrameworkElement object's position in the AnimatedLayoutPanel.
        /// </summary>
        private TranslateTransform translate;

        /// <summary>
        /// Identifies the index that is referenced as part of a PropertyPath for the TranslateTransform within a TransformGroup.
        /// </summary>
        private int position;

        #endregion

        #region Constructor

        /// <summary>
        /// TransformInformation Constructor.
        /// </summary>
        public TransformInformation()
        {
            this.translate = new TranslateTransform();
        }

        /// <summary>
        /// TransformInformation Constructor with overloads.
        /// </summary>
        /// <param name="translateTransform">TranslateTransform instance.</param>
        /// <param name="positionInCollection">Position of the TranslateTransform within the TransformGroup.</param>
        public TransformInformation(TranslateTransform translateTransform, int positionInCollection)
        {
            this.translate = translateTransform;
            this.position = positionInCollection;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets the TranslateTransform instance.
        /// </summary>
        public TranslateTransform TranslateTransform
        {
            get { return this.translate; }
            set { this.translate = value; }
        }

        /// <summary>
        /// Gets or sets the index of the TranslateTransform within the TransformGroup.
        /// </summary>
        public int PositionInCollection
        {
            get { return this.position; }
            set { this.position = value; }
        }

        #endregion
    }
}
