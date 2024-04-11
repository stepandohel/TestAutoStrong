using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using WpfClientApp;
using WpfClientApp.Endpoints;
using WpfClientApp.Modeles;
using WpfClientApp.NewFolder;

namespace WpfClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ItemVM> _items { get; set; } = new();
        public MainWindow()
        {  
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var createForm = new CreateItemForm(_items, null);

            createForm.ShowDialog();
        }

        async void OnLoadAsync(object sender, RoutedEventArgs e)
        {
            var httpClient = new ItemHttpClient();
            var items = await httpClient.GetItems();
            _items = items;
            this.ItemBox.ItemsSource = items;
        }     

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var httpClient = new ItemHttpClient();
            var items = await httpClient.GetItems();
            _items = items; 
            this.ItemBox.ItemsSource = items;
        }
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            //Через команды лучше
            //var id = ((Button)sender).Tag;
            var createForm = new CreateItemForm(_items, null);

            createForm.ShowDialog();
        }

        private void Edit_Handler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var createForm = new CreateItemForm(_items, null);

            createForm.ShowDialog();
        }
    }

    //public class EditCommand
    //{
    //    public EditCommand()
    //    {
    //        ZXC = new RoutedCommand("Edit", typeof(MainWindow));
    //    }
    //    public static RoutedCommand ZXC { get; set; }

    //    private static RoutedUICommand requery;
    //    public static RoutedUICommand Requery
    //    {
    //        get { return requery; }
    //    }
    //}
}