using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using August2008.Model;
using August2008.Models;
using AutoMapper;

namespace August2008
{
    public class AutoMapperConfig
    {
        public static void ConfigMapper()
        {
            Mapper.CreateMap<HeroModel, Hero>();
            Mapper.CreateMap<Hero, HeroModel>();
        }
    }
}