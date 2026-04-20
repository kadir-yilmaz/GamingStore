using System.ComponentModel.DataAnnotations;

namespace GamingStore.WebUI.ViewModels
{
    public class LoginModel
    {
        private string? _returnurl;

        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl
        {
            get => _returnurl ?? "/";
            set => _returnurl = value;
        }
    }
}
