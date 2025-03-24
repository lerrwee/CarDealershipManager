using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System;
using Microsoft.Win32;

namespace CarDealershipManager
{
    public partial class CarsPage : Page
    {
        private CarDealershipDBContext context;
        private Car selectedCar;
        private User currentUser;

        public CarsPage(User user)
        {
            InitializeComponent();
            context = new CarDealershipDBContext();
            Model_TextBox.Focus();
            currentUser = user;
            LoadBrands();
            LoadCars();

            CommonUtils.DisplayUserInfo(currentUser, UserInfo_TextBlock);
            CommonUtils.CheckUserRole(currentUser, Delete_Button);
        }

        private void LoadBrands()
        {
            Brand_ComboBox.ItemsSource = context.Brands.ToList();
            Brand_ComboBox.DisplayMemberPath = "name";
            Brand_ComboBox.SelectedValuePath = "id";
        }

        private void LoadCars()
        {
            Cars_ListBox.ItemsSource = context.Cars.ToList();
        }

        private void Cars_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCar = Cars_ListBox.SelectedItem as Car;
            if (selectedCar != null)
            {
                Model_TextBox.Text = selectedCar.model;
                Year_TextBox.Text = selectedCar.year.ToString();
                Price_TextBox.Text = selectedCar.price.ToString();
                Equipment_TextBox.Text = selectedCar.equipment;
                Brand_ComboBox.SelectedValue = selectedCar.brand_id;
                Photo_TextBox.Text = selectedCar.photo;
                Mileage_TextBox.Text = selectedCar.mileage.ToString();
                Color_TextBox.Text = selectedCar.color;
                EngineType_TextBox.Text = selectedCar.engine_type;
                FuelType_TextBox.Text = selectedCar.fuel_type;
                Edit_Button.Visibility = Visibility.Visible;
                Save_Button.Visibility = Visibility.Collapsed;

                if (currentUser != null && currentUser.Role.name == "Администратор")
                {
                    Delete_Button.Visibility = Visibility.Visible;
                }

                if (!string.IsNullOrEmpty(selectedCar.photo))
                {
                    try
                    {
                        CarPhoto_Image.Source = new BitmapImage(new Uri(selectedCar.photo, UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при загрузке фото: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    CarPhoto_Image.Source = null;
                }
            }
            else
            {
                ClearFields();
                CarPhoto_Image.Source = null;
            }
        }

        private void SelectPhoto_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                Photo_TextBox.Text = openFileDialog.FileName;
                CarPhoto_Image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInput())
            {
                try
                {
                    if (selectedCar == null)
                    {
                        Car car = new Car
                        {
                            model = CommonUtils.TrimAndReplaceDoubleSpaces(Model_TextBox.Text),
                            year = int.Parse(Year_TextBox.Text),
                            price = int.Parse(Price_TextBox.Text),
                            equipment = CommonUtils.TrimAndReplaceDoubleSpaces(Equipment_TextBox.Text),
                            brand_id = (int)Brand_ComboBox.SelectedValue,
                            photo = CommonUtils.TrimAndReplaceDoubleSpaces(Photo_TextBox.Text),
                            mileage = int.Parse(Mileage_TextBox.Text),
                            color = CommonUtils.TrimAndReplaceDoubleSpaces(Color_TextBox.Text),
                            engine_type = CommonUtils.TrimAndReplaceDoubleSpaces(EngineType_TextBox.Text),
                            fuel_type = CommonUtils.TrimAndReplaceDoubleSpaces(FuelType_TextBox.Text)
                        };

                        context.Cars.Add(car);
                    }
                    else
                    {
                        selectedCar.model = CommonUtils.TrimAndReplaceDoubleSpaces(Model_TextBox.Text);
                        selectedCar.year = int.Parse(Year_TextBox.Text);
                        selectedCar.price = int.Parse(Price_TextBox.Text);
                        selectedCar.equipment = CommonUtils.TrimAndReplaceDoubleSpaces(Equipment_TextBox.Text);
                        selectedCar.brand_id = (int)Brand_ComboBox.SelectedValue;
                        selectedCar.photo = CommonUtils.TrimAndReplaceDoubleSpaces(Photo_TextBox.Text);
                        selectedCar.mileage = int.Parse(Mileage_TextBox.Text);
                        selectedCar.color = CommonUtils.TrimAndReplaceDoubleSpaces(Color_TextBox.Text);
                        selectedCar.engine_type = CommonUtils.TrimAndReplaceDoubleSpaces(EngineType_TextBox.Text);
                        selectedCar.fuel_type = CommonUtils.TrimAndReplaceDoubleSpaces(FuelType_TextBox.Text);
                    }

                    context.SaveChanges();

                    ClearFields();
                    LoadCars();
                    CarPhoto_Image.Source = null;
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
            if (IsValidInput())
            {
                try
                {
                    if (selectedCar != null)
                    {
                        selectedCar.model = CommonUtils.TrimAndReplaceDoubleSpaces(Model_TextBox.Text);
                        selectedCar.year = int.Parse(Year_TextBox.Text);
                        selectedCar.price = int.Parse(Price_TextBox.Text);
                        selectedCar.equipment = CommonUtils.TrimAndReplaceDoubleSpaces(Equipment_TextBox.Text);
                        selectedCar.brand_id = (int)Brand_ComboBox.SelectedValue;
                        selectedCar.photo = CommonUtils.TrimAndReplaceDoubleSpaces(Photo_TextBox.Text);
                        selectedCar.mileage = int.Parse(Mileage_TextBox.Text);
                        selectedCar.color = CommonUtils.TrimAndReplaceDoubleSpaces(Color_TextBox.Text);
                        selectedCar.engine_type = CommonUtils.TrimAndReplaceDoubleSpaces(EngineType_TextBox.Text);
                        selectedCar.fuel_type = CommonUtils.TrimAndReplaceDoubleSpaces(FuelType_TextBox.Text);

                        context.SaveChanges();

                        ClearFields();
                        LoadCars();
                        CarPhoto_Image.Source = null;
                    }
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
            if (selectedCar != null && currentUser != null && currentUser.Role.name == "Администратор")
            {
                try
                {
                    context.Cars.Remove(selectedCar);
                    context.SaveChanges();

                    ClearFields();
                    LoadCars();
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

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private bool IsValidInput()
        {
            int temp;
            return !string.IsNullOrEmpty(Model_TextBox.Text) &&
                   int.TryParse(Year_TextBox.Text, out temp) &&
                   int.TryParse(Price_TextBox.Text, out temp) &&
                   !string.IsNullOrEmpty(Equipment_TextBox.Text) &&
                   Brand_ComboBox.SelectedItem != null &&
                   !string.IsNullOrEmpty(Photo_TextBox.Text) &&
                   int.TryParse(Mileage_TextBox.Text, out temp) &&
                   !string.IsNullOrEmpty(Color_TextBox.Text) &&
                   !string.IsNullOrEmpty(EngineType_TextBox.Text) &&
                   !string.IsNullOrEmpty(FuelType_TextBox.Text);
        }

        private void ClearFields()
        {
            Model_TextBox.Text = string.Empty;
            Year_TextBox.Text = string.Empty;
            Price_TextBox.Text = string.Empty;
            Equipment_TextBox.Text = string.Empty;
            Brand_ComboBox.SelectedItem = null;
            Photo_TextBox.Text = string.Empty;
            Mileage_TextBox.Text = string.Empty;
            Color_TextBox.Text = string.Empty;
            EngineType_TextBox.Text = string.Empty;
            FuelType_TextBox.Text = string.Empty;
            selectedCar = null;
            Edit_Button.Visibility = Visibility.Collapsed;
            Delete_Button.Visibility = Visibility.Collapsed;
            Save_Button.Visibility = Visibility.Visible;
            Model_TextBox.IsEnabled = true;
            Year_TextBox.IsEnabled = true;
            Price_TextBox.IsEnabled = true;
            Equipment_TextBox.IsEnabled = true;
            Brand_ComboBox.IsEnabled = true;
            Photo_TextBox.IsEnabled = true;
            Mileage_TextBox.IsEnabled = true;
            Color_TextBox.IsEnabled = true;
            EngineType_TextBox.IsEnabled = true;
            FuelType_TextBox.IsEnabled = true;
            Cars_ListBox.SelectedIndex = -1;
            CarPhoto_Image.Source = null;
        }
               
        private void OpenBrandsPage_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandsPage(currentUser));
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void CarsList_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarsListPage(currentUser));
        }

        private void PreviewMouseWheel1(object sender, MouseWheelEventArgs e)
        {
            CommonUtils.PreviewMouseWheel1(sender, e);
        }
    }
}