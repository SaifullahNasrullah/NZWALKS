using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        } 
        public async Task<IEnumerable<Walk>> GetAllWalkAsync()
        {
            //With the help of include keyword we can bring also 
            //navigation properties
            return await _nZWalksDbContext.Walks
                        .Include(x=>x.Region)
                        .Include(x=>x.WalkDifficulty)
                        .ToListAsync();
        }

        public async Task<Walk> GetWalkByIdAsync(Guid Id)
        {
            return await _nZWalksDbContext.Walks
                        .Include(x=>x.Region)
                        .Include(x=>x.WalkDifficulty)
                        .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            walk.Id = walk.Id;
            await _nZWalksDbContext.Walks.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();
            return walk; 
        }

        public async Task<Walk> UpdateWalkAsync(Walk walk, Guid Id)
        {
            var walkDb = await _nZWalksDbContext.Walks.FindAsync(Id);
            if (walkDb == null)
                return null;
            walkDb.Name = walk.Name;
            walkDb.Length = walk.Length;
            walkDb.RegionId = walk.RegionId;
            walkDb.WalkDifficultyId = walk.WalkDifficultyId;

            await _nZWalksDbContext.SaveChangesAsync();

            return walkDb; 
        }

        public async Task<Walk> DeleteWalkAsync(Guid Id)
        {
            var walkDb = await _nZWalksDbContext.Walks.FindAsync(Id);
            if (walkDb == null)
                return null;
            _nZWalksDbContext.Walks.Remove(walkDb);
            await _nZWalksDbContext.SaveChangesAsync();
            return walkDb;
        }
    }
}
