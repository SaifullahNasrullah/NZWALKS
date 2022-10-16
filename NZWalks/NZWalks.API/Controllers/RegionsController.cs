using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDb = await _regionRepository.GetAllAsync();
            //Instead Of Domail region we should return DTO region
            //Inorder to hide the domail model for exposing outside.


            //Now this section will be done with automapper.
            /*var regionDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(domainReg =>
            {
                var region = new Models.DTO.Region()
                {
                    Id = domainReg.Id,
                    Code = domainReg.Code,
                    Name = domainReg.Name,
                    Area = domainReg.Area,
                    Long = domainReg.Long,
                    Lat = domainReg.Lat,
                    Population = domainReg.Population
                };
                regionDTO.Add(region);
            });*/
            var regionsWithAutoMapper = _mapper.Map<List<Models.DTO.Region>>(regionsDb);
            return Ok(regionsWithAutoMapper);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var regionDb = await _regionRepository.GetRegionByIdAsync(id);
            if (regionDb == null)
                return NotFound();

            var regionWithAutoMapper = _mapper.Map<List<Models.DTO.Region>>(regionDb);
            return Ok(regionWithAutoMapper);
        }

        [HttpPost] 
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Convert Request(DTO) to domain model
            //Pass details to repository
            //Convert back to DTO

            //Manual Validation request

            /*if(!ValidateAddRegionAsync(addRegionRequest))
                return BadRequest(ModelState);*/

            //Now we are using FluentValidator don't need to come even to method
            //if the requirements are not valid 

            var domainReg = new Models.Domain.Region
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,

            };
            
            var regionDb = await _regionRepository.AddRegionAsync(domainReg);
            var regionDto = new Models.DTO.Region
            {
                Id = regionDb.Id,
                Code = regionDb.Code,
                Area = regionDb.Area,
                Name = regionDb.Name,
                Lat = regionDb.Lat,
                Long = regionDb.Long,
                Population = regionDb.Population,
            };
            return CreatedAtAction(nameof(GetRegionAsync), new {id= regionDto.Id}, regionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        { 
            var deleteRegionResponse = await _regionRepository.DeleteRegionAsync(id);
            //Check if null then return notfound
            if (deleteRegionResponse == null)
                return NotFound();
            //Convert back to DTO
            var deleteResToDTO = new Models.DTO.Region
            {
                Id = deleteRegionResponse.Id,
                Code = deleteRegionResponse.Code,
                Name = deleteRegionResponse.Name,
                Area = deleteRegionResponse.Area,
                Lat = deleteRegionResponse.Lat,
                Long = deleteRegionResponse.Long,
                Population = deleteRegionResponse.Population,

            }; 
            //return Ok response
            return Ok(deleteRegionResponse);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, 
                                    [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Manual Validate Request
            /*if (!ValidateUpdateRegionAsync(updateRegionRequest))
                return BadRequest(ModelState);*/

            //Now we are using FluentValidator don't need to come even to method
            //if the requirements are not valid 

            var domainRequest = new Region
            { 
                Name = updateRegionRequest.Name,
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,
            }; 
            var updateRS = await _regionRepository.UpdateRegionAsync(id, domainRequest);

            if (updateRS == null)
                return NotFound();
            var updateDTO = new Models.DTO.Region
            {
                Id = updateRS.Id, 
                Name = updateRS.Name,
                Code = updateRS.Code,
                Area = updateRS.Area,
                Lat = updateRS.Lat,
                Long = updateRS.Long,
                Population = updateRS.Population,
            };
            return Ok(updateDTO);
        }
        #region Private Methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                    $"Add data is required !");
                return false;
            }
            if (string.IsNullOrEmpty(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{addRegionRequest.Code} can not be null or empty!");
            }
            if (string.IsNullOrEmpty(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{addRegionRequest.Name} can not be null or empty!");
            }
            if(addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{addRegionRequest.Area} can not be smaller or equal to 0 !");
            }

            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{addRegionRequest.Lat} can not be smaller or equal to 0 !");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{addRegionRequest.Long} can not be smaller or equal to 0 !");
            }
            if (addRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{addRegionRequest.Population} can not be smaller or equal to 0 !");
            }
            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if(updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"Update data can not be null or empty !");
                return false;
            }

            if (string.IsNullOrEmpty(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{updateRegionRequest.Code} can not be null or empty!");
            }
            if (string.IsNullOrEmpty(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{updateRegionRequest.Name} can not be null or empty!");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{updateRegionRequest.Area} can not be smaller or equal to 0 !");
            }

            if (updateRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat),
                    $"{updateRegionRequest.Lat} can not be smaller or equal to 0 !");
            }
            if (updateRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long),
                    $"{updateRegionRequest.Long} can not be smaller or equal to 0 !");
            }
            if (updateRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{updateRegionRequest.Population} can not be smaller or equal to 0 !");
            }
            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }
        #endregion
    }
}
