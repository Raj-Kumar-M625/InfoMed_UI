using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface ILastYearMemoriesService
    {
        public Task<LastYearMemoryDto> AddLastYearMemories(LastYearMemoryDto lastYeatMemoryDto);
        public Task<List<LastYearMemoryDto>> GetLastYearMemoriesList(int id, int idVersion);
        public Task<LastYearMemoryDto> GetLastYearMemoriesById(int idLastYearMemory);
        public Task<LastYearMemoryDto> UpdateLastYearMemories(LastYearMemoryDto lastYeatMemoryDto);
    }
}
