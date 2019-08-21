using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlidEnglish.App.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SlidEnglish.Web.IntegrationTests
{
    public static class HttpMessageExtensions
    {
        public static async Task<Dictionary<string, object>> ToDictionary(this HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        public static async Task<object[]> ToArray(this HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<object[]>(json);
        }

        public static async Task<Dictionary<string, object>[]> ToArrayOfDictionaries(this HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, object>[]>(json);
        }

		public static async Task<T> ToGraphQlResponse<T>(this HttpResponseMessage response, string field)
		{
			string json = await response.Content.ReadAsStringAsync();

			var jobject = JObject.Parse(json);
			return jobject["data"][field].ToObject<T>();
		}
	}
}
