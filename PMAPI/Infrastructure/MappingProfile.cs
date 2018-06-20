﻿using System;
using AutoMapper;
using PMAPI.Data.Models;
using PMAPI.Domain.Models;
using PMAPI.Models.Transactions;

namespace PMAPI.Infrastructure
{
	public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TransactionRequest, TransactionResponse>()
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
                                   return new Money
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
                                   return new Money
                                   {
                                       Amount = src.Amount,
                                       Currency = currency
                                   };
                               }
                          ));
        }
    }
}