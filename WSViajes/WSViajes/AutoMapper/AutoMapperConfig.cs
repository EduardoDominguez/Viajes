using System;
using AutoMapper;

namespace WSViajes.AutoMapper
{
    public class AutoMapperConfig
    {
        public AutoMapperConfig()
        {
        }

        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToExtrasMappingProfile());
                cfg.AddProfile(new RequestToExtrasMappingProfile());
            });
        }
    }
}
