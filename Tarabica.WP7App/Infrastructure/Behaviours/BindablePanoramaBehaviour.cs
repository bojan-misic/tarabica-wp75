// (c) Copyright Graeme Bradbury.
// graeme@siliconshark.com
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace Tarabica.WP7App.Infrastructure.Behaviours
{
    /// <summary>
    /// Behaviour that exposes a TargetIndex property allowing binding between the Panorama control and a ViewModel.
    /// Usefull in tombstoning. Allowing the user to return to the correct panorama page.
    /// </summary>
    public class BindablePanoramaBehaviour : Behavior<Panorama>
    {
        public static readonly DependencyProperty TargetIndexProperty =
            DependencyProperty.Register("TargetIndex", typeof(Binding), typeof(BindablePanoramaBehaviour),
                                        new PropertyMetadata(null, HandleTargetIndexBindingChanged));

        private readonly BindingListener _listener;

        public BindablePanoramaBehaviour()
        {
            _listener = new BindingListener(HandleBindingValueChanged);
        }

        public Binding TargetIndex
        {
            get { return (Binding)this.GetValue(BindablePanoramaBehaviour.TargetIndexProperty); }
            set { this.SetValue(BindablePanoramaBehaviour.TargetIndexProperty, value); }
        }

        /// <summary>
        /// Handles the the source value changing
        /// </summary>
        private void HandleBindingValueChanged(Object sender, BindingChangedEventArgs args)
        {
            if (!_selectionChanging)
                SetSelectedIndex();
        }

        private void SetSelectedIndex()
        {
            var i = (int)_listener.Value;

            if (AssociatedObject != null)
            {
                if (i >= 0 && i < AssociatedObject.Items.Count)
                {
                    //Bypass the private Setter on the SelectedItem property by directly calling SetValue
                    //on the SelectedItem dependency property.
                    //AssociatedObject.SetValue(Panorama.SelectedItemProperty, AssociatedObject.Items[i]);
                    //AssociatedObject.Measure(new Size());
                    AssociatedObject.DefaultItem = AssociatedObject.Items[i];
                }
            }
        }

        private static void HandleTargetIndexBindingChanged(DependencyObject sender,
                                                            DependencyPropertyChangedEventArgs args)
        {
            ((BindablePanoramaBehaviour)sender).OnTargetIndexBindingChanged(args);
        }

        private void OnTargetIndexBindingChanged(DependencyPropertyChangedEventArgs args)
        {
            _listener.Binding = (Binding)args.NewValue;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _listener.Element = AssociatedObject;
            AssociatedObject.SelectionChanged += HandleSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _listener.Element = null;
            AssociatedObject.SelectionChanged -= HandleSelectionChanged;
        }

        private bool _selectionChanging;
        /// <summary>
        /// Handles the Panorama control selected item changing
        /// </summary>
        private void HandleSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectionChanging = true;
            _listener.Value = AssociatedObject.SelectedIndex;
            _selectionChanging = false;
        }
    }
}