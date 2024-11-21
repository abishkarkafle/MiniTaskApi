using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniTaskAPI.DTOs;
using miniTaskAPI.Interface;
using miniTaskAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace miniTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound("Category not found.");
                return Ok(category);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description
            };

            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (id != categoryUpdateDto.Id)
                return BadRequest("Category ID mismatch.");

            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
                return NotFound("Category not found.");

            existingCategory.Name = categoryUpdateDto.Name ?? existingCategory.Name;
            existingCategory.Description = categoryUpdateDto.Description ?? existingCategory.Description;

            try
            {
                var isUpdated = await _categoryService.UpdateCategoryAsync(existingCategory);
                if (!isUpdated)
                    return StatusCode(500, "Error updating category.");
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var isDeleted = await _categoryService.DeleteCategoryAsync(id);
                if (!isDeleted)
                    return NotFound("Category not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
