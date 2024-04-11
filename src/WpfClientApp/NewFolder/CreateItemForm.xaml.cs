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
                SubmitBtn.Click -= Button_Click_1;
                SubmitBtn.Click += Update_Click;
                //ItemVM = itemVM;
                ItemVM.Id = itemVM.Id;
                ItemVM.Text = itemVM.Text;
                ItemVM.BitmapImage = itemVM.BitmapImage;
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
            var createdItemId = await httpClient.SendItem(ItemVM);
            ItemVM.Id = createdItemId;
            _items.Add(ItemVM);
            this.Close();
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            var httpClient = new ItemHttpClient();
            var createdItemId = await httpClient.UpdateItem(ItemVM);

            var itemFromSource = _items.First(x=>x.Id.Equals(ItemVM.Id));
            itemFromSource.Text = ItemVM.Text;
            itemFromSource.BitmapImage = ItemVM.BitmapImage;
            this.Close();
        }
    }
}
