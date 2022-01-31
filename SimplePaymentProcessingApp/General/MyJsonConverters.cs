using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimplePaymentProcessingApp.General
{
    public static class MyJsonConverters
    {
        /// <summary>
        /// Custom JSON converter that deserializes decimal values normally, but ensures they are rounded to the nearest hundreth when serializing them.
        /// </summary>
        public class MoneyConverter : JsonConverter<decimal>
        {
            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.GetDecimal();
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(Math.Round(value, 2));
            }
        }

        /// <summary>
        /// Custom JSON converter that deserializes and serializes DateTime structs in a specific format. That format is: MM/YYYY.
        /// </summary>
        public class ExpirationDateConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? expirationDateString = reader.GetString();

                if (DateTime.TryParseExact(expirationDateString, "MM\\/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                else
                {
                    return DateTime.UnixEpoch;
                }
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("MM\\/yyyy"));
            }
        }
    }
}
