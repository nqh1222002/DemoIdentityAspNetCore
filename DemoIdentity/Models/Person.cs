using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace DemoIdentity.Models
{
    public class Person
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber{ get; set; }

        public string Email { get; set; }
        public string Address { get; set; }

    }
}
