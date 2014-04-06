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
using System.Windows.Interactivity;

namespace Tarabica.WP7App.Infrastructure.Behaviours
{
    public class MouseLeaveBehaviour : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        void AssociatedObject_MouseLeave(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateStoryboard().Begin();
        }

        private Storyboard CreateStoryboard()
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation animationX = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(400),
                From = -10,
                To = 0,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTargetProperty(animationX, new PropertyPath("(UIElement.Projection).(PlaneProjection.RotationX)"));

            sb.Children.Add(animationX);

            Storyboard.SetTarget(sb, AssociatedObject);
            return sb;
        }
    }
}
