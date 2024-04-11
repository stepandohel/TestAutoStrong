using AutoMapper;
using Domain.Data.Models;
using Domain.Migrations;
using Server.Shared.Models;
using WebAPI.Managers.Interfaces;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    //В сервисах логика сохранения картинки на диск
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemManager;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemManager, IMapper mapper, IFileService fileService)
        {
            _itemManager = itemManager;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task CreateItem(ItemRequestModel itemRequestModel, CancellationToken ct = default)
        {
            //Альтернативный путь для файлов
            var fileGuid = Guid.NewGuid();
            var fileExtension = Path.GetExtension(itemRequestModel.File.FileName);
            var filePath = fileGuid + fileExtension;

            await _fileService.SaveFile(itemRequestModel.File, filePath);
            var item = _mapper.Map<Item>(itemRequestModel);
            item.FilePath = filePath;
            await _itemManager.CreateItem(item, ct);
        }

        public async Task DeleteItem(int id, CancellationToken ct = default)
        {
            await _itemManager.DeleteItem(id);
            //Надо еще файл удалять
        }

        public async Task<IEnumerable<ItemResponseModel>> GetAllItems(CancellationToken ct = default)
        {
            var items = await _itemManager.GetItems(ct);
            var itemsModels = new List<ItemResponseModel>();

            await Parallel.ForEachAsync(items, async (item, ct) =>
                {
                    var bytes = await _fileService.ReadFileBytes(item.FilePath);
                    var newItemCM = new ItemResponseModel()
                    {
                        Id = item.Id,
                        Text = item.Text,
                        FileContent = bytes
                    };

                    itemsModels.Add(newItemCM);
                });

            //foreach (var item in items)
            //{
            //    var bytes = await _fileService.ReadFileBytes(item.FilePath);
            //    var newItemCM = new ItemResponseModel()
            //    {
            //        Text = item.Text,
            //        FileContent = bytes
            //    };

            //    itemsModels.Add(newItemCM);
            //}

            return itemsModels;
        }

        public async Task<ItemResponseModel> GetItem(int id, CancellationToken ct = default)
        {
            var item = await _itemManager.GetItem(id, ct);
            var itemResponse = _mapper.Map<ItemResponseModel>(item);
            var bytes = await _fileService.ReadFileBytes(item.FilePath);
            itemResponse.FileContent = bytes;
            return itemResponse;
        }
    }
}

