using System;
using System.Runtime.Caching;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxyCache
{
    public class GlobalCache
    {
        ObjectCache cache;
        DateTimeOffset dt_default;
        static readonly HttpClient client = new HttpClient();

        public GlobalCache()
        {
            cache = new MemoryCache("mycache");
            dt_default = ObjectCache.InfiniteAbsoluteExpiration;
        }

        public void Add(string name, string content, DateTimeOffset dt)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = dt,
            };

            cache.Add(name, content, cacheItemPolicy);
        }

        public string Get(string name, double dt_seconds)
        {
            if (cache.Get(name) == null)
            {
                string content = getContentAsync(name).Result;
                this.Add(name, content, DateTimeOffset.Now.AddSeconds(dt_seconds));
                return content;
            }
            return (string)cache.Get(name);
        }

        public string Get(string name)
        {
            if (cache.Get(name) == null)
            {
                string content = getContentAsync(name).Result;
                this.Add(name, content, dt_default);
                return content;
            }
            return (string)cache.Get(name);
        }

        public string Get(string name, DateTimeOffset dt)
        {
            if (cache.Get(name) == null)
            {
                string content = getContentAsync(name).Result;
                this.Add(name, content, dt);
                return content;
            }
            return (string)cache.Get(name);
        }

        private async Task<string> getContentAsync(string request)
        {
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
