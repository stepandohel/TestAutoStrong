using Domain.Data.Models;

namespace WebAPI.Managers.Interfaces
{
    public interface IItemRepository
    {
        Task CreateItem(Item item, CancellationToken ct = default);
        Task<ICollection<Item>> GetItems(CancellationToken ct = default);
        Task<Item> GetItem(int id, CancellationToken ct = default);
        Task<string?> DeleteItem(int id, CancellationToken ct = default);
        Task<string?> UpdateItem(int id, Item newItem, CancellationToken ct = default);
    }
}
