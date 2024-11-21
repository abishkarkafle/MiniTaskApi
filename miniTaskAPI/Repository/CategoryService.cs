using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using miniTaskAPI.Data;
using miniTaskAPI.Interface;
using miniTaskAPI.Models;

namespace miniTaskAPI.Repository
{
    public class CategoryService : ICategoryService
    {
        private readonly AuthDbContext _context;

        public CategoryService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Tasks)
                .Include(c => c.CreatedBy)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Tasks)
                .Include(c => c.CreatedBy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.CreatedById == userId)
                .Include(c => c.Tasks)
                .ToListAsync();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
