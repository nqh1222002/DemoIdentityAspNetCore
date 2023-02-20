using AutoMapper;
using DemoIdentity.Data;
using DemoIdentity.Helper;
using DemoIdentity.Models;
using DemoIdentity.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;
        public PeopleController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<PersonDto>> Get() {
            var people = await _context.People.ToListAsync();
            return _mapper.Map<List<Person>, List<PersonDto>>(people);
        }

        
        
           
    }
}
