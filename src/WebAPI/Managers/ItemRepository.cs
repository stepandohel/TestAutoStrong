using Domain.Data;
using Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Managers.Interfaces;

namespace WebAPI.Managers
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDBContext _context;

        public ItemRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task CreateItem(Item item, CancellationToken ct = default)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync(ct);
        }

        //asdsadsadsadasdsada
        public async Task DeleteItem(int id, CancellationToken ct = default)
        {
            var item = await _context.Items.FindAsync(id);

            if (item is not null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync(ct);
            }
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!
        public async Task<Item> GetItem(int id, CancellationToken ct = default)
        {
            var item = await _context.Items.FindAsync(id);

            if (item is not null)
            {
                return item;
            }
            return null;
        }

        public async Task<ICollection<Item>> GetItems(CancellationToken ct = default)
        {
            var items = await _context.Items.AsNoTracking().ToListAsync(ct);
            return items;
        }
    }
}
