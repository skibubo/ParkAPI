using AutoMapper;
using PakiAPI.Models;
using PakiAPI.Models.Dtos;

namespace PakiAPI.PakiMapper
{
    public class PakiMappings : Profile
    {
        public PakiMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
