
using Microsoft.AspNetCore.Identity;

namespace PM.DATA.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<TaskItem> AssignedTasks { get; set; }
    }


}
