using System;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;
using PM.API.Models.Request;

namespace PM.API.Infrastructure.Mappers
{
	public class TransactionProfile : Profile
    {
        void HandleAction(IMemberConfigurationExpression<TransactionDTO, Transaction, Category> obj)
        {
        }


        public TransactionProfile()
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
                                   Currency currency = Currency.CRC;
                                   if (Enum.TryParse<Currency>(src.Currency.ToString(), out var parsedCurrency))
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
                                   Currency currency = Currency.CRC;
                                   if (Enum.TryParse<Currency>(src.Currency.ToString(), out var parsedCurrency))
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
                          ))
                .ForMember(dest => dest.Category,
                           opts => opts.Ignore()
                          );

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.Category,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   var category = new CategoryDTO
                                   {
                                       ID = src.CategoryID,
                                   };
                                   if(src.Category != null)
                                   {
                                       category.Name = src.Category.Name;
                                   }
                                   return category;
                               }
                          ))
                .ForMember(dest => dest.Money,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   Currency currency = Currency.CRC;
                                   if (Enum.TryParse<Currency>(src.Currency.ToString(), out var parsedCurrency))
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
