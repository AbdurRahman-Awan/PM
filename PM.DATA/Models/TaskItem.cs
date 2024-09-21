using PM.DATA.Enums;

namespace PM.DATA.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Priority Priority { get; set; }  // Low, Medium, High
        public Enums.TaskStatus Status { get; set; }  // InProgress, Completed, Deferred

        // Recurring Task Properties
        public bool IsRecurring { get; set; }
        public RecurrencePattern? RecurrencePattern { get; set; } // Daily, Weekly, Monthly

        // Foreign key to Project
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        // Foreign key for task assignment
        public string AssignedToUserId { get; set; }
        public ApplicationUser AssignedToUser { get; set; }
    }


}
