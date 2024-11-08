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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public string FlagAddOrEdit = "default";
        public Data.Product _currentProduct = new Data.Product();
        public AddEditPage(Data.Product product)
        {
            InitializeComponent();

            if (product != null)
            {

                _currentProduct = product;
                FlagAddOrEdit = "edit";
            }
            else
            {
                FlagAddOrEdit = "add";
            }
            DataContext = _currentProduct;

            Init();
        }
        public void Init()
        {
            try
            {
                CategoryComboBox.ItemsSource = Data.TradeEntities.GetContext().Category.ToList();

                if (FlagAddOrEdit == "add")
                {
                    IdTextBox.Visibility = Visibility.Hidden;
                    IdLabel.Visibility = Visibility.Hidden;
                    CategoryComboBox.SelectedItem = null;
                    CountTextBox.Text = String.Empty;
                    UnitTextBox.Text = String.Empty;
                    NameTextBox.Text = String.Empty;
                    CostTextBox.Text = String.Empty;
                    SupplierTextBox.Text = String.Empty;
                    DescriptionTextBox.Text = String.Empty;
                }
                else if (FlagAddOrEdit == "edit")
                {
                    IdTextBox.Visibility = Visibility.Visible;
                    IdLabel.Visibility = Visibility.Visible;

                    CategoryComboBox.SelectedItem = _currentProduct;
                    CountTextBox.Text = _currentProduct.ProductQuantityInStock.ToString();
                    UnitTextBox.Text = _currentProduct.ProductStatus;  
                    NameTextBox.Text = _currentProduct.ProductName.Name;
                    CostTextBox.Text = _currentProduct.ProductCost.ToString();
                    SupplierTextBox.Text = _currentProduct.Produser.Name;
                    DescriptionTextBox.Text = _currentProduct.ProductDescription;

                    IdTextBox.Text = Data.TradeEntities.GetContext().Product.Max(d => d.ProductArticleNumber + 1).ToString();
                    CategoryComboBox.SelectedItem = Data.TradeEntities.GetContext().Category.Where(d => d.Id == _currentProduct.IdProductCategory).FirstOrDefault();
                }

            }
            catch (Exception)
            {

            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
        }

            private void ProductImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Логика открытия диалогового окна
        }
    }
}
