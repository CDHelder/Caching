using System.Collections.Generic;
using System.Threading.Tasks;
using TypiCode.Domain.Interfaces;
using TypiCode.Domain.Models;

namespace TypiCode.Business
{
    public class CommentService : IComments
    {
        private readonly ITypiCodeClient typiCodeClient;

        public CommentService(ITypiCodeClient typiCodeClient)
        {
            this.typiCodeClient = typiCodeClient;
        }

        public async Task<IEnumerable<Comment>> GetsAsync(int postId)
        {
            return await typiCodeClient.Execute<IEnumerable<Comment>>(Constants.CommentUrl + "?postId=" + postId);
        }
    }
}
