using Microsoft.AspNetCore.Identity;
using miniTaskAPI.Models;

namespace miniTaskAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties if needed
        public ICollection<Task> CreatedTasks { get; set; }
    }

}