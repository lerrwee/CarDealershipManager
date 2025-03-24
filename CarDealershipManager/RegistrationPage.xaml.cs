using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CarDealershipManager
{
    public partial class RegistrationPage : Page
    {
        private CarDealershipDBContext context;

        public RegistrationPage()
        {
            InitializeComponent();
            context = new CarDealershipDBContext();
            Login_TextBox.Focus();
            LoadRoles();
        }

        private void LoadRoles()
        {
            Role_ComboBox.ItemsSource = context.Roles.ToList();
            Role_ComboBox.DisplayMemberPath = "name";
            Role_ComboBox.SelectedValuePath = "id";
        }

        private void Registration_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInput())
            {
                string Login = Login_TextBox.Text.ToUpper();
                string Password = Password_PasswordBox.Password.ToUpper();
                int RoleId = (int)Role_ComboBox.SelectedValue;

                if (IsLoginUnique(Login))
                {
                    User user = new User
                    {
                        login = Login,
                        password = Password,
                        role_id = RoleId
                    };

                    context.Users.Add(user);
                    context.SaveChanges();
                    ClearFields();
                    NavigationService.Navigate(new LoginPage());
                    MessageBox.Show("Регистрация успешна! Войдите в аккаунт.",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Логин уже занят! Выберите другой логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Регистрация не прошла! " +
                    "Логин и пароль должны содержать только английские буквы и цифры, а также иметь длину не менее " +
                    "8 и 6 символов соответственно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private bool IsValidInput()
        {
            return IsValidLogin(Login_TextBox.Text) &&
                   IsValidPassword(Password_PasswordBox.Password) &&
                   Password_PasswordBox.Password == PasswordConfirm_PasswordBox.Password &&
                   Role_ComboBox.SelectedItem != null;
        }

        private bool IsValidLogin(string login)
        {
            // Логин должен содержать только английские буквы, цифры и символы _ и иметь длину не менее 8 символов
            return Regex.IsMatch(login, @"^[a-zA-Z0-9_]{8,}$");
        }

        private bool IsValidPassword(string password)
        {
            // Пароль должен содержать только английские буквы и цифры и иметь длину не менее 6 символов
            return Regex.IsMatch(password, @"^[a-zA-Z0-9]{6,}$");
        }

        private bool IsLoginUnique(string login)
        {
            return !context.Users.Any(u => u.login == login);
        }

        private void ClearFields()
        {
            Login_TextBox.Text = string.Empty;
            Password_PasswordBox.Password = string.Empty;
            PasswordConfirm_PasswordBox.Password = string.Empty;
            Role_ComboBox.SelectedItem = null;
        }                
    }
}