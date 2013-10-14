//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>The demo application.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Showcase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// The demo application.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// App constructor.
        /// </summary>
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            this.InitializeComponent();
        }

        /// <summary>
        /// Creates the page as the root visual.
        /// </summary>
        /// <param name="sender">The application.</param>
        /// <param name="e">Startup event args.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new Page();
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="sender">The application.</param>
        /// <param name="e">Event args.</param>
        private void Application_Exit(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Displays an error dialogue.
        /// </summary>
        /// <param name="sender">The application.</param>
        /// <param name="e">Application Unhandled Exception Event Args.</param>
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;

                try
                {
                    string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                    errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                    System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
