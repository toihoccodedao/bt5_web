using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các category
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                                 .Include(c => c.Products) // Nếu bạn muốn lấy kèm danh sách sản phẩm
                                 .ToListAsync();
        }

        // Lấy category theo ID
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
                                 .Include(c => c.Products) // Nếu muốn lấy kèm sản phẩm
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Thêm mới category
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        // Cập nhật category
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        // Xóa category
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}