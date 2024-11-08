using petshop.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace petshop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminLKPage.xaml
    /// </summary>
    public partial class AdminLKPage : Page
    {
        public AdminLKPage()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            ProductListView.ItemsSource = Data.TradeEntities.GetContext().Product.ToList();
            var manufactList = Data.TradeEntities.GetContext().Produser.ToList();
            manufactList.Insert(0, new Data.Produser { Name = "Все производители" });
            ManufacturerComboBox.ItemsSource = manufactList;
            ManufacturerComboBox.SelectedIndex = 0;

            if (Manager.CurrentUser != null)
            {
                FIOLable.Visibility = Visibility.Visible;
                FIOLable.Content = $"{Manager.CurrentUser.UserSurname} {Manager.CurrentUser.UserName} {Manager.CurrentUser.UserPatronymic}";
            }
            else
            {
                FIOLable.Visibility = Visibility.Hidden;
            }

            CountOfLable.Content = $"{Data.TradeEntities.GetContext().Product.Count()}/{Data.TradeEntities.GetContext().Product.Count()}";
        }

        public List<Data.Product> _currentProduct = Data.TradeEntities.GetContext().Product.ToList();

        public void Update()
        {
            try
            {
                _currentProduct = Data.TradeEntities.GetContext().Product.ToList();
                _currentProduct = (from item in _currentProduct
                                   where item.ProductName.Name.ToLower().Contains(SearchTexBox.Text.ToLower()) ||
                                   item.ProductDescription.ToLower().Contains(SearchTexBox.Text.ToLower()) ||
                                   item.Produser.Name.ToLower().Contains(SearchTexBox.Text.ToLower()) ||
                                   item.ProductCost.ToString().ToLower().Contains(SearchTexBox.Text.ToLower()) ||
                                   item.ProductQuantityInStock.ToString().ToLower().Contains(SearchTexBox.Text.ToLower())
                                   select item).ToList();

                if (SortUpRadioButton.IsChecked == true)
                {
                    _currentProduct = _currentProduct.OrderBy(d => d.ProductCost).ToList();
                }
                if (SortDownRadioButton.IsChecked == true)
                {
                    _currentProduct = _currentProduct.OrderByDescending(d => d.ProductCost).ToList();
                }

                var selected = ManufacturerComboBox.SelectedItem as Data.Produser;
                if (selected != null && selected.Name != "Все производители")
                {
                    _currentProduct = _currentProduct.Where(d => d.Produser.Id == selected.Id).ToList();
                }

                CountOfLable.Content = $"{_currentProduct.Count}/{Data.TradeEntities.GetContext().Product.Count()}";

                ProductListView.ItemsSource = _currentProduct;
            }
            catch (Exception)
            {

            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void SortUpRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void SortDownRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.AddEditPage()));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = (sender as Button).DataContext as Data.Product;
                var orderProduct = Data.TradeEntities.GetContext().OrderProduct.Where(d => d.IdOrder == selected.Id).ToList();

                if (orderProduct.Count() > 0)
                {
                    MessageBox.Show("товар из заказа удалить нельзя!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {

                    Data.TradeEntities.GetContext().Product.Remove(selected);
                    Data.TradeEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно удалено!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Update();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Ошибка!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.MainFrame.CanGoBack)
            {
                if (Manager.CurrentUser != null)
                {
                    Manager.CurrentUser = null;
                }

                Manager.MainFrame.GoBack();
            }
        }

        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.AddEditPage()));
        }
    }
}
