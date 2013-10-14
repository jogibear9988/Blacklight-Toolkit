//-----------------------------------------------------------------------
// <copyright file="AnimationState.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>24-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>Enumeration for representing state of an animation.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
{
    using System;

    /// <summary>
    /// Enumeration for representing state of an animation.
    /// </summary>
    public enum AnimationState
    {
        /// <summary>
        /// The animation is playing.
        /// </summary>
        Playing,

        /// <summary>
        /// The animation is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The animation is stopped.
        /// </summary>
        Stopped
    }
}
