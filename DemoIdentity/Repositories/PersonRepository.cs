using DemoIdentity.Data;
using DemoIdentity.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace DemoIdentity.Repositories
{
    public interface IPersonRepository
    {
        public List<Person> GetAll(string search,int page = 1);
    }
    public class PersonRepository : IPersonRepository
    {
        public int pageSize = 3;
        private readonly AppDbContext _context;
        public List<Person> GetAll(string search, int page = 1)
        {
            var allPeople = _context.People.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                allPeople = allPeople.Where(p => p.FirstName == search);
                allPeople = allPeople.Where(p => p.LastName == search);
            }

            allPeople = allPeople.Skip((page - 1) * pageSize).Take(pageSize);
            return allPeople.Select(p => new Person
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthDate = p.BirthDate,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
            }).ToList();

        }
    }

   
}
