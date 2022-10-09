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
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _regionRepository.GetAllAsync();
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
            var regionsWithAutoMapper = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsWithAutoMapper);
        }
    }
}
