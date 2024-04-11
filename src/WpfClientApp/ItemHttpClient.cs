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
            //ВЫнести????
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(item.BitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            var fileName = Path.GetFileName(item.BitmapImage.UriSource.OriginalString);

            var stream = new MemoryStream(data);
            var requestUrl = ItemEndpoints.ControllerRoute;
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileStreamContent = new StreamContent(stream);
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
                multipartFormContent.Add(fileStreamContent, "File", fileName);

                var textContent = new StringContent(item.Text);
                multipartFormContent.Add(textContent, "Text");

                var response = await _httpClient.PostAsync(requestUrl, multipartFormContent);
                response.EnsureSuccessStatusCode();
                var responseModel = await response.Content.ReadFromJsonAsync<ItemResponseModel>();

                return responseModel.Id;
            }
        }

        public async Task<int> UpdateItem(ItemVM item)
        {
            //ВЫнести????
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(item.BitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            var fileName = Path.GetFileName(item.BitmapImage.UriSource.OriginalString);

            var stream = new MemoryStream(data);
            var requestUrl = $"{ItemEndpoints.ControllerRoute}/{item.Id}";
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileStreamContent = new StreamContent(stream);
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
                multipartFormContent.Add(fileStreamContent, "File", fileName);

                var textContent = new StringContent(item.Text);
                multipartFormContent.Add(textContent, "Text");

                var response = await _httpClient.PutAsync(requestUrl, multipartFormContent);
                response.EnsureSuccessStatusCode();
                var responseModel = await response.Content.ReadFromJsonAsync<ItemResponseModel>();

                return responseModel.Id;
            }
        }

        public async Task<ObservableCollection<ItemVM>> GetItems()
        {
            var requestUrl = ItemEndpoints.ControllerRoute;
            var response = await _httpClient.GetFromJsonAsync<List<ItemResponseModel>>(requestUrl);

            // Это тоже в сервис
            var items = new ObservableCollection<ItemVM>();
            foreach (var item in response)
            {
                var itemVM = new ItemVM()
                {
                    Id = item.Id,
                    Text = item.Text
                };

                using (var ms = new System.IO.MemoryStream(item.FileContent))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                    itemVM.BitmapImage = image;
                }
                items.Add(itemVM);
            }

            return items;

        }

        //public Task ValidateResponce(HttpResponseMessage response)
        //{
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        return OK();
        //    }
        //}
    }
}
