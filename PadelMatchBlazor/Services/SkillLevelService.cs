using PadelMatchBlazor.Models;
using System.Net.Http.Json;

namespace PadelMatchBlazor.Services
{
    public class SkillLevelService
    {
        private readonly HttpClient _httpClient;

        public SkillLevelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SkillLevel>> GetAllAsync()
        {
            try
            {
                var skillLevels = await _httpClient.GetFromJsonAsync<List<SkillLevel>>("api/skilllevels");
                return skillLevels ?? new List<SkillLevel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des niveaux de compétence: {ex.Message}");
                return new List<SkillLevel>();
            }
        }
    }
}