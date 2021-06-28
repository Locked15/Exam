using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using Bool = System.Boolean;

namespace Exam2021
{
    /// <summary>
    /// Логика взаимодействия для DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        Bool searched = false;

        public User CurrentUser { get; set; }

        public DataWindow(String name)
        {
            CurrentUser = User.AllUsers.Where(user => user.Name == name).ToList()[0];

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewEmploy_Date.DisplayDateEnd = DateTime.Now;

            UpdateSheetToBase();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
            {
                searched = false;

                SearchBox.Text = "Имя (Поиск)/Номер (Удаление)";

                UpdateSheetToBase();
            }

            else
            {
                List<Employer> employers = new List<Employer>(1);

                employers = CurrentUser.Employeers.Where(Emp => Emp.Name.Contains(SearchBox.Text)).ToList();

                Sheet.ItemsSource = null;
                Sheet.ItemsSource = employers;

                searched = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(SearchBox.Text))
            {
                try
                {
                    CurrentUser.Employeers.RemoveAt(CurrentUser.Employeers.FindIndex(Emp => Emp.Number.ToString() == SearchBox.Text));

                    if (searched)
                    {
                        List<Employer> source = (List<Employer>)Sheet.ItemsSource;
                        source.RemoveAt(source.FindIndex(Emp => Emp.Number.ToString() == SearchBox.Text));

                        UpdateSheet(source);
                    }

                    else
                    {
                        UpdateSheetToBase();
                    }
                }

                catch
                {
                    MessageBox.Show("Пользователь с указанным индексом не найден.", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else
            {
                SearchBox.Text = "Имя (Поиск)/Номер (Удаление)";
            }
        }

        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.CreateExcelFile();
        }

        private void WordButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.CreateWordFile();
        }

        private void AddNewEmployer_Click(object sender, RoutedEventArgs e)
        {
            Int32 number;
            Place place;
            String name = NewEmploy_Name.Text;
            DateTime birth = NewEmploy_Date.SelectedDate.Value;

            if (Int32.TryParse(NewEmploy_Number.Text, out number) && !String.IsNullOrEmpty(name)
            && Enum.TryParse(NewEmploy_Place.Text, out place) &&
            CurrentUser.Employeers.Where(Emp => Emp.Number == number).Count() == 0)
            {
                CurrentUser.Employeers.Add(new Employer(number, name, birth, place));

                NewEmploy_Name.Text = "(Имя)";
                NewEmploy_Number.Text = "(Номер)";
                NewEmploy_Date.SelectedDate = DateTime.Now;
                NewEmploy_Place.SelectedItem = null;

                UpdateSheetToBase();
            }

            else if (CurrentUser.Employeers.Where(Emp => Emp.Number == number).Count() > 0)
            {
                MessageBox.Show("Сотрудник с данным номером уже существует.", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                MessageBox.Show("При попытке создать нового сотрудника возникла ошибка.", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSheet(IEnumerable<Employer> collection)
        {
            Sheet.ItemsSource = null;
            Sheet.ItemsSource = collection;
        }

        private void UpdateSheetToBase()
        {
            Sheet.ItemsSource = null;
            Sheet.ItemsSource = CurrentUser.Employeers;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            User.SaveChanges();
        }
    }
}
