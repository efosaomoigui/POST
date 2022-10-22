using System.Threading.Tasks;

namespace POST.Core.IServices.Utility
{
    public interface IPasswordGenerator : IServiceDependencyMarker
    {
        Task<string> Generate(int length = 6);
    }
}
