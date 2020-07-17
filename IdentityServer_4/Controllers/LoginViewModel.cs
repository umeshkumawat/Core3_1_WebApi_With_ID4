namespace IdentityServer_4.Controllers
{
    public class LoginViewModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}