using System;
using System.Text.Json;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend
{
    public static class ValueUtilities
    {
        public static void PreprocessQuery(QueryRequest queryRequest)
        {
            if (queryRequest.Filters != null)
            {
                foreach (var queryRequestFilter in queryRequest.Filters)
                {
                    ConvertValues(queryRequestFilter);
                }
            }
        }
        
        
        public static void ConvertValues(QueryInstance instance)
        {
            var element = (JsonElement) instance.Value;
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    instance.Value = element.GetString();
                    break;
                case JsonValueKind.Number:
                    instance.Value = element.GetDouble();
                    break;
                case JsonValueKind.False:
                case JsonValueKind.True:
                    instance.Value = element.GetBoolean();
                    break;
                case JsonValueKind.Null:
                    instance.Value = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}