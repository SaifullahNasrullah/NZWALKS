using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            var walkDifficulties = await _walkDifficultyRepository.GetAllWalkDifficultyAsync();
            var autoMapResultWD = _mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);
            return Ok(autoMapResultWD);
        }
         
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficultyByIdAsync(id);
            var autoMapResultWD = _mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(autoMapResultWD);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficutyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Manual Validation
            /*if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
                return BadRequest(ModelState);*/

            //Manual Validation commented because now it using Fluent validator.

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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest,
            [FromRoute] Guid id)
        {
            //Manual Validation
            /*if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
                return BadRequest(ModelState);*/

            //Manual Validation commented because now it using Fluent validator.
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var deleteWD = await _walkDifficultyRepository.DeleteWalkDifficultyAsync(id);
            if (deleteWD == null)
                return NotFound();
            var autoMapResultWD = _mapper.Map<Models.DTO.WalkDifficulty>(deleteWD);
            return Ok(autoMapResultWD);
        }

        #region Private Methods 
        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest) 
        {
            if(addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                    $"Add data can not be null !");
                return false;
            }
            if (string.IsNullOrEmpty(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                    $"{addWalkDifficultyRequest.Code} is invalid !");
            }
            if (ModelState.ErrorCount > 0)
                return false;
            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                    $"Add data can not be null !");
                return false;
            }
            if (string.IsNullOrEmpty(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{updateWalkDifficultyRequest.Code} is invalid !");
            }
            if (ModelState.ErrorCount > 0)
                return false;
            return true;
        }
        #endregion
    }
}
