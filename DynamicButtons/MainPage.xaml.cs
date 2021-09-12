using DynamicButtons.ViewModels;
using DynamicButtons.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DynamicButtons
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
            var handler = new WebRequestHandler();
            var items = JsonConvert.DeserializeObject<List<InventoryItem>>(handler.Get("http://localhost/shoppingCart/cart/GetInv").Result);
            var cartItems = JsonConvert.DeserializeObject<List<Product>>(handler.Get("http://localhost/shoppingCart/cart/GetCart").Result);
            var context = DataContext as MainViewModel;

            items.ForEach(context.Inventory.Add);
            cartItems.ForEach(context.Cart.Add);
        }

        private void Inventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MainViewModel).AddToCart();
        }

        private void Cart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MainViewModel).RemoveFromCart();
        }

        private void Checkout(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).CheckoutCart();
        }

        private void ClearItems(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).ClearCart();
        }

        private void Clear_Search(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).ClearSearch();
        }
        private  void Search(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Search_Products();
        }

    }
}
