using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace CarDealershipManager
{
    public partial class BrandsPage : Page
    {
        private CarDealershipDBContext context;
        private Brand selectedBrand;
        private User currentUser;

        public BrandsPage(User user)
        {
            InitializeComponent();
            context = new CarDealershipDBContext();
            Name_TextBox.Focus();
            currentUser = user;
            LoadManufacturers();
            LoadBrands();

            CommonUtils.DisplayUserInfo(currentUser, UserInfo_TextBlock);
            CommonUtils.CheckUserRole(currentUser, Delete_Button);
        }

        private void LoadManufacturers()
        {
            Manufacturer_ComboBox.ItemsSource = context.Manufacturers.ToList();
            Manufacturer_ComboBox.DisplayMemberPath = "name";
            Manufacturer_ComboBox.SelectedValuePath = "id";
        }

        private void LoadBrands()
        {
            Brands_ListBox.ItemsSource = context.Brands.ToList();
        }

        private void Brands_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBrand = Brands_ListBox.SelectedItem as Brand;
            if (selectedBrand != null)
            {
                Name_TextBox.Text = selectedBrand.name;
                Manufacturer_ComboBox.SelectedValue = selectedBrand.manufacturer_id;
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
                    if (selectedBrand == null)
                    {
                        Brand brand = new Brand
                        {
                            name = CommonUtils.TrimAndReplaceDoubleSpaces(Name_TextBox.Text),
                            manufacturer_id = (int)Manufacturer_ComboBox.SelectedValue
                        };

                        context.Brands.Add(brand);
                    }
                    else
                    {
                        selectedBrand.name = CommonUtils.TrimAndReplaceDoubleSpaces(Name_TextBox.Text);
                        selectedBrand.manufacturer_id = (int)Manufacturer_ComboBox.SelectedValue;
                    }

                    context.SaveChanges();

                    ClearFields();
                    LoadBrands();
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
            if (selectedBrand != null && IsValidInput())
            {
                try
                {
                    selectedBrand.name = CommonUtils.TrimAndReplaceDoubleSpaces(Name_TextBox.Text);
                    selectedBrand.manufacturer_id = (int)Manufacturer_ComboBox.SelectedValue;

                    context.SaveChanges();

                    ClearFields();
                    LoadBrands();
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

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBrand != null && currentUser != null && currentUser.Role.name == "Администратор")
            {
                try
                {
                    var carsWithBrand = context.Cars.Any(c => c.brand_id == selectedBrand.id);
                    if (carsWithBrand)
                    {
                        MessageBox.Show("Нельзя удалить марку, так как существуют автомобили с этой маркой.",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    context.Brands.Remove(selectedBrand);
                    context.SaveChanges();

                    ClearFields();
                    LoadBrands();
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
            if (string.IsNullOrEmpty(Name_TextBox.Text) || Manufacturer_ComboBox.SelectedItem == null)
            {
                return false;
            }

            string pattern = @"^[а-яА-Яa-zA-Z0-9\s]+$";
            if (!Regex.IsMatch(Name_TextBox.Text, pattern))
            {
                MessageBox.Show("Название модели может содержать только русские, английские буквы и цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            Name_TextBox.Text = string.Empty;
            Manufacturer_ComboBox.SelectedItem = null;
            selectedBrand = null;
            Edit_Button.Visibility = Visibility.Collapsed;
            Delete_Button.Visibility = Visibility.Collapsed;
            Save_Button.Visibility = Visibility.Visible;
            Brands_ListBox.SelectedIndex = -1;
        }

        private void OpenManufacturersPage_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManufacturersPage(currentUser));
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarsPage(currentUser));
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