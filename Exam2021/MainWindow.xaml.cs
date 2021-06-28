using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Bool = System.Boolean;

/// <summary>
/// Область кода с окном Входа.
/// </summary>
namespace Exam2021
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "ShowPasswordButton".
        /// Нужно для отображения пароля.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            Bool passwordIsHide = UserPasswordBox.Visibility == Visibility.Visible;

            if (passwordIsHide)
            {
                UserPasswordBox.Visibility = Visibility.Hidden;

                UserPasswordBox_Hidden.Visibility = Visibility.Visible;
            }

            else
            {
                UserPasswordBox.Visibility = Visibility.Visible;

                UserPasswordBox_Hidden.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "EnterInAccountButton".
        /// Выполняет проверку аккаунта на существование и входит в него.
        /// Если аккаунта нет, предлагает пользователю создать его.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EnterInAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Bool passwordIsHide = UserPasswordBox.Visibility == Visibility.Visible;

            User check = new User(UserNameBox.Text, passwordIsHide ?
            UserPasswordBox.Password : UserPasswordBox_Hidden.Text, new List<Employer>(1));

            if (User.AllUsers.Count > 0 && check.CheckUser())
            {
                DataWindow newWindow = new DataWindow(check.Name);

                newWindow.Show();
                Close();
            }

            else
            {
                if (MessageBox.Show("Пользователь не найден.\n\nСоздать аккаунт?", "Ошибка!", 
                MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    RegisterWindow newWindow = new RegisterWindow();

                    newWindow.Show();
                    Close();
                }
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "RegisterNewAccountButton".
        /// Нужно для перехода к окну создания аккаунта.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void RegisterNewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow newWindow = new RegisterWindow();

            newWindow.Show();
            Close();
        }

        /// <summary>
        /// Событие, возникающее при изменении пароля в "UserPasswordBox".
        /// Нужно для синхронизации введенных паролей между элементами управления.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UserPasswordBox.Visibility == Visibility.Visible)
            {
                UserPasswordBox_Hidden.Text = UserPasswordBox.Password;
            }
        }

        /// <summary>
        /// Событие, возникающее при изменении текста в "UserPasswordBox_Hidden".
        /// Нужно для синхронизации введенного пароля между элементами управления.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPasswordBox_Hidden_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UserPasswordBox_Hidden.Visibility == Visibility.Visible)
            {
                UserPasswordBox.Password = UserPasswordBox_Hidden.Text;
            }
        }
    }
}
