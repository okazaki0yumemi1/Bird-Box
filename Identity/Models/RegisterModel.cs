using System.ComponentModel.DataAnnotations;

namespace Bird_Box.Authentication
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class RegisterModel
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        [Required(ErrorMessage = "Username is required")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Username { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Email { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        [Required(ErrorMessage = "Password is required")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Password { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}