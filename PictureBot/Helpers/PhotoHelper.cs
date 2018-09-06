using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace PictureBot.Helpers
{
    public static class PhotoHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _basePath =
            $"https://api.unsplash.com/photos/random?client_id={ConfigurationManager.AppSettings["client_id"]}";

        public static async Task<string> GetPhotoUrlAsync(string topic)
        {
            if (String.IsNullOrEmpty(topic))
                throw new ArgumentOutOfRangeException("topic", "is null or empty.");

            Uri uri = new Uri($"{_basePath}&query={Uri.EscapeDataString(topic)}");
            var response = await _httpClient.GetStringAsync(uri);
            var data = JObject.Parse(response);
            return (string)data["urls"]["small"];
        }
    }
}
