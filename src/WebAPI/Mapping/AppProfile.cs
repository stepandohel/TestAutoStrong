using AutoMapper;
using Domain.Data.Models;
using Server.Shared.Models;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<ItemRequestModel, Item>()
                .ForMember(x=>x.FilePath, opt => opt.Ignore());
            CreateMap<Item, ItemResponseModel>()
                .ForMember(x => x.FileContent, opt => opt.Ignore());
        }
    }
}
