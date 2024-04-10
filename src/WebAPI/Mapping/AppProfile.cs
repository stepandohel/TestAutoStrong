using AutoMapper;
using Domain.Data.Models;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<ItemRequestModel, Item>()
                .ForMember(x=>x.FilePath, opt => opt.MapFrom(x=>x.File.FileName));
            CreateMap<Item, ItemRequestModel>()
                .ForMember(x => x.File, opt => opt.Ignore());
        }
    }
}
