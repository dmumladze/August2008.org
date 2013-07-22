using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using August2008.Model;
using August2008.Models;
using AutoMapper;
using AutoMapper.Mappers;
using August2008.Helpers;

namespace August2008
{
    public class MapperConfig
    {
        public static void RegisterMapper()
        {
            Mapper.CreateMap<HeroModel, Hero>();
            Mapper.CreateMap<Hero, HeroModel>();

            Mapper.CreateMap<UserModel, User>();
            Mapper.CreateMap<User, UserModel>();

            Mapper.CreateMap<Role, RoleModel>();

            Mapper.CreateMap<string, decimal>().ConvertUsing<StringDecimalConverter>();

            Mapper.CreateMap<PayPalModel, Donation>()
                .ForMember(x => x.Amount, o => o.MapFrom(y => y.mc_gross))
                .ForMember(x => x.Currency, o => o.MapFrom(y => y.mc_currency))
                .ForMember(x => x.FirstName, o => o.MapFrom(y => y.first_name))
                .ForMember(x => x.LastName, o => o.MapFrom(y => y.last_name))
                .ForMember(x => x.Email, o => o.MapFrom(y => y.payer_email));

            Mapper.CreateMap<Donation, DonationModel>();
            Mapper.CreateMap<DonationModel, Donation>();
            
        }
    }
}