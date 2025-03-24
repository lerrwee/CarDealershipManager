using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CarDealershipManager
{
    public partial class ManufacturersPage : Page
    {
        private CarDealershipDBContext context;
        private Manufacturer selectedManufacturer;
        private User currentUser;

        public ManufacturersPage(User user)
        {
            InitializeComponent();
            context = new CarDealershipDBContext();
            Name_TextBox.Focus();
            currentUser = user;
            LoadManufacturers();

            CommonUtils.DisplayUserInfo(currentUser, UserInfo_TextBlock);
            CommonUtils.CheckUserRole(currentUser, Delete_Button);
        }

        private void LoadManufacturers()
        {
            Manufacturers_ListBox.ItemsSource = context.Manufacturers.ToList();
        }

        private void Manufacturers_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedManufacturer = Manufacturers_ListBox.SelectedItem as Manufacturer;
            if (selectedManufacturer != null)
            {
                Name_TextBox.Text = selectedManufacturer.name;
                Country_TextBox.Text = selectedManufacturer.country;
                Edit_Button.Visibility = Visibility.Visible;
                Save_Button.Visibility = Visibility.Collapsed;

                if (currentUser != null && currentUser.Role.name == "Администратор")
                {
                    Delete_Button.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ClearFields();
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInput())
            {
                try
                {
                    if (selectedManufacturer == null)
                    {
                        Manufacturer manufacturer = new Manufacturer
                        {
                            name = CommonUtils.TrimAndReplaceDoubleSpaces(Name_TextBox.Text),
                            country = CommonUtils.TrimAndReplaceDoubleSpaces(Country_TextBox.Text)
                        };

                        context.Manufacturers.Add(manufacturer);
                    }
                    else
                    {
                        selectedManufacturer.name = CommonUtils.TrimAndReplaceDoubleSpaces(Name_TextBox.Text);
                        selectedManufacturer.country = CommonUtils.TrimAndReplaceDoubleSpaces(Country_TextBox.Text);
                    }

                    context.SaveChanges();

                    ClearFields();
                    LoadManufacturers();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedManufacturer != null && IsValidInput())
            {
                try
                {
                    selectedManufacturer.name = CommonUtils.TrimAndReplaceDoubleSpaces(Name_TextBox.Text);
                    selectedManufacturer.country = CommonUtils.TrimAndReplaceDoubleSpaces(Country_TextBox.Text);

                    context.SaveChanges();
                    ClearFields();
                    LoadManufacturers();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при редактировании данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandsPage(currentUser));
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedManufacturer != null && currentUser != null && currentUser.Role.name == "Администратор")
            {
                try
                {
                    var brandsWithManufacturer = context.Brands.Any(b => b.manufacturer_id == selectedManufacturer.id);
                    if (brandsWithManufacturer)
                    {
                        MessageBox.Show("Нельзя удалить производителя, так как существуют бренды с этим производителем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    context.Manufacturers.Remove(selectedManufacturer);
                    context.SaveChanges();

                    ClearFields();
                    LoadManufacturers();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при удалении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("У вас нет прав на удаление данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrEmpty(Name_TextBox.Text) || string.IsNullOrEmpty(Country_TextBox.Text))
            {
                return false;
            }

            if (!Regex.IsMatch(Country_TextBox.Text, @"^[а-яА-Яa-zA-Z\s]+$"))
            {
                return false;
            }
            if (!Regex.IsMatch(Name_TextBox.Text, @"^[а-яА-Яa-zA-Z0-9\s]+$"))
            {
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            Name_TextBox.Text = string.Empty;
            Country_TextBox.Text = string.Empty;
            selectedManufacturer = null;
            Edit_Button.Visibility = Visibility.Collapsed;
            Delete_Button.Visibility = Visibility.Collapsed;
            Save_Button.Visibility = Visibility.Visible;
            Manufacturers_ListBox.SelectedIndex = -1;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}