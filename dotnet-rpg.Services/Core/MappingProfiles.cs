using AutoMapper;
using dotnet_rpg.DataModels.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO, Character>();
            CreateMap<UpdateCharacterDTO, Character>();
        }
    }
}
