using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IItemService
    {
        public Task CreateItem(ItemRequestModel itemCM);
        public Task<IEnumerable<ItemResponseModel>> GetAllItems();
    }
}
