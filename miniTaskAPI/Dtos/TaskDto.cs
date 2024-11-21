namespace miniTaskAPI.DTOs
{
    public class TaskCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // Optional
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class TaskUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
    }

}
