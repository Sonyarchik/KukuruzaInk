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
    /// Логика взаимодействия для OrdersHistory.xaml
    /// </summary>
    public partial class OrdersHistory : Window
    {
        public OrdersHistory()
        {
            InitializeComponent();
            PrintDatagrid();
        }

        private void PrintDatagrid()
        {
            using var db = new FarmDeliveryDbContext();

            var orders = from o in db.Orders
                         join od in db.OrderDetails on o.OrderId equals od.OrderId
                         join u in db.Users on o.UserId equals u.UserId
                         join p1 in db.Products on od.ProductId equals p1.ProductId into prod1
                         from p1 in prod1.DefaultIfEmpty()
                         join p2 in db.Products on od.ProductId2 equals p2.ProductId into prod2
                         from p2 in prod2.DefaultIfEmpty()
                         join p3 in db.Products on od.ProductId3 equals p3.ProductId into prod3
                         from p3 in prod3.DefaultIfEmpty()
                         join s in db.Statuses on o.StatusId equals s.StatusId
                         select new
                         {
                             Имя__клиента = u.Name,
                             Товар1 = p1.Name,
                             Цена1 = p1 != null ? p1.Price : 0, // Здесь используется проверка на null и возвращается 0 при отсутствии значения
                             Товар2 = p2.Name,
                             Цена2 = p2 != null ? p2.Price : 0,
                             Товар3 = p3.Name,
                             Цена3 = p3 != null ? p3.Price : 0,
                             Статус = s.StatusName,
                         };

            DataGridOrders.ItemsSource = orders.ToList();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = TextBoxSearch.Text; // Получаем текст для поиска из TextBoxSearch

                using (var db = new FarmDeliveryDbContext())
                {
                    var filteredOrders = from o in db.Orders
                                         join od in db.OrderDetails on o.OrderId equals od.OrderId
                                         join u in db.Users on o.UserId equals u.UserId
                                         join p1 in db.Products on od.ProductId equals p1.ProductId into prod1
                                         from p1 in prod1.DefaultIfEmpty()
                                         join p2 in db.Products on od.ProductId2 equals p2.ProductId into prod2
                                         from p2 in prod2.DefaultIfEmpty()
                                         join p3 in db.Products on od.ProductId3 equals p3.ProductId into prod3
                                         from p3 in prod3.DefaultIfEmpty()
                                         join s in db.Statuses on o.StatusId equals s.StatusId
                                         where u.Name.Contains(searchTerm) // Фильтруем по имени клиента, содержащему текст из TextBoxSearch
                                         select new
                                         {
                                             Имя_клиента = u.Name,
                                             Товар1 = p1.Name,
                                             Цена1 = p1 != null ? p1.Price : 0,
                                             Товар2 = p2.Name,
                                             Цена2 = p2 != null ? p2.Price : 0,
                                             Товар3 = p3.Name,
                                             Цена3 = p3 != null ? p3.Price : 0,
                                             Статус = s.StatusName,
                                         };

                    DataGridOrders.ItemsSource = filteredOrders.ToList(); // Устанавливаем отфильтрованные заказы в DataGridOrders
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При поиске пошло что-то не так, пожалуйста, попробуйте еще раз!","Ошибка системы",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            PrintDatagrid();
        }

        private void ButtonSort_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new FarmDeliveryDbContext())
            {
                var orders = from o in db.Orders
                             join od in db.OrderDetails on o.OrderId equals od.OrderId
                             join u in db.Users on o.UserId equals u.UserId
                             join p1 in db.Products on od.ProductId equals p1.ProductId into prod1
                             from p1 in prod1.DefaultIfEmpty()
                             join p2 in db.Products on od.ProductId2 equals p2.ProductId into prod2
                             from p2 in prod2.DefaultIfEmpty()
                             join p3 in db.Products on od.ProductId3 equals p3.ProductId into prod3
                             from p3 in prod3.DefaultIfEmpty()
                             join s in db.Statuses on o.StatusId equals s.StatusId
                             select new
                             {
                                 Имя__клиента = u.Name,
                                 Товар1 = p1.Name,
                                 Цена1 = p1 != null ? p1.Price : 0, // Здесь используется проверка на null и возвращается 0 при отсутствии значения
                                 Товар2 = p2.Name,
                                 Цена2 = p2 != null ? p2.Price : 0,
                                 Товар3 = p3.Name,
                                 Цена3 = p3 != null ? p3.Price : 0,
                                 Статус = s.StatusName,
                             };

                if (RadioButtonAya.IsChecked == true)
                {
                    orders = orders.OrderBy(o => o.Товар1).ThenBy(o => o.Товар2).ThenBy(o => o.Товар3);
                }
                else if (RadioButtonYaA.IsChecked == true)
                {
                    orders = orders.OrderByDescending(o => o.Товар1).ThenByDescending(o => o.Товар2).ThenByDescending(o => o.Товар3);
                }
                else if (RadiobuttonCosts.IsChecked == true)
                {
                    orders = orders.OrderByDescending(o => o.Цена1).ThenByDescending(o => o.Цена2).ThenByDescending(o => o.Цена3);
                }

                DataGridOrders.ItemsSource = orders.ToList();
            }
        }
    }
}
