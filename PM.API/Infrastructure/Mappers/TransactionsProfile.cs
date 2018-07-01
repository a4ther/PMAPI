using System;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;
using PM.API.Models.Request;

namespace PM.API.Infrastructure.Mappers
{
	public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            CreateMap<PostTransaction, TransactionDTO>()
                .ForMember(dest => dest.Category,
                           opts => opts.MapFrom(
                               src => new CategoryDTO
                               {
                                   ID = src.CategoryID
                               }
                          ))
                .ForMember(dest => dest.Money,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   Domain.Models.Currency currency = Domain.Models.Currency.CRC;
                                   if (Enum.TryParse<Domain.Models.Currency>(src.Currency.ToString(), out var parsedCurrency))
                                   {
                                       currency = parsedCurrency;
                                   }
                                   return new MoneyDTO
                                   {
                                       Amount = src.Amount,
                                       Currency = currency
                                   };
                               }
                          ));

            CreateMap<PutTransaction, TransactionDTO>()
                .ForMember(dest => dest.Category,
                           opts => opts.MapFrom(
                               src => new CategoryDTO
                               {
                                   ID = src.CategoryID
                               }
                          ))
                .ForMember(dest => dest.Money,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   Domain.Models.Currency currency = Domain.Models.Currency.CRC;
                                   if (Enum.TryParse<Domain.Models.Currency>(src.Currency.ToString(), out var parsedCurrency))
                                   {
                                       currency = parsedCurrency;
                                   }
                                   return new MoneyDTO
                                   {
                                       Amount = src.Amount,
                                       Currency = currency
                                   };
                               }
                          ));

            CreateMap<TransactionDTO, Transaction>()
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

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.Category,
                           opts => opts.MapFrom(
                               src => new CategoryDTO
                               {
                                   ID = src.CategoryID
                               }
                          ))
                .ForMember(dest => dest.Money,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   Domain.Models.Currency currency = Domain.Models.Currency.CRC;
                                   if (Enum.TryParse<Domain.Models.Currency>(src.Currency.ToString(), out var parsedCurrency))
                                   {
                                       currency = parsedCurrency;
                                   }
                                   return new MoneyDTO
                                   {
                                       Amount = src.Amount,
                                       Currency = currency
                                   };
                               }
                          ));
        }
    }
}
