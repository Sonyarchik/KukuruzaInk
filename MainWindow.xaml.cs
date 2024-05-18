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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Window
    {
        public OrderPage()
        {
            InitializeComponent();

            using (var db = new FarmDeliveryDbContext())
            {
                ComboboxCustomers.Items.Clear();
                ComboboxProduct1.Items.Clear();
                ComboboxProduct2.Items.Clear();
                ComboboxProduct3.Items.Clear();

                ComboboxCustomers.ItemsSource = db.Users.Select(u => u.Name).ToList();
                ComboboxProduct1.ItemsSource = db.Products.Select(p => p.Name).ToList();
                ComboboxProduct2.ItemsSource = db.Products.Select(p => p.Name).ToList();
                ComboboxProduct3.ItemsSource = db.Products.Select(p => p.Name).ToList();
            }
        }

        private void printlabel()
        {
            double totalPrice = 0;

            using (var db = new FarmDeliveryDbContext())
            {
                double price1 = GetPrice(db, ComboboxProduct1.SelectedItem.ToString());
                double price2 = GetPrice(db, ComboboxProduct2.SelectedItem.ToString());
                double price3 = GetPrice(db, ComboboxProduct3.SelectedItem.ToString());

                totalPrice = price1 + price2 + price3;

                LabelTotalPrice.Content = totalPrice.ToString("C");
            }
        }

        private double GetPrice(FarmDeliveryDbContext db, string productName)
        {
            double price = 0.0;

            var product = db.Products.FirstOrDefault(p => p.Name == productName);
            if (product != null)
            {
                price = Convert.ToDouble(product.Price);
            }

            return price;
        }


        private void MakeOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FarmDeliveryDbContext())
                {
                    var customerLastName = ComboboxCustomers.SelectedItem.ToString();
                    var customerId = db.Users.FirstOrDefault(c => c.Name == customerLastName)?.UserId;

                    if (customerId != null)
                    {
                        var order = new Order
                        {
                            UserId = customerId.Value,
                            OrderDate = DateTime.Today.Date,
                            StatusId = 1 // ID статуса "В обработке"
                        };
                        db.Orders.Add(order);
                        db.SaveChanges();

                        int orderId = order.OrderId;

                        var productIds = new List<int>();
                        foreach (var combobox in new List<ComboBox> { ComboboxProduct1, ComboboxProduct2, ComboboxProduct3 })
                        {
                            var productName = combobox.SelectedItem.ToString();
                            var productId = db.Products.FirstOrDefault(p => p.Name == productName)?.ProductId;
                            if (productId != null)
                            {
                                productIds.Add(productId.Value);
                            }
                        }

                        var orderDetail = new OrderDetail
                        {
                            OrderId = orderId,
                            ProductId = productIds.Count > 0 ? productIds[0] : 0,
                            ProductId2 = productIds.Count > 1 ? productIds[1] : 0,
                            ProductId3 = productIds.Count > 2 ? productIds[2] : 0
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                        MessageBox.Show("Данные о заказе успешно добавлены!", "Сообщение системы", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Перепроверьте данные", "Что-то пошло не так!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCount_Click(object sender, RoutedEventArgs e)
        {
            printlabel();
        }

        private void ButtonNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddClient client = new AddClient();
            client.ShowDialog();
        }

        private void ButtonHistory_Click(object sender, RoutedEventArgs e)
        {
            OrdersHistory history = new OrdersHistory();
            history.ShowDialog();
        }

        private void ButtonNewRefresh_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new FarmDeliveryDbContext())
            {
                ComboboxCustomers.ItemsSource = db.Users.Select(u => u.Name).ToList();
            }
        }
    }
}
