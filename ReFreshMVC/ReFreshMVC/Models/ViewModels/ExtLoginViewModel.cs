using System.ComponentModel.DataAnnotations;

namespace ReFreshMVC.Models.ViewModels
{
    public class ExtLoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
