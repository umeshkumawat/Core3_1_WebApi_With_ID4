using System.ComponentModel.DataAnnotations;

namespace IdentityServer_4.Controllers
{
    public class RegisterViewModel
    {
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}