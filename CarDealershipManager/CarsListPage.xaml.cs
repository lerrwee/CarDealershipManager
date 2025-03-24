using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarDealershipManager
{
    public partial class CarsListPage : Page
    {
        private CarDealershipDBContext context;
        private User currentUser;
        private List<Car> allCars;

        public CarsListPage(User user)
        {
            InitializeComponent();
            context = new CarDealershipDBContext();
            currentUser = user;
            LoadCars();

            CommonUtils.DisplayUserInfo(currentUser, UserInfo_TextBlock);
        }

        private void LoadCars()
        {
            allCars = context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Brand.Manufacturer)
                .ToList();

            foreach (var car in allCars)
            {
                if (!string.IsNullOrEmpty(car.photo))
                {
                    car.photo = new BitmapImage(new Uri(car.photo, UriKind.RelativeOrAbsolute)).ToString();
                }
            }

            Cars_DataGrid.ItemsSource = allCars;
        }

        private void Search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = Search_TextBox.Text.ToLower();
            var filteredCars = allCars
                .Where(c => c.model.ToLower().IndexOf(searchText) >= 0 ||
                            c.year.ToString().ToLower().IndexOf(searchText) >= 0 ||
                            c.price.ToString().ToLower().IndexOf(searchText) >= 0 ||
                            c.equipment.ToLower().IndexOf(searchText) >= 0 ||
                            c.mileage.ToString().ToLower().IndexOf(searchText) >= 0 ||
                            c.color.ToLower().IndexOf(searchText) >= 0 ||
                            c.engine_type.ToLower().IndexOf(searchText) >= 0 ||
                            c.fuel_type.ToLower().IndexOf(searchText) >= 0 ||
                            c.photo.ToLower().IndexOf(searchText) >= 0 ||
                            c.Brand.name.ToLower().IndexOf(searchText) >= 0 ||
                            c.Brand.Manufacturer.name.ToLower().IndexOf(searchText) >= 0 ||
                            c.Brand.Manufacturer.country.ToLower().IndexOf(searchText) >= 0)
                .ToList();
            Cars_DataGrid.ItemsSource = filteredCars;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            Search_TextBox.Text = string.Empty;
            Cars_DataGrid.ItemsSource = allCars;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarsPage(currentUser));
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void PreviewMouseWheel1(object sender, MouseWheelEventArgs e)
        {
            CommonUtils.PreviewMouseWheel1(sender, e);
        }
    }
}