using System.Collections.ObjectModel;
using System.Windows;
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
            DataContext = _items;
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

        private void Edit_Handler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var itemId = (int)e.Parameter;
            var itemForUpdate = _items[itemId-1];
            var createForm = new CreateItemForm(_items, itemForUpdate);

            createForm.ShowDialog();
        }
    }
}