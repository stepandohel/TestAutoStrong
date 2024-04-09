using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        public ItemVM ItemVM { get; set; } = new() { Text=string.Empty };
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

            //imgPreview.Source = bitMapImg;

            //byte[] data;
            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bitMapImg));
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    encoder.Save(ms);
            //    data = ms.ToArray();
            //}
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            //Получаю байты с картинки
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitMapImg));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            var client = new HttpClient();
            
            //К моделе по хорошему байндинг
            var item = new ItemVM()
            {
                Text = "text",
                FileName = FileName,
                FileContent = data
            };

            var stream = new MemoryStream(data);

            //Формирую бади
            var requestUrl = ItemEndpoints.CreateItemRoute();
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileStreamContent = new StreamContent(stream);
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
                multipartFormContent.Add(fileStreamContent, "File", FileName);

                    var folderIdContent = new StringContent("richText");
                    multipartFormContent.Add(folderIdContent, "Text");

                var response = await client.PostAsync(requestUrl, multipartFormContent);
            }
            }
    }
}
