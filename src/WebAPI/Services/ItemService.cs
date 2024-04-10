using AutoMapper;
using Domain.Data.Models;
using Domain.Migrations;
using WebAPI.Managers.Interfaces;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    //В сервисах логика сохранения картинки на диск
    public class ItemService : IItemService
    {
        //Спросить про это у Артура
        IWebHostEnvironment _appEnvironment;
        private readonly IItemManager _itemManager;
        private readonly IMapper _mapper;

        public ItemService(IWebHostEnvironment appEnvironment, IItemManager itemManager, IMapper mapper)
        {
            _appEnvironment = appEnvironment;
            _itemManager = itemManager;
            _mapper = mapper;
        }

        public async Task CreateItem(ItemRequestModel itemCM)
        {
            //FileService для работы с файлами
            //Альтернативный путь для файлов
            string path = "/Files/" + itemCM.File.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await itemCM.File.CopyToAsync(fileStream);
            }

            var item = _mapper.Map<Item>(itemCM);
            await _itemManager.CreateItem(item);
        }

        public async Task<IEnumerable<ItemResponseModel>> GetAllItems()
        {
            var items = await _itemManager.GetItems();

            //return File(getOrderControlsWithValuesResult.Data.Content, MediaTypeNames.Application.Octet, getOrderControlsWithValuesResult.Data.Name);

            //var itemsCM = items.Select(x =>
            //{
            //    byte[] buffer;
            //    string path = _appEnvironment.WebRootPath + x.FilePath;
            //    //using (FileStream fstream = File.OpenRead(path))
            //    //{
            //    //    // выделяем массив для считывания данных из файла
            //    //    //buffer = new byte[fstream.Length];
            //    //    //// считываем данные
            //    //    //await fstream.ReadAsync(buffer, 0, buffer.Length);
            //    //    //var file = File()
            //    //    return new ItemCM()
            //    //    {
            //    //        Text = x.Text,
            //    //        File = new FormFile(fstream, 0, fstream.Length, x.FilePath, x.FilePath)
            //    //    };
            //    //}
            //});

            //Запускать параллельно 
            //Guid для имя файла
            var itemsModels = new List<ItemResponseModel>();

            foreach (var item in items)
            {
                string path = _appEnvironment.WebRootPath + "/Files/" + item.FilePath;
                var bytes = await File.ReadAllBytesAsync(path);
                var newItemCM = new ItemResponseModel()
                {
                    Text = item.Text,
                    FileContent = bytes
                };

                itemsModels.Add(newItemCM);
            }

            return itemsModels;
        }
    }
}

