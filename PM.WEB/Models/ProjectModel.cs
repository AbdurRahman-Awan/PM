using PM.DATA.Enums;
using PM.WEB.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PM.WEB.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [DateGreaterThan("StartDate", ErrorMessage = "End Date must be greater than Start Date.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public ProjectStatus Status { get; set; }
    }

}
