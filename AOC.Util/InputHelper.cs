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
        public static async Task<IReadOnlyList<T>> GetInputLines<T>(string uri)
        {
            var inputLines = (await _httpClient.GetStringAsync(uri))
                .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int64:
                        return inputLines.Select(x => (T)(object)long.Parse(x)).ToList();
                case TypeCode.Int32:
                    return inputLines.Select(x => (T)(object)int.Parse(x)).ToList();
                case TypeCode.String:
                    return inputLines.Select(x => (T)(object)x).ToList();
            }
            throw new NotImplementedException();
        }
    }
}
