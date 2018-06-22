using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PM.Data.Models;

namespace PM.API.Models
{
    public class PostTransaction
    {
        [BindRequired]
        public int CategoryID { get; set; }

        [BindRequired]
        public DateTime Date { get; set; }

        [BindRequired]
        public decimal Amount { get; set; }

        [BindRequired]
        [EnumDataType(typeof(Currency), ErrorMessage = "Currency value is not valid")]
        public Currency Currency { get; set; }

        [BindRequired]
        [MaxLength(10, ErrorMessage = "Wallet must be 10 characters or less")]
        public string Wallet { get; set; }
    }
}
