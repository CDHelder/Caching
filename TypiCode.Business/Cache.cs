using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypiCode.Domain.Interfaces;
using TypiCode.Domain.Models;

namespace TypiCode.Business
{
    public class Cache : ICache
    {
        private readonly IMemoryCache memoryCache;

        //public MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
        public ConcurrentDictionary<string, SemaphoreSlim> locks = new();

        public Cache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task<TObject> GetOrCreate<TObject>(string key, Func<Task<TObject>> createObject, ExpirationType expirationType, TimeSpan time)
        {
            TObject cacheEntry;

            //Vraag: Vanwaar twee keer dezelfde if
            if (!memoryCache.TryGetValue(key, out cacheEntry))
            {
                var myLocks = locks.GetOrAdd(key, new SemaphoreSlim(1, 1));

                await myLocks.WaitAsync();

                try
                { 
                    if (!memoryCache.TryGetValue(key, out cacheEntry))
                    {
                        MemoryCacheEntryOptions options = new();

                        if (expirationType == ExpirationType.Absolute)
                            options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(time);
                        else if(expirationType == ExpirationType.Sliding)
                            options = new MemoryCacheEntryOptions().SetSlidingExpiration(time);

                        cacheEntry = await createObject();
                        memoryCache.Set(key, cacheEntry, options);
                    }
                }
                finally
                {
                    myLocks.Release();
                }
            }

            return cacheEntry;
        }
    }
}
