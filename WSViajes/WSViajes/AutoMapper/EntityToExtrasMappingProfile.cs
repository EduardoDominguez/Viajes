using System;
using AutoMapper;
using Viajes.EL.Extras;
using Viajes.DAL.Modelo;

namespace WSViajes.AutoMapper
{
    public class EntityToExtrasMappingProfile: Profile
    {
        public EntityToExtrasMappingProfile()
        {
            /*CreateMap<Otorgante, OtorganteListItemViewModel>()
                .ForMember(dest => dest.NombreEstatusRegistro, opt => opt.MapFrom(src => src.EstatusRegistro.Nombre))
            ;*/

            CreateMap<CTL_PRODUCTO, E_PRODUCTO>()
                .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.id_producto))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.descripcion))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.precio))
                .ForMember(dest => dest.Fotografia, opt => opt.MapFrom(src => src.fotografia))
                .ForMember(dest => dest.IdLocal, opt => opt.MapFrom(src => src.id_local))
                .ForMember(dest => dest.IdPersonaAlta, opt => opt.MapFrom(src => src.id_persona_alta))
                .ForMember(dest => dest.IdPersonaModifica, opt => opt.MapFrom(src => src.id_persona_mod))
            ;
        }
    }
}
