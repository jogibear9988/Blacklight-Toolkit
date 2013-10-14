//-----------------------------------------------------------------------
// <copyright file="OrientedSize.cs" company="Microsoft Corporation copyright 2008.">
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.
// </copyright>
// <date>26-Feb-2009</date>
// <summary>Oriented size class.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;

    /// <summary>
    /// The OrientedSize structure is used to abstract the growth direction from
    /// the layout algorithms of WrapPanel.  When the growth direction is
    /// oriented horizontally (ex: the next element is arranged on the side of
    /// the previous element), then the Width grows directly with the placement
    /// of elements and Height grows indirectly with the size of the largest
    /// element in the row.  When the orientation is reversed, so is the
    /// directional growth with respect to Width and Height.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct OrientedSize
    {
        /// <summary>
        /// The orientation of the structure.
        /// </summary>
        private Orientation orientation;

        /// <summary>
        /// The size dimension that grows directly with layout placement.
        /// </summary>
        private double direct;

        /// <summary>
        /// The size dimension that grows indirectly with the maximum value of
        /// the layout row or column.
        /// </summary>
        private double indirect;

        /// <summary>
        /// Initializes a new OrientedSize structure.
        /// </summary>
        /// <param name="orientation">Orientation of the structure.</param>
        public OrientedSize(Orientation orientation) :
            this(orientation, 0.0, 0.0)
        {
        }

        /// <summary>
        /// Initializes a new OrientedSize structure.
        /// </summary>
        /// <param name="orientation">Orientation of the structure.</param>
        /// <param name="width">Un-oriented width of the structure.</param>
        /// <param name="height">Un-oriented height of the structure.</param>
        public OrientedSize(Orientation orientation, double width, double height)
        {
            this.orientation = orientation;

            // All fields must be initialized before we access the this pointer
            this.direct = 0.0;
            this.indirect = 0.0;

            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Gets the orientation of the structure.
        /// </summary>
        public Orientation Orientation
        {
            get { return this.orientation; }
        }

        /// <summary>
        /// Gets or sets the size dimension that grows directly with layout
        /// placement.
        /// </summary>
        public double Direct
        {
            get { return this.direct; }
            set { this.direct = value; }
        }

        /// <summary>
        /// Gets or sets the size dimension that grows indirectly with the
        /// maximum value of the layout row or column.
        /// </summary>
        public double Indirect
        {
            get { return this.indirect; }
            set { this.indirect = value; }
        }

        /// <summary>
        /// Gets or sets the width of the size.
        /// </summary>
        public double Width
        {
            get
            {
                return (this.Orientation == Orientation.Horizontal) ?
                    this.Direct :
                    this.Indirect;
            }

            set
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    this.Direct = value;
                }
                else
                {
                    this.Indirect = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the size.
        /// </summary>
        public double Height
        {
            get
            {
                return (this.Orientation != Orientation.Horizontal) ?
                    this.Direct :
                    this.Indirect;
            }

            set
            {
                if (this.Orientation != Orientation.Horizontal)
                {
                    this.Direct = value;
                }
                else
                {
                    this.Indirect = value;
                }
            }
        }
    }
}