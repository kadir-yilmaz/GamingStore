using GamingStore.BLL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.WebUI.Components
{
    public class UserSummary : ViewComponent
    {
        private readonly IAuthService _authService;
        public UserSummary(IAuthService authService)
        {
            _authService = authService;
        }

        public string Invoke()
        {
            return _authService
                .GetAllUsers()
                .Count()
                .ToString();
        }
    }
}
