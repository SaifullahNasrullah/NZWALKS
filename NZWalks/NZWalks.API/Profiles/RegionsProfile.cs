using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile: Profile
    {
        public RegionsProfile()
        {
            //This means that we mapp our Domain model to DTO model
            //this is possibe with authmapper
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap(); 
        }
    }
}
