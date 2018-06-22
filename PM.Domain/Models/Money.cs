using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PM.Domain.Models
{
    public class Money : IEquatable<Money>
    {
        public decimal Amount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyResponse Currency { get; set; }

        public bool Equals(Money other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Amount == other.Amount && this.Currency == other.Currency;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var moneyObj = obj as Money;
            if (moneyObj == null)
            {
                return false;
            }
            return Equals(moneyObj);
        }

        public override int GetHashCode()
        {
            var hashString = $"{this.Amount}{this.Currency}";
            return hashString.GetHashCode();
        }

        public static bool operator == (Money money1, Money money2)
        {
            if (Object.ReferenceEquals(money1, null))
            {
                return Object.ReferenceEquals(money2, null);
            }

            return money1.Equals(money2);
        }

        public static bool operator != (Money money1, Money money2)
        {
            return !(money1 == money2);
        }
    }
}
