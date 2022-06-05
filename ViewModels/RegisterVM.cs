using System.ComponentModel.DataAnnotations;

namespace fiorello.ViewModels
{
    public class RegisterVM
    {
        [Required, StringLength(200)]
        public string FullName { get; set; }

        [Required, StringLength(200)]
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password),Compare(nameof(Password))]
        public string CheckPassword { get; set; }

    }
}
