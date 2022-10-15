using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkDifficultyRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllWalkDifficultyAsync();
        Task<WalkDifficulty> GetWalkDifficultyByIdAsync(Guid Id);
        Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty walk);
        Task<WalkDifficulty> UpdateWalkDifficultyAsync(WalkDifficulty walk, Guid Id);
        Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid Id);
    }
}
