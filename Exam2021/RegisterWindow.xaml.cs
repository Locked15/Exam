using System;
using System.Windows;
using System.Windows.Controls;
using Bool = System.Boolean;

namespace Exam2021
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void UserPasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UserPasswordTextBox.Visibility == Visibility.Visible)
            {
                UserPasswordPasswordBox.Password = UserPasswordTextBox.Text;
            }
        }

        private void UserPasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UserPasswordTextBox.Visibility == Visibility.Hidden)
            {
                UserPasswordTextBox.Text = UserPasswordPasswordBox.Password;
            }
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Bool userPasswordIsHided = UserPasswordPasswordBox.Visibility == Visibility.Visible;

            if (String.IsNullOrEmpty(UserNameBox.Text) || userPasswordIsHided ?
            String.IsNullOrEmpty(UserPasswordPasswordBox.Password) : String.IsNullOrEmpty(UserPasswordTextBox.Text))
            {
                MessageBox.Show("Поля не заполнены.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (User.CheckName(UserNameBox.Text))
            {
                MessageBox.Show("Пользователь с таким именем уже определен в системе.", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            User newUser = User.CreateNewUser(UserNameBox.Text, userPasswordIsHided ? UserPasswordPasswordBox.Password : UserPasswordTextBox.Text);

            DataWindow newWindow = new DataWindow(newUser.Name);

            newWindow.Show();
            Close();
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            Bool passwordIsHided = UserPasswordPasswordBox.Visibility == Visibility.Visible;

            if (passwordIsHided)
            {
                UserPasswordPasswordBox.Visibility = Visibility.Hidden;

                UserPasswordTextBox.Visibility = Visibility.Visible;
            }

            else
            {
                UserPasswordPasswordBox.Visibility = Visibility.Visible;

                UserPasswordTextBox.Visibility = Visibility.Hidden;
            }
        }
    }
}
