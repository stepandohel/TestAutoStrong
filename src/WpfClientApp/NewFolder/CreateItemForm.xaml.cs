using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfClientApp.Endpoints;
using WpfClientApp.Modeles;
using Path = System.IO.Path;

namespace WpfClientApp.NewFolder
{
    /// <summary>
    /// Interaction logic for CreateItemForm.xaml
    /// </summary>
    public partial class CreateItemForm : Window
    {
        public BitmapImage bitMapImg { get; set; }
        public string FileName { get; set; }

        public ItemVM ItemVM { get; set; } = new() { Text = string.Empty };
        public CreateItemForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            var FilePath = openFileDialog.FileName;

            FileName = Path.GetFileName(FilePath);

            ItemVM.BitmapImage = new BitmapImage((new Uri(FilePath)));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            //Получаю байты с картинки
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(ItemVM.BitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            var client = new HttpClient();
            client.BaseAddress = new Uri(@"https://localhost:7279/");

            //К моделе по хорошему байндинг
            var item = new ItemVM()
            {
                Text = "text"
            };

            var stream = new MemoryStream(data);

            //Формирую бади
            var requestUrl = ItemEndpoints.ControllerRoute;
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileStreamContent = new StreamContent(stream);
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
                multipartFormContent.Add(fileStreamContent, "File", FileName);

                var textContent = new StringContent(ItemVM.Text);
                multipartFormContent.Add(textContent, "Text");

                var response = await client.PostAsync(requestUrl, multipartFormContent);
            }
        }
        public class ItemResponseModel
        {
            public string Text { get; set; }
            public byte[] FileContent { get; set; }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(@"https://localhost:7279/");
            var requestUrl = ItemEndpoints.ControllerRoute;
            var response = await client.GetFromJsonAsync<List<ItemResponseModel>>(requestUrl);


            using (var ms = new System.IO.MemoryStream(response.First().FileContent))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                imgPreview.Source = image;
            }

        }
    }
}
