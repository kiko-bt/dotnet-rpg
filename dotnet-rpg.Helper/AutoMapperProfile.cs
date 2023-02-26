using AutoMapper;
using dotnet_rpg.DataModels.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
        }
    }
}