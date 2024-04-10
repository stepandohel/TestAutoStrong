using WebAPI.Modeles;

namespace WebAPI.Services.Interfaces
{
    public interface IItemService
    {
        public Task SaveFile(ItemCM itemCM);
        public Task<IEnumerable<ItemCM>> GetAllItems();
    }
}
