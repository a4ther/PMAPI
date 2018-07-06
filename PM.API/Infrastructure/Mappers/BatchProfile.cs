using System;
using AutoMapper;
using PM.API.Models.Request;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.API.Infrastructure.Mappers
{
	public class BatchProfile : Profile
    {
        public BatchProfile()
        {
            CreateMap<PostBatch, BatchDTO>()
                .ForMember(dest => dest.Transactions,
                           opts => opts.MapFrom(
                               src => src.Transactions
                          ));

            CreateMap<PostBatchTransaction, TransactionDTO>()
                .ForMember(dest => dest.Category,
                           opts => opts.MapFrom(
                               src => new CategoryDTO
                               {
                                   Name = src.CategoryName
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

            CreateMap<BatchDTO, Batch>()
                .ForMember(dest => dest.Transactions,
                           opts => opts.Ignore());

            CreateMap<Batch, BatchDTO>()
                .ForMember(dest => dest.Transactions,
                           opts => opts.Ignore())
                .ForMember(dest => dest.Date,
                           opts => opts.MapFrom(
                               src => src.DateAdded
                          ));
        }
    }
}
