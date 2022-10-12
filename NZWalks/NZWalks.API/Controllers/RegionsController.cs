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
    }
}
