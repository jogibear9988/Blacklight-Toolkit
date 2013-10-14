//-----------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>04-Dec-2008</date>
// <author>Martin Grayson</author>
// <summary>Class with trig and other helper functions.</summary>
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
    /// Class with trig and other helper functions.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Converts degress into radians.
        /// </summary>
        /// <param name="degrees">The degree value.</param>
        /// <returns>The degrees as radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Converts radians to degress.
        /// </summary>
        /// <param name="radians">The radians value.</param>
        /// <returns>The radians as degrees.</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Gets a point offset by a distance and angle (in degrees).
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>The offset point.</returns>
        public static Point GetOffset(double angle, double distance)
        {
            double x = Math.Cos(MathHelper.DegreesToRadians(angle)) * distance;
            double y = Math.Tan(MathHelper.DegreesToRadians(angle)) * x;
            return new Point(x, y);
        }

        /// <summary>
        /// Gets the angle between to points.
        /// </summary>
        /// <param name="offset">The offset point.</param>
        /// <returns>The angle for the offset.</returns>
        public static double GetAngleFromOffset(Point offset)
        {
            double opposite = Math.Abs(offset.Y);
            double adjacent = Math.Abs(offset.X);

            if (offset.Y < 0)
            {
                opposite = -opposite;
            }

            double angle = MathHelper.RadiansToDegrees(Math.Atan(opposite / adjacent));
            if (double.IsNaN(angle))
            {
                return 0.0;
            }

            if (offset.X < 0)
            {
                angle = 90 + (90 - angle);
            }

            return angle;
        }
    }
}
