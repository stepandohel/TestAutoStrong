using AutoMapper;
using Domain.Data.Models;
using WebAPI.Modeles;

namespace WebAPI.Mapping
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<ItemCM, Item>()
                .ForMember(x=>x.FilePath, opt => opt.MapFrom(x=>x.File.FileName));
            CreateMap<Item, ItemCM>()
                .ForMember(x => x.File, opt => opt.Ignore());
        }
    }
}
