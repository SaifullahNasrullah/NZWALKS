using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region> GetRegionByIdAsync(Guid Id);
        Task<Region> AddRegionAsync(Region region);

        Task<Region> DeleteRegionAsync(Guid Id);
        Task<Region> UpdateRegionAsync(Guid Id, Region region);

    }
}
