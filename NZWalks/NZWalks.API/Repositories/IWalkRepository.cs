using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAllWalkAsync();
        Task<Walk> GetWalkByIdAsync(Guid Id); 
        Task<Walk> AddWalkAsync(Walk walk);
        Task<Walk> UpdateWalkAsync(Walk walk, Guid Id);
        Task<Walk> DeleteWalkAsync(Guid Id);
    }
}
