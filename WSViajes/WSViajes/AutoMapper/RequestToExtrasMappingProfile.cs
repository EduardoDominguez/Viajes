using System;
using AutoMapper;
using Viajes.EL.Extras;
using Viajes.DAL.Modelo;
using WSViajes.Models.Request;

namespace WSViajes.AutoMapper
{
    public class RequestToExtrasMappingProfile : Profile
    {
        public RequestToExtrasMappingProfile()
        {
            /*CreateMap<Otorgante, OtorganteListItemViewModel>()
                .ForMember(dest => dest.NombreEstatusRegistro, opt => opt.MapFrom(src => src.EstatusRegistro.Nombre))
            ;*/

            CreateMap<InsertaPedidoRequest, E_PEDIDO>()
            ;
        }
    }
}
