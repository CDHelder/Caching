using System.Collections.Generic;
using System.Threading.Tasks;
using TypiCode.Domain.Models;

namespace TypiCode.Domain.Interfaces
{
    public interface IPosts
    {
        Task<IEnumerable<Post>> Get();

        Task<Post> Get(int id);
    }
}
