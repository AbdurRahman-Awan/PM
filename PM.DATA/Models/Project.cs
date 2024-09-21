
using PM.DATA.Enums;

namespace PM.DATA.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; } // Active, Deferred, Completed

        // Navigation property
        public ICollection<TaskItem> Tasks { get; set; }=new List<TaskItem>();
    }

}
