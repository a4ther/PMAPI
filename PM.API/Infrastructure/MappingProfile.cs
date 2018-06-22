using System;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;
using PM.API.Models;

namespace PM.API.Infrastructure
{
	public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PostTransaction, TransactionResponse>()
                .ForMember(dest => dest.Category,
                           opts => opts.MapFrom(
                               src => new CategoryResponse
                               {
                                   ID = src.CategoryID
                               }
                          ))
                .ForMember(dest => dest.Money,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   CurrencyResponse currency = CurrencyResponse.CRC;
                                   if (Enum.TryParse<CurrencyResponse>(src.Currency.ToString(), out var parsedCurrency))
                                   {
                                       currency = parsedCurrency;
                                   }
                                   return new MoneyResponse
                                   {
                                       Amount = src.Amount,
                                       Currency = currency
                                   };
                               }
                          ));

            CreateMap<PutTransaction, TransactionResponse>()
                .ForMember(dest => dest.Category,
                           opts => opts.MapFrom(
                               src => new CategoryResponse
                               {
                                   ID = src.CategoryID
                               }
                          ))
                .ForMember(dest => dest.Money,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   CurrencyResponse currency = CurrencyResponse.CRC;
                                   if (Enum.TryParse<CurrencyResponse>(src.Currency.ToString(), out var parsedCurrency))
                                   {
                                       currency = parsedCurrency;
                                   }
                                   return new MoneyResponse
                                   {
                                       Amount = src.Amount,
                                       Currency = currency
                                   };
                               }
                          ));

            CreateMap<TransactionResponse, Transaction>()
                .ForMember(dest => dest.Amount,
                           opts => opts.MapFrom(
                               src => src.Money.Amount
                          ))
                .ForMember(dest => dest.CategoryID,
                           opts => opts.MapFrom(
                               src => src.Category.ID
                          ))
                .ForMember(dest => dest.Currency,
                           opts => opts.MapFrom(
                               src => src.Money.Currency
                          ));

            CreateMap<Transaction, TransactionResponse>()
                .ForMember(dest => dest.Category,
                           opts => opts.MapFrom(
                               src => new CategoryResponse
                               {
                                   ID = src.CategoryID
                               }
                          ))
                .ForMember(dest => dest.Money,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   CurrencyResponse currency = CurrencyResponse.CRC;
                                   if (Enum.TryParse<CurrencyResponse>(src.Currency.ToString(), out var parsedCurrency))
                                   {
                                       currency = parsedCurrency;
                                   }
                                   return new MoneyResponse
                                   {
                                       Amount = src.Amount,
                                       Currency = currency
                                   };
                               }
                          ));
        }
    }
}
