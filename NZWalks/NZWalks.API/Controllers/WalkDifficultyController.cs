using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            var walkDifficulties = await _walkDifficultyRepository.GetAllWalkDifficultyAsync();
            var autoMapResultWD = _mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);
            return Ok(autoMapResultWD);
        }
         
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficultyByIdAsync(id);
            var autoMapResultWD = _mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(autoMapResultWD);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficutyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };
            var addWalkDiffRes = await _walkDifficultyRepository.AddWalkDifficultyAsync(walkDifficulty);
            var autoMapResultWD = _mapper.Map<Models.DTO.WalkDifficulty>(addWalkDiffRes);
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = autoMapResultWD.Id }, autoMapResultWD);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest,
            [FromRoute] Guid id)
        {
            var walkDiffDb = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code,
            };
            var updateWalkDf = await _walkDifficultyRepository.UpdateWalkDifficultyAsync(walkDiffDb, id);
            if (updateWalkDf == null)
                return NotFound();
            var autoMapResultWD = _mapper.Map<Models.DTO.WalkDifficulty>(updateWalkDf);
            return Ok(autoMapResultWD);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var deleteWD = await _walkDifficultyRepository.DeleteWalkDifficultyAsync(id);
            if (deleteWD == null)
                return NotFound();
            var autoMapResultWD = _mapper.Map<Models.DTO.WalkDifficulty>(deleteWD);
            return Ok(autoMapResultWD);
        }
    }
}
