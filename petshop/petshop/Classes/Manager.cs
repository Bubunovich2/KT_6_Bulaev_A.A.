using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace petshop.Classes
{
    public static class Manager
    {
        public static Frame MainFrame { get; set; }
        public static Data.User CurrentUser { get; set; }

        public static void Img()
        {
            try
            {
                var list = Data.TradeEntities.GetContext().Product.ToList();
                foreach (var item in list)
                {
                    string path = Directory.GetCurrentDirectory() + @"\Image\" + item.Img;
                    if (File.Exists(path))
                    {
                        item.ProductImage = File.ReadAllBytes(path);
                    }

                }
                Data.TradeEntities.GetContext().SaveChanges();
            }
            catch (Exception) { }
        }
    }
}
