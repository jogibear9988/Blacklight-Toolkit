//-----------------------------------------------------------------------
// <copyright file="StripHtmlConverter.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>26-Feb-2009</date>
// <author>Martin Grayson</author>
// <summary>A converter for stripping HTML tags from text.</summary>
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
    using System.Windows.Data;

    /// <summary>
    /// A converter for stripping HTML tags from text.
    /// </summary>
    public class StripHtmlConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Returns the converted value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converter value.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value.ToString();
            string strippedText = string.Empty;
            bool inHtml = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (!inHtml && (text[i] == '<' || text[i] == '&'))
                {
                    inHtml = true;
                }
                else if ((inHtml && text[i] == '>') || (!inHtml && text[i] == ';'))
                {
                    inHtml = false;
                }
                else
                {
                    if (!inHtml)
                    {
                        strippedText += text[i];
                    }
                }
            }

            return new TextTrimmingConverter().Convert(strippedText.Replace("\n", " ").Replace("\r", " ").Replace("\t", " "), targetType, parameter, culture);
        }

        /// <summary>
        /// Converts a value back.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The converted back value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
