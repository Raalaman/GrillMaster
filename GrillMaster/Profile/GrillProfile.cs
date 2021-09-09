using ApplicationCore;
using AutoMapper;
using Infrastructure;

namespace GrillMaster
{
    public class GrillProfile : Profile
    {
        public GrillProfile()
        {
            CreateMap<GrillMenu, GrillMenuModel>()
                .ForMember(
                    dest => dest.Menu,
                    opt => opt.MapFrom(src => src.Menu));

            CreateMap<GrillMenuItem, GrillMenuItemModelWQuantity>()
                .ForMember(
                    dest => dest.Height,
                    opt => opt.MapFrom(src => src.Length))
                .ForMember(
                    dest => dest.Width,
                    opt => opt.MapFrom(src => src.Width))
                .ForMember(
                    dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity));
        }
    }
}
