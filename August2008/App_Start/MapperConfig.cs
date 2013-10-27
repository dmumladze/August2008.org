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
            Mapper.CreateMap<HeroSearchCriteria, HeroSearchModel>();

            Mapper.CreateMap<UserModel, User>();
            Mapper.CreateMap<User, UserModel>();

            Mapper.CreateMap<Role, RoleModel>();

            Mapper.CreateMap<string, decimal>().ConvertUsing<StringDecimalConverter>();
            Mapper.CreateMap<string, int>().ConvertUsing<StringIntegerConverter>();
            Mapper.CreateMap<string, DateTime>().ConvertUsing<PayPalDateTimeConverter>();

            Mapper.CreateMap<PayPalConfirm, DonationModel>()
                .ForMember(x => x.Amount, o => o.MapFrom(y => y.amt))
                .ForMember(x => x.Currency, o => o.MapFrom(y => y.cc))
                .ForMember(x => x.ExternaStatus, o => o.MapFrom(y => y.st));

            Mapper.CreateMap<PayPalVariables, Donation>()
                .ForMember(x => x.UserId, o => o.ResolveUsing<PayPalUserIdResolver>())
                .ForMember(x => x.Amount, o => o.MapFrom(y => y.mc_gross))
                .ForMember(x => x.Currency, o => o.MapFrom(y => y.mc_currency))
                .ForMember(x => x.DateDonated, o => o.MapFrom(y => y.payment_date))
                .ForMember(x => x.ExternalId, o => o.MapFrom(y => y.txn_id))
                .ForMember(x => x.ExternalStatus, o => o.MapFrom(y => y.payment_status))
                .ForMember(x => x.TransactionType, o => o.MapFrom(y => y.txn_type))
                .ForMember(x => x.ProviderXml, o => o.MapFrom(y => y.ToXml()))
                .ForMember(x => x.DonationProviderId, o => o.UseValue<int>(1));

            Mapper.CreateMap<Donation, DonationModel>();
            Mapper.CreateMap<DonationModel, Donation>();
            Mapper.CreateMap<DonationSearchCriteria, DonationSearchModel>();
            Mapper.CreateMap<DonationSearchModel, DonationSearchCriteria>();

            Mapper.CreateMap<PayPalVariables, DonationSubscription>()
                .ForMember(x => x.StartDate, o => o.MapFrom(y => y.subscr_date))
                .ForMember(x => x.SubscriptionId, o => o.MapFrom(y => y.subscr_id))
                .ForMember(x => x.Username, o => o.MapFrom(y => y.username))
                .ForMember(x => x.Password, o => o.MapFrom(y => y.password))
                .ForMember(x => x.RecurrenceTimes, o => o.MapFrom(y => y.recur_times))
                .ForMember(x => x.UserId, o => o.ResolveUsing<PayPalUserIdResolver>())
                .ForMember(x => x.ProviderXml, o => o.MapFrom(y => y.ToXml()));
        }
    }
}