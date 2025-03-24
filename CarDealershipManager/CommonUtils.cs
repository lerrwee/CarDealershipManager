using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace CarDealershipManager
{
    public static class CommonUtils
    {
        public static void DisplayUserInfo(User currentUser, TextBlock userInfoTextBlock)
        {
            if (currentUser != null)
            {
                userInfoTextBlock.Text = $"Вы вошли как: {currentUser.login}, Роль: {currentUser.Role.name}";
                userInfoTextBlock.Visibility = Visibility.Visible;
            }
        }

        public static void CheckUserRole(User currentUser, Button deleteButton)
        {
            if (currentUser != null && currentUser.Role.name != "Администратор")
            {
                deleteButton.Visibility = Visibility.Collapsed;
            }
        }

        public static void PreviewMouseWheel1(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = FindVisualParent<ScrollViewer>(sender as DependencyObject);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }

        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }

        public static string TrimAndReplaceDoubleSpaces(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input.Trim(), @"\s+", " ");
        }
    }
}