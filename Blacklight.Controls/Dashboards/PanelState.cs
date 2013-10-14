//-----------------------------------------------------------------------
// <copyright file="PanelState.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>06-Mar-2009</date>
// <author>Martin Grayson</author>
// <summary>Enum for panel states.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
{
    using System;

    /// <summary>
    /// Enum for panel states.
    /// </summary>
    public enum PanelState
    {
        /// <summary>
        /// The normal state for a panel.
        /// </summary>
        Restored,

        /// <summary>
        /// The maxmized state for a panel.
        /// </summary>
        Maximized,

        /// <summary>
        /// The minimized state for a panel.
        /// </summary>
        Minimized
    }
}
