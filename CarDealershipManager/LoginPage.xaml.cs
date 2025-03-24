using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace CarDealershipManager
{
    public partial class LoginPage : Page
    {
        private CarDealershipDBContext context;

        public LoginPage()
        {
            InitializeComponent();
            context = new CarDealershipDBContext();
            Login_TextBox.Focus();
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Login_TextBox.Text) && !string.IsNullOrEmpty(Password_PasswordBox.Password))
            {
                string Login = Login_TextBox.Text.ToUpper();
                string Password = Password_PasswordBox.Password.ToUpper();
                var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.login == Login && u.password == Password);

                if (user != null)
                {
                    MessageBox.Show("Успешная авторизация!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new CarsPage(user));
                }
                else
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Registration_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }        
    }
}