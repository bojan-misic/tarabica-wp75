﻿//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Tarabica.WP7App.Infrastructure.Behaviours
{
    public class PopupHideOnLeftMouseUp : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.MouseLeftButtonUp += this.MouseLeftButtonUp;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseLeftButtonUp -= this.MouseLeftButtonUp;
            base.OnDetaching();
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Hide Popup
            FrameworkElement topElement;

            // Look for parent popup
            for (topElement = (FrameworkElement)this.AssociatedObject.Parent;
                 topElement != null && !(topElement is Popup);
                 topElement = (FrameworkElement)topElement.Parent)
            {
            }
            if (topElement != null)
            {
                ((Popup)topElement).IsOpen = false;
            }
        }
    }
}
