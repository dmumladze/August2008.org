using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using August2008.Model;
using August2008.Models;
using AutoMapper;

namespace August2008
{
    public class MapperConfig
    {
        public static void RegisterMapper()
        {
            Mapper.CreateMap<HeroModel, Hero>();
            Mapper.CreateMap<Hero, HeroModel>();
        }
    }
}