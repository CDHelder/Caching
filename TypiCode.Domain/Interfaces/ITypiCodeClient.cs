using System.Threading.Tasks;

namespace TypiCode.Domain.Interfaces
{
    public interface ITypiCodeClient
    {
        Task<T> Execute<T>(string url);
    }
}
