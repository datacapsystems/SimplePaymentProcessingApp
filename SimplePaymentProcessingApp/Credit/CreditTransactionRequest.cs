using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimplePaymentProcessingApp.General;

namespace SimplePaymentProcessingApp.Credit
{
    /// <summary>
    /// JSON-deserializable object that contains data for a credit transaction request.
    /// </summary>
    public class CreditTransactionRequest
    {
        [JsonInclude]
        [JsonConverter(typeof(MyJsonConverters.MoneyConverter))] // Redundant for deserialization, but present for consistency.
        public decimal? Amount { get; private set; }

        [JsonInclude]
        public string? CardNumber { get; private set; }

        [JsonInclude]
        [JsonPropertyName("Expiration")]
        [JsonConverter(typeof(MyJsonConverters.ExpirationDateConverter))]
        public DateTime? ExpirationDate { get; private set; }

        [JsonInclude]
        public string? CardholderName { get; private set; }

        /// <summary>
        /// Overridden equality comparator that checks if all fields match to determine whether two instances are equal.
        /// Note: this is much different than simply using the '==' operator.
        /// </summary>
        /// <param name="obj">Other object to compare this object to.</param>
        /// <returns>True if the objects have the same fields, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            CreditTransactionRequest? other = obj as CreditTransactionRequest;
            if (other != null)
            {
                return this.Amount == other.Amount
                    && this.CardNumber == other.CardNumber
                    && this.ExpirationDate == other.ExpirationDate
                    && this.CardholderName == other.CardholderName;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
