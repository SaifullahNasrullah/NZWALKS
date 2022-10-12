using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalkProfile: Profile
    {
        public WalkProfile()
        {
            //This means that we mapp our Domain model to DTO model
            //this is possibe with authmapper
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>()
                .ReverseMap();

            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>()
               .ReverseMap();

            CreateMap<Models.Domain.Region, Models.DTO.Region>()
               .ReverseMap();
        }
    }
}
