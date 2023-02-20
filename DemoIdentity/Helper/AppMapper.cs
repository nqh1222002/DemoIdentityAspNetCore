using AutoMapper;
using DemoIdentity.Models;
namespace DemoIdentity.Helper
{
    public class AppMapper : Profile
    {
        public AppMapper() {
            CreateMap<Person, PersonDto>();
        }
    }
}
