using PM.DATA.Enums;
using System.ComponentModel.DataAnnotations;

namespace PM.DATA.Models.Dto
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public ProjectStatus Status { get; set; }
    }

}
