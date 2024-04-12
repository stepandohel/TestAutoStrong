using AutoMapper;
using Domain.Data.Models;
using Server.Shared.Models;
using WebAPI.Managers.Interfaces;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper, IFileService fileService)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ItemResponseModel> CreateItem(ItemRequestModel itemRequestModel, CancellationToken ct = default)
        {
            var fileNameGuid = Guid.NewGuid();
            var fileExtension = Path.GetExtension(itemRequestModel.File.FileName);
            var newFileName = fileNameGuid + fileExtension;

            await _fileService.SaveFile(itemRequestModel.File, newFileName);
            var item = _mapper.Map<Item>(itemRequestModel);
            item.FilePath = newFileName;
            await _itemRepository.CreateItem(item, ct);

            return _mapper.Map<ItemResponseModel>(item);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken ct = default)
        {
            var deletedFilePath = await _itemRepository.DeleteItem(id);
            if (deletedFilePath is null)
            {
                return false;
            }

            _fileService.DeleteFile(deletedFilePath);
            return true;
        }

        public async Task<IEnumerable<ItemResponseModel>> GetAllItems(CancellationToken ct = default)
        {
            var items = await _itemRepository.GetItems(ct);
            var itemsModels = new List<ItemResponseModel>();

            var tasks = items.Select(async x =>
            {
                var bytes = await _fileService.ReadFileBytes(x.FilePath);
                var newItemCM = new ItemResponseModel()
                {
                    Id = x.Id,
                    Text = x.Text,
                    FileContent = bytes
                };
                itemsModels.Add(newItemCM);
            });

            await Task.WhenAll(tasks);

            //await Parallel.ForEachAsync(items, async (item, ct) =>
            //    {
            //        var bytes = await _fileService.ReadFileBytes(item.FilePath);
            //        var newItemCM = new ItemResponseModel()
            //        {
            //            Id = item.Id,
            //            Text = item.Text,
            //            FileContent = bytes
            //        };

            //        itemsModels.Add(newItemCM);
            //    });

            return itemsModels;
        }

        public async Task<ItemResponseModel> GetItem(int id, CancellationToken ct = default)
        {
            var item = await _itemRepository.GetItem(id, ct);
            var itemResponse = _mapper.Map<ItemResponseModel>(item);

            var bytes = await _fileService.ReadFileBytes(item.FilePath);
            itemResponse.FileContent = bytes;

            return itemResponse;
        }

        public async Task<ItemResponseModel> UpdateItem(int id, ItemRequestModel itemRequestModel, CancellationToken ct = default)
        {
            var newItem = _mapper.Map<Item>(itemRequestModel);
            string newFileName = string.Empty;
            if (!itemRequestModel.File.FileName.Equals("old Path.jpg"))
            {
                var fileNameGuid = Guid.NewGuid();
                var fileExtension = Path.GetExtension(itemRequestModel.File.FileName);
                newFileName = fileNameGuid + fileExtension;
                newItem.FilePath = newFileName;
            }

            var oldFilePath = await _itemRepository.UpdateItem(id, newItem);

            if(oldFilePath is null)
            {
                return null;
            }
            if(!newFileName.Equals(string.Empty)) 
            {
                _fileService.DeleteFile(oldFilePath);
                _fileService.SaveFile(itemRequestModel.File, newFileName);
            }

            return _mapper.Map<ItemResponseModel>(newItem);
        }
    }
}

