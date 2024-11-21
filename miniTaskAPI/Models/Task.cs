namespace miniTaskAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Completed"
        public string Priority { get; set; } // 1 = High, 2 = Medium, 3 = Low
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        // Foreign key for category
        public int? CategoryId { get; set; }

        // Navigation properties
        public ApplicationUser CreatedBy { get; set; }
        public Category Category { get; set; }
    }
}
