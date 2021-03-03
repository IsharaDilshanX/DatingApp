using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            //"ForMember" means which property that we want to affect 
            //And the first parameter we pass is the destination. 
            //What property are we looking to affect? in this case, it's PhotoUrl 
            //The next part of this is the options. 
            //We can tell it where we want it to map from very specifically, 
            //where do we want this to map from? 
            //And we want to get this from our source and 
            //we're going to say the source is the photos and then 
            //we're going to say first or defaults inside here and 
            //we'll need to bring in system link because this is a link expression. 
            //And then inside the first or default, 
            //we're going to say x goes to x.ismain and then 
            //we're going to get the Url from this property. 

            //When we map an individual property, 
            //we give it the destination property, the photo URL, 
            //we tell it where we want to map from and 
            //the source of where we're mapping from. And 
            //we're going to go into our users photos collection and 
            //get the first photo or defaults that is main and 
            //get the URL from that. 
            CreateMap<Photo, PhotoDto>();
        }
    }
}