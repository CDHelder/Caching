using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TypiCode.Domain.Interfaces;
using TypiCode.Domain.Models;

namespace TypiCode.Business
{
    public class PostService: IPosts
    {
        private readonly ITypiCodeClient typiCodeClient;
        private readonly ICache cache;

        public PostService(ITypiCodeClient typiCodeClient, ICache cache)
        {
            this.typiCodeClient = typiCodeClient;
            this.cache = cache;
        }

        public async Task<IEnumerable<Post>> Get()
        {
            var posts = await cache.GetOrCreate("AllPosts", 
                () => typiCodeClient.Execute<IEnumerable<Post>>(Constants.PostsUrl),
                ExpirationType.Absolute, 
                TimeSpan.FromSeconds(300));

            return posts;
        }

        public async Task<Post> Get(int id)
        {
            if(id <= 0)
                throw new ArgumentNullException(nameof(id));

            var post = await cache.GetOrCreate($"Post:{id}",
                () => typiCodeClient.Execute<Post>(Constants.PostsUrl + "/" + id),
                ExpirationType.Sliding,
                TimeSpan.FromSeconds(60));

            return post;
        }
    }
}
