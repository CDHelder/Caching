using System.Collections.Generic;
using System.Threading.Tasks;
using TypiCode.Domain.Models;

namespace TypiCode.Domain.Interfaces
{
    public interface IComments
    {
        Task<IEnumerable<Comment>> GetsAsync(int postId);
    }
}
