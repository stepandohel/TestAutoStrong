using Domain.Data;
using Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Managers.Interfaces;

namespace WebAPI.Managers
{
    public class ItemManager : IItemManager
    {
        private readonly AppDBContext _context;

        public ItemManager(AppDBContext context)
        {
            _context = context;
        }

        public async Task CreateItem(Item item, CancellationToken ct = default)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<ICollection<Item>> GetItems(CancellationToken ct = default)
        {
            var items = await _context.Items.AsNoTracking().ToListAsync(ct);
            return items;
        }
    }
}
