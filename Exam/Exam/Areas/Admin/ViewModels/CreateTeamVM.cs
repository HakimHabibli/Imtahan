using System.ComponentModel.DataAnnotations;

namespace Exam.Areas.Admin.ViewModels
{
    public class CreateTeamVM
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public string Surname { get; set; }
        [Required]


        public string Position { get; set; }

        [Required]
        public string FacebookLink { get; set; }

        [Required]
        public string InstagramLink { get; set; }

        [Required]
        public string TwitterLink { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
