using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<IEnumerable<WalkDifficulty>> GetAllWalkDifficultyAsync()
        {
            return await _nZWalksDbContext.WalkDifficulty.ToListAsync();
        }
        public async Task<WalkDifficulty> GetWalkDifficultyByIdAsync(Guid Id)
        {
           return await _nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await _nZWalksDbContext.AddAsync(walkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid Id)
        {
            var walkDifficulty = await _nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == Id);
            if (walkDifficulty == null)
                return null;
            //Delete region
            _nZWalksDbContext.WalkDifficulty.Remove(walkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty;
        } 

        public async Task<WalkDifficulty> UpdateWalkDifficultyAsync(WalkDifficulty walkDifficulty, Guid Id)
        {
            var walkDifficultyDB = await _nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == Id);
            if (walkDifficultyDB == null)
                return null; 
            walkDifficultyDB.Code = walkDifficulty.Code; 
            await _nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }
    }
}
