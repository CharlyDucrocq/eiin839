using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProxyCache;

namespace ProxyCache
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class CacheService : ICacheService
    {
        private GlobalCache cache = new GlobalCache();
        public string getResultFromRequest(string request)
        {
            return cache.Get(request, 60).ToString();
        }

        public string getResultFromRequestWithTimeLimit(string request, double timeLimit)
        {
            return cache.Get(request, timeLimit).ToString();
        }
    }
}
