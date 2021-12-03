using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AOC.Util
{
    public static class InputHelper
    {
        private static HttpClient _httpClient = new HttpClient();
        private static IConfiguration _config;
        static InputHelper()
        {
            InitializeUserSecrets();
            InitializeHttpClient();
        }
        private static void InitializeUserSecrets()
        {
            _config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddUserSecrets(typeof(InputHelper).Assembly)
            .Build();
        }
        private static void InitializeHttpClient()
        {
            string sessionId = _config["sessionId"];
            _httpClient.DefaultRequestHeaders.Add("cookie", $"session={sessionId}");
        }
        public static async Task<IReadOnlyList<T>> GetInputLines<T>(string url, string tupleArgumentSeparator = " ")
        {
            var inputLines = (await _httpClient.GetStringAsync(url))
                .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (typeof(T).IsGenericType)
            {
                var genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(ValueTuple<,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,,,,>))
                {
                    var parsingMethodsPerTypeArgument =
                        typeof(T).GenericTypeArguments
                        .Select(typeArgument =>
                            typeof(InputHelper)
                            .GetMethod(nameof(ParseString), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                            .MakeGenericMethod(typeArgument))
                        .ToList();
                    return inputLines.Select(x =>
                    {
                        string[] splitted = x.Split(tupleArgumentSeparator);
                        object[] tupleArguments = new object[parsingMethodsPerTypeArgument.Count()];
                        for (int i = 0; i < parsingMethodsPerTypeArgument.Count; ++i)
                        {
                            tupleArguments[i] = parsingMethodsPerTypeArgument[i].Invoke(null, new object[] { splitted[i] });
                        }
                        return (T)Activator.CreateInstance(typeof(T), tupleArguments);
                    }).ToList();
                }
            }
            else
                return inputLines.Select(x => ParseString<T>(x)).ToList();
            throw new NotImplementedException();
        }

        private static T ParseString<T>(string s)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int64:
                    return (T)(object)long.Parse(s);
                case TypeCode.Int32:
                    return (T)(object)int.Parse(s);
                case TypeCode.String:
                    return (T)(object)s;
            }
            throw new NotImplementedException();
        }
    }
}
