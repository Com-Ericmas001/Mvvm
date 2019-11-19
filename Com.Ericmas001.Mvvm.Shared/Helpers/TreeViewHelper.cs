using System.Windows;
using System.Windows.Controls;
#pragma warning disable 1591

namespace Com.Ericmas001.Mvvm.Helpers
{
    public class TreeViewHelper
    {
        public static object GetSelectedItem(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new PropertyMetadata(new object(), SelectedItemChanged));

        static void SelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView == null)
                return;

            treeView.SelectedItemChanged -= treeView_SelectedItemChanged;
            treeView.SelectedItemChanged += treeView_SelectedItemChanged;

            TreeViewItem thisItem = treeView.ItemContainerGenerator.ContainerFromItem(e.NewValue) as TreeViewItem;
            if (thisItem != null)
                thisItem.IsSelected = true;

        }

        static void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = sender as TreeView;
            SetSelectedItem(treeView, e.NewValue);
        }
    }
}
