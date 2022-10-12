using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync()
        {
            var walksDb = await _walkRepository.GetAllWalkAsync();
            
            var walksWithAutoMapper = _mapper.Map<List<Models.DTO.Walk>>(walksDb);
            return Ok(walksWithAutoMapper);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkByIdAsync")]
        public async Task<IActionResult> GetWalkByIdAsync(Guid id)
        {
            var walkDb = await _walkRepository.GetWalkByIdAsync(id);

            //Convert it to DTO manually
            //var walkDot = new Models.DTO.Walk
            //{
            //    Id = walkDb.Id,
            //    Name = walkDb.Name,
            //    Length = walkDb.Length,
            //    RegionId = walkDb.RegionId,
            //    WalkDifficultyId = walkDb.WalkDifficultyId,
            //    //Region = walkDb.Region,
            //    //WalkDifficulty = walkDb.WalkDifficulty,
            //};
            //Or convert with Authomapper !!!
            var walkDTOAutomapper = _mapper.Map<Models.DTO.Walk>(walkDb);
            return Ok(walkDTOAutomapper);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            var walkDomain = new Models.Domain.Walk
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            var addedWalk = await _walkRepository.AddWalkAsync(walkDomain);
            var dtoWalk = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            return CreatedAtAction(nameof(GetWalkByIdAsync), new { id = dtoWalk.Id }, dtoWalk);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
                                    [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            var domainRequest = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            var updateRS = await _walkRepository.UpdateWalkAsync(domainRequest,id); 
            if (updateRS == null)
                return NotFound();

            var updateDTO = new Models.DTO.Walk
            {
                Id = updateRS.Id,
                Name = updateRS.Name,
                Length = updateRS.Length,
                RegionId = updateRS.RegionId,
                WalkDifficultyId = updateRS.WalkDifficultyId
            };
            return Ok(updateDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var deleteWalkResponse = await _walkRepository.DeleteWalkAsync(id);
            //Check if null then return notfound
            if (deleteWalkResponse == null)
                return NotFound();
            //Convert back to DTO
            var deleteResToDTO = new Models.DTO.Walk
            {
                Id = deleteWalkResponse.Id,
                Name = deleteWalkResponse.Name,
                Length = deleteWalkResponse.Length,
                RegionId = deleteWalkResponse.RegionId,
                WalkDifficultyId = deleteWalkResponse.WalkDifficultyId

            };
            //By Automapper....
            //var deleteAutmap = _mapper.Map<Models.DTO.Walk>(deleteWalkResponse);
            //return Ok response
            return Ok(deleteWalkResponse);
        }
    }
}
