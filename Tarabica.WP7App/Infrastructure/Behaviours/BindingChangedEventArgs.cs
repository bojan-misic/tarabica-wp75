// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Windows;

namespace Tarabica.WP7App.Infrastructure.Behaviours
{
    /// <summary>
    /// Event args for when binding values change.
    /// </summary>
    public class BindingChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e"></param>
        public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e)
        {
            this.EventArgs = e;
        }

        /// <summary>
        /// Original event args.
        /// </summary>
        public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
    }
}
