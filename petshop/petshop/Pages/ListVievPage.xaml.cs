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
    /// Логика взаимодействия для ListVievPage.xaml
    /// </summary>
    public partial class ListVievPage : Page
    {
        public ListVievPage()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            var produserList = Data.TradeEntities.GetContext().Produser.ToList();
            produserList.Insert(0, new Data.Produser { Name = "Все производители" });
            ProducerCombobox.ItemsSource = produserList;
            ProducerCombobox.SelectedIndex = 0;

            if (Classes.Manager.CurrentUser != null)
            {
                FIOLabel.Visibility = Visibility.Visible;
                FIOLabel.Content = $"{Classes.Manager.CurrentUser.UserSurname} " +
                    $"{Classes.Manager.CurrentUser.UserName} " +
                    $"{Classes.Manager.CurrentUser.UserPatronymic}";

            }
            else
            {
                FIOLabel.Visibility = Visibility.Hidden;
            }
        }


        public List<Data.Product> _currentproduct = Data.TradeEntities.GetContext().Product.ToList();

        public void Update()
        {
            try
            {
                _currentproduct = Data.TradeEntities.GetContext().Product.ToList();

                _currentproduct = (from item in Data.TradeEntities.GetContext().Product
                                   where item.ProductDescription.ToLower().Contains(SearchTextBox.Text) ||
                                    item.Produser.Name.ToLower().Contains(SearchTextBox.Text) ||
                                    item.ProductCost.ToString().ToLower().Contains(SearchTextBox.Text) ||
                                    item.ProductQuantityInStock.ToString().ToLower().Contains(SearchTextBox.Text)
                                    select item).ToList();

                if (SortUpRadioButton.IsChecked == true)
                {
                    _currentproduct = _currentproduct.OrderBy(d => d.ProductCost).ToList();
                }
                if (SortDownRadioButton.IsChecked == true)
                {
                    _currentproduct = _currentproduct.OrderByDescending(d => d.ProductCost).ToList();
                }
                var selected = ProducerCombobox.SelectedItem as Data.Produser;
               if (selected != null && selected.Name != "Все производители")
               {
                    _currentproduct = _currentproduct.Where(d => d.IdProduser == selected.Id).ToList();
               }
                ProductListView.ItemsSource = _currentproduct;
            }
            catch
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

        private void ProducerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Manager.MainFrame.CanGoBack)
            {
                if (Classes.Manager.CurrentUser != null)
                {
                    Classes.Manager.CurrentUser = null;
                }


                Classes.Manager.MainFrame.GoBack();
            }
        }

    }
}
