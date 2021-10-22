using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TypiCode.Business;
using TypiCode.Domain.Interfaces;
using TypiCode.Domain.Models;

namespace ConsoleApp
{
    internal class Program
    {
        private static IPosts postService;
        private static IComments commentService;

        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            postService = serviceProvider.GetService<IPosts>();
            commentService = serviceProvider.GetService<IComments>();

            PostOverview().Wait();
        }

        private async static Task PostOverview()
        {
            var posts = await postService.Get();

            Console.WriteLine(posts.ToStringTable(
                new[] { "Id", "Title" },
                a => a.Id, a => a.Title)
            );

            var selected = Console.ReadLine();

            await ShowPostDetails(Convert.ToInt32(selected));
        }


        static async Task ShowPostDetails(int postId)
        {
            Console.Clear();
            var post = await postService.Get(postId);

            Console.WriteLine("Title: " + post.Title);
            Console.WriteLine("Body: " + post.Body);

            var comments = await commentService.GetsAsync(postId);

            Console.WriteLine();
            Console.WriteLine(">>>> COMMENTS <<<<");
            Console.WriteLine();

            foreach (var item in comments)
            {
                ShowComment(item);
            }

            Console.ReadLine();

            await PostOverview();
        }

        static void ShowComment(Comment comment)
        {
            Console.WriteLine(comment.Name + " (" + comment.Email + ")");
            Console.WriteLine();
            Console.WriteLine(comment.Body);
            Console.WriteLine(new string('-', 73));
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITypiCodeClient, TypiCodeClient>();
            services.AddScoped<IPosts, PostService>();
            services.AddScoped<IComments, CommentService>();

            services.AddScoped<ICache, Cache>();

            services.AddMemoryCache();
        }
    }
}
