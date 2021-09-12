using DynamicButtons.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.Storage;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DynamicButtons.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<InventoryItem> Inventory { get; set; }
        public InventoryItem SelectedProduct { get; set; }
        public ObservableCollection<Product> Cart { get; set; }
        public Product SelectedInCart { get; set; }
        public string Item { get; set; }

        public string Subtotal => $"Subtotal {Cart.Sum(i => i.Price):C}";
        public string Tax => $"Tax {Cart.Sum(i => i.Price) * 0.07:C}";
        public string Total => $"Total {(Cart.Sum(i => i.Price)*0.07) + Cart.Sum(i => i.Price):C}";

        public MainViewModel()
        {
            Inventory = new ObservableCollection<InventoryItem>();
            Cart = new ObservableCollection<Product>();
            Item = "";
        }

        public async void AddToCart()
        {
            
            var handler = new WebRequestHandler();
            await handler.Post("http://localhost/shoppingCart/cart/add", SelectedProduct);
            var cartItems = JsonConvert.DeserializeObject<List<Product>>(handler.Get("http://localhost/shoppingCart/cart/GetCart").Result);
            Cart.Clear();
            cartItems.ForEach(Cart.Add);
            SelectedProduct = null;
      
            NotifyPropertyChanged("SelectedProduct");
            NotifyPropertyChanged("Subtotal");
            NotifyPropertyChanged("Tax");
            NotifyPropertyChanged("Total");
        }

        public async void RemoveFromCart()
        {
            var handler = new WebRequestHandler();
            await handler.Post("http://localhost/shoppingCart/cart/delete", SelectedInCart);
            var cartItems = JsonConvert.DeserializeObject<List<Product>>(handler.Get("http://localhost/shoppingCart/cart/GetCart").Result);
            Cart.Clear();
            cartItems.ForEach(Cart.Add);

            SelectedInCart = null;
            NotifyPropertyChanged("SelectedInCart");
            NotifyPropertyChanged("Subtotal");
            NotifyPropertyChanged("Tax");
            NotifyPropertyChanged("Total");
        }

        public void ClearCart()
        {
            var handler = new WebRequestHandler();
            var cartItems = JsonConvert.DeserializeObject<List<Product>>(handler.Get("http://localhost/shoppingCart/cart/clear").Result);
            Cart.Clear();
            cartItems.ForEach(Cart.Add);

            NotifyPropertyChanged("Subtotal");
            NotifyPropertyChanged("Tax");
            NotifyPropertyChanged("Total");
        }

        public void CheckoutCart()
        {
            StreamWriter file = new StreamWriter($@"{AppDataPaths.GetDefault().LocalAppData}\\receipt.txt", true, Encoding.ASCII);
            var handler = new WebRequestHandler();
            string output = handler.Get("http://localhost/shoppingCart/cart/checkout").Result;
            file.WriteLine(output);
            file.Close();
            if (!File.Exists($"{AppDataPaths.GetDefault().LocalAppData}\\saveFile.txt"))
            {
                File.WriteAllText($"{AppDataPaths.GetDefault().LocalAppData}\\saveFile.txt", JsonConvert.SerializeObject(this));
            }
            Application.Current.Exit();
        }

        public async void Search_Products()
        {

            var handler = new WebRequestHandler();
            await handler.Post("http://localhost/shoppingCart/inv/search", Item);
            var invItems = JsonConvert.DeserializeObject<List<InventoryItem>>(handler.Get("http://localhost/shoppingCart/inv/searchResults").Result);
            Inventory.Clear();
            invItems.ForEach(Inventory.Add);
            NotifyPropertyChanged("Subtotal");
            NotifyPropertyChanged("Tax");
            NotifyPropertyChanged("Total");
        }
        public async void ClearSearch()
        {
            var handler = new WebRequestHandler();
            await handler.Get("http://localhost/shoppingCart/inv/clear");
            var invItems = JsonConvert.DeserializeObject<List<InventoryItem>>(handler.Get("http://localhost/shoppingCart/cart/GetInv").Result);
            Inventory.Clear();
            invItems.ForEach(Inventory.Add);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
