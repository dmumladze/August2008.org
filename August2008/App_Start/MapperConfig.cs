using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using August2008.Model;
using August2008.Models;
using AutoMapper;
using AutoMapper.Mappers;

namespace August2008
{
    public class MapperConfig
    {
        public static void RegisterMapper()
        {
            Mapper.CreateMap<HeroModel, Hero>();
            Mapper.CreateMap<Hero, HeroModel>();

            Mapper.CreateMap<List<Role>, List<int>>().ForMember(
                x => x,
                o => o.MapFrom(y => y.Select(z => z.RoleId)));

            Mapper.CreateMap<List<int>, List<Role>>().ForMember(
                x => x.Select(p => p.RoleId),
                o => o.MapFrom(y => y));

            Mapper.CreateMap<UserModel, User>();
        }
    }
}