using Domain.Data.Models;

namespace WebAPI.Managers.Interfaces
{
    public interface IItemManager
    {
        Task CreateItem(Item item, CancellationToken ct = default);
        Task<ICollection<Item>> GetItems(CancellationToken ct = default);
    }
}
