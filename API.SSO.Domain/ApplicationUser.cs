using Microsoft.AspNetCore.Identity;

namespace API.SSO.Domain
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private ApplicationUser() { }

        public ApplicationUser(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            PhoneNumber = phoneNumber;
        }

        public void UpdateUserInfo(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }
}
