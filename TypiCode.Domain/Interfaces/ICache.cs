using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypiCode.Domain.Models;

namespace TypiCode.Domain.Interfaces
{
    public interface ICache
    {
        public Task<TObject> GetOrCreate<TObject>(string key, Func<Task<TObject>> createObject, ExpirationType type, TimeSpan time);
    }
}
