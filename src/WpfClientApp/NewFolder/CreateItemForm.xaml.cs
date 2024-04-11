using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfClientApp.Modeles;

namespace WpfClientApp.NewFolder
{
    /// <summary>
    /// Interaction logic for CreateItemForm.xaml
    /// </summary>
    public partial class CreateItemForm : Window
    {
        public ItemVM ItemVM { get; set; } = new() { Text = string.Empty };
        private readonly ObservableCollection<ItemVM> _items;
        public CreateItemForm()
        {
            InitializeComponent();
        }

        public CreateItemForm(ObservableCollection<ItemVM> items, ItemVM itemVM)
        {
            InitializeComponent();
            _items = items;
            if (itemVM != null)
            {
                ItemVM = itemVM;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            var FilePath = openFileDialog.FileName;

            ItemVM.BitmapImage = new BitmapImage((new Uri(FilePath)));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var httpClient = new ItemHttpClient();
            await httpClient.SendItem(ItemVM);
            _items.Add(ItemVM);
            this.Close();
        }
    }
}
