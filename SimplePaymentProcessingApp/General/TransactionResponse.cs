using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimplePaymentProcessingApp.General
{
    /// <summary>
    /// JSON-serializable object that contains data for a transaction response.
    /// </summary>
    public class TransactionResponse
    {
        [JsonInclude]
        [JsonPropertyName("CmdStatus")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CommandStatus CommandStatus { get; private set; }

        [JsonInclude]
        [JsonPropertyName("TextMessage")]
        public string Message { get; private set; }

        [JsonInclude]
        [JsonConverter(typeof(MyJsonConverters.MoneyConverter))]
        public decimal ProcessingFee { get; private set; }

        [JsonInclude]
        public bool RequiresSignature { get; private set; }

        public TransactionResponse(CommandStatus commandStatus, string message, decimal processingFee, bool requiresSignature)
        {
            CommandStatus = commandStatus;
            Message = message;
            ProcessingFee = processingFee;
            RequiresSignature = requiresSignature;
        }
    }
}
