using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        } 

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionByIdAsync(Guid Id)
        { 
            return await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region> AddRegionAsync(Region region)
        { 
            region.Id = region.Id;
            await _nZWalksDbContext.AddAsync(region);
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteRegionAsync(Guid id)
        {
            var region = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
                return null;
            //Delete region
            _nZWalksDbContext.Regions.Remove(region);
            await _nZWalksDbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> UpdateRegionAsync(Guid Id, Region region)
        {
            //existing
            var regionDB = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            if (regionDB == null)
                return null;

            regionDB.Code = region.Code;
            regionDB.Name = region.Name;
            regionDB.Area = region.Area;
            regionDB.Lat = region.Lat;
            regionDB.Long = region.Long;
            regionDB.Population = region.Population;
             
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }
    }
}
