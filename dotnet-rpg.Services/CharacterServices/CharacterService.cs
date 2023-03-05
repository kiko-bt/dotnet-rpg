using AutoMapper;
using dotnet_rpg.DataModels.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Persistence;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterServices
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetCharacters(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var characters = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();

            serviceResponse.Data = characters.Select(_mapper.Map<GetCharacterDTO>).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var characters = await _context.Characters.ToListAsync();

            var character = characters.FirstOrDefault(x => x.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var character = _mapper.Map<Character>(newCharacter);

            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters
                .Select(c => _mapper.Map<GetCharacterDTO>(c))
                .ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();

            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id)
                    ?? throw new Exception($"Character with Id {updatedCharacter.Id} not found.");

                _mapper.Map(updatedCharacter, character);

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = (RpgClass)updatedCharacter.Class;


                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id)
                    ?? throw new Exception($"Character with Id {id} not found.");

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();  

                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
