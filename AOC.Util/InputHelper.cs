using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.Util
{
    public static class InputHelper
    {
        private static readonly HttpClient _httpClient = new();
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
        private static async Task<string> GetOrCreateInputFileData(string url)
        {
            string fileName = url[(url.LastIndexOf('/') + 1)..];
            string data;
            try
            {
                data = File.ReadAllText(fileName);
            }
            catch
            {
                data = await _httpClient.GetStringAsync(url);
                File.WriteAllText(fileName, data);
            }
            return data;
        }
        public static Task<string> GetInputText(string url) => GetOrCreateInputFileData(url);
        public static async Task<IReadOnlyList<T>> GetInputLines<T>(string url, string argumentSeparatorRegex = " ", string lineSeparatorRegex = "\n")
        {
            string data = await GetOrCreateInputFileData(url);
            var inputLines = Regex.Split(data, lineSeparatorRegex)
                .Where(x => ! string.IsNullOrWhiteSpace(x))
                .ToList();

#pragma warning disable IDE0062 // Make local function 'static'
            MethodInfo GetParsingMethod(Type type)
#pragma warning restore IDE0062 // Make local function 'static'
            {
                return typeof(InputHelper)
                    .GetMethod(nameof(ParseString), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .MakeGenericMethod(type);
            } 
            if(typeof(T).IsArray)
            {
                var elementType = typeof(T).GetElementType();
                var parsingMethod = GetParsingMethod(elementType);
                return inputLines.Select(x =>
                {
                    string[] splitted = Regex.Split(x, argumentSeparatorRegex)
                        .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    var array = Array.CreateInstance(elementType, splitted.Length);
                    for (int i = 0; i < splitted.Length; ++i)
                    {
                        array.SetValue(parsingMethod.Invoke(null, new object[] { splitted[i] }), i);
                    }
                    return (T)(object)array;
                }).ToList();
            }
            else if (typeof(T).IsGenericType)
            {
                var genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(ValueTuple<,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,,,>)
                    || genericTypeDefinition == typeof(ValueTuple<,,,,,,,>)
                    || genericTypeDefinition == typeof(Tuple<,>))
                {
                    var parsingMethodsPerTypeArgument =
                        typeof(T).GenericTypeArguments
                        .Select(typeArgument => GetParsingMethod(typeArgument))
                        .ToList();
                    return inputLines.Select(x =>
                    {
                        string[] splitted = Regex.Split(x, argumentSeparatorRegex);
                        if (splitted.Length < parsingMethodsPerTypeArgument.Count)
                            throw new NotImplementedException();
                        object[] tupleArguments = new object[parsingMethodsPerTypeArgument.Count];
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
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (Type.GetTypeCode(typeof(T)))
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
                case TypeCode.Int64:
                    return (T)(object)long.Parse(s);
                case TypeCode.Int32:
                    return (T)(object)int.Parse(s);
                case TypeCode.String:
                    return (T)(object)s;
                case TypeCode.Char:
                    return (T)(object)s[0];
            }
            throw new NotImplementedException();
        }
    }
}
