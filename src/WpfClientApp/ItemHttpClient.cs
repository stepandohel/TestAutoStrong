using Server.Shared.Endpoints;
using Server.Shared.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Windows.Media.Imaging;
using WpfClientApp.Modeles;

namespace WpfClientApp
{
    public class ItemHttpClient
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri(@"https://localhost:7279/") };

        public async Task<int> SendItem(ItemVM item)
        {
            var data = GetBytesFromImage(item.BitmapImage);
            var fileName = Path.GetFileName(item.BitmapImage.UriSource.OriginalString);
            var requestUrl = ItemEndpoints.ControllerRoute;

            using (var formContent = CreateFormContent(data, fileName, item.Text))
            {
                var response = await _httpClient.PostAsync(requestUrl, formContent);

                response.EnsureSuccessStatusCode();
                var responseModel = await response.Content.ReadFromJsonAsync<ItemResponseModel>();

                return responseModel.Id;
            }
        }

        public async Task<int> UpdateItem(ItemVM item)
        {
            var data = GetBytesFromImage(item.BitmapImage);
            var fileName = Path.GetFileName(item.BitmapImage.UriSource?.OriginalString);
            var requestUrl = $"{ItemEndpoints.ControllerRoute}/{item.Id}";

            using (var formContent = CreateFormContent(data, fileName, item.Text))
            {
                var response = await _httpClient.PutAsync(requestUrl, formContent);

                response.EnsureSuccessStatusCode();
                var responseModel = await response.Content.ReadFromJsonAsync<ItemResponseModel>();

                return responseModel.Id;
            }
        }

        public async Task DeleteItem(int itemId)
        {
            var requestUrl = $"{ItemEndpoints.ControllerRoute}/{itemId}";
            var response = await _httpClient.DeleteAsync(requestUrl);

            response.EnsureSuccessStatusCode();
        }

        public async Task<ObservableCollection<ItemVM>> GetItems()
        {
            var requestUrl = ItemEndpoints.ControllerRoute;
            var response = await _httpClient.GetFromJsonAsync<List<ItemResponseModel>>(requestUrl);

            var itemsVM = response.Select(x =>
            {
                var itemVM = new ItemVM()
                {
                    Id = x.Id,
                    Text = x.Text
                };

                using (var ms = new System.IO.MemoryStream(x.FileContent))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                    itemVM.BitmapImage = image;
                }
                return itemVM;
            });

            var items = new ObservableCollection<ItemVM>(itemsVM);

            return items;
        }

        private byte[] GetBytesFromImage(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        private MultipartFormDataContent CreateFormContent(byte[] fileContent, string fileName, string textContentString)
        {
            //Не было времени написать нормальный обработчик на файлы, поэтому сделал так на случай если контент не меняется :((
            if (fileName == null)
            {
                fileName = "old Path.jpg";
            }
            var stream = new MemoryStream(fileContent);
            var multipartFormContent = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(stream);
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
            multipartFormContent.Add(fileStreamContent, "File", fileName);

            var textContent = new StringContent(textContentString);
            multipartFormContent.Add(textContent, "Text");

            return multipartFormContent;
        }
    }
}
