namespace miniTaskAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Work", "Personal"
        public string CreatedById { get; set; }
        public string Description { get; set; }

        // Navigation property
        public ICollection<Task> Tasks { get; set; }
        public ApplicationUser CreatedBy { get; set; }

    }
}
