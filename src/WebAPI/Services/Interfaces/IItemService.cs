using Server.Shared.Models;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IItemService
    {
        Task CreateItem(ItemRequestModel itemCM, CancellationToken ct = default);
        Task<IEnumerable<ItemResponseModel>> GetAllItems(CancellationToken ct = default);
        Task<ItemResponseModel> GetItem(int id, CancellationToken ct = default);
        Task DeleteItem(int id, CancellationToken ct = default);
    }
}
