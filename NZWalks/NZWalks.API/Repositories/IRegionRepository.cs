using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        public Task<IEnumerable<Region>> GetAllAsync();
    }
}
