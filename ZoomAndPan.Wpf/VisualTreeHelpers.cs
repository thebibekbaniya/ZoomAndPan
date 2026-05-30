using System.Windows;
using System.Windows.Media;

namespace ZoomAndPan.Wpf;

public static class VisualTreeHelpers
{
    /// <summary>
    /// Find first parent of type T in VisualTree.
    /// </summary>
    public static T FindParentControl<T>(this DependencyObject control) where T : DependencyObject
    {
        var parent = VisualTreeHelper.GetParent(control);
        while (parent != null && !(parent is T))
            parent = VisualTreeHelper.GetParent(parent);
        return parent as T;
    }

    /// <summary>
    /// Find first child of type T in VisualTree (breadth-first among siblings, then recurses).
    /// </summary>
    public static T FindChildControl<T>(this DependencyObject control) where T : DependencyObject
    {
        var childNumber = VisualTreeHelper.GetChildrenCount(control);
        for (var i = 0; i < childNumber; i++)
        {
            var child = VisualTreeHelper.GetChild(control, i);
            if (child is T match)
                return match;
            var result = FindChildControl<T>(child);
            if (result != null)
                return result;
        }
        return null;
    }
}