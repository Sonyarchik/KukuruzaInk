using KukuruzaInk.DATABASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KukuruzaInk
{
    /// <summary>
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        public AddClient()
        {
            InitializeComponent();
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FarmDeliveryDbContext())
                {
                    string newName = TextBoxName.Text;

                    var user = db.Users.FirstOrDefault(u => u.Name == newName);
                    if (user != null)
                    {
                        user.Email = TextBoxEmail.Text;
                        user.Phone = TextBoxPhone.Text;
                        user.Address = TextBoxAdress.Text;

                        db.SaveChanges();
                        MessageBox.Show("Данные клиента успешно изменены!", "Сообщение системы", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так! Попробуйте еще раз позже", "Сообщение системы", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_AddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FarmDeliveryDbContext())
                {
                    string newName = TextBoxName.Text;

                    // Проверяем, нет ли уже клиента с таким именем в базе данных
                    var existingUser = db.Users.FirstOrDefault(u => u.Name == newName);

                    if (existingUser == null)
                    {
                        // Создаем нового пользователя
                        var newUser = new User
                        {
                            Name = TextBoxName.Text,
                            Email = TextBoxEmail.Text,
                            Phone = TextBoxPhone.Text,
                            Address = TextBoxAdress.Text
                        };

                        // Добавляем нового пользователя в базу данных
                        db.Users.Add(newUser);
                        db.SaveChanges();

                        // Оповещаем пользователя об успешном добавлении
                        MessageBox.Show("Новый клиент успешно добавлен!", "Сообщение системы", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Оповещаем пользователя, что клиент с таким именем уже существует
                        MessageBox.Show("Клиент с таким именем уже существует!", "Сообщение системы", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так! Попробуйте еще раз позже","Сообщение системы", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
