using System.ComponentModel.DataAnnotations;

namespace Maxim_Project.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(8)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(10)]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(DataType.Password))]

        public string RepeatPassword { get; set; }

    }
}
