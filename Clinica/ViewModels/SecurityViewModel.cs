using System.Collections.Generic;

namespace Clinica.ViewModels
{
    public class SecurityViewModel
    {
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
