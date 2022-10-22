using POST.Core.IServices.Utility;
using System;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Utility
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public Task<string> Generate(int length)
        {
            var strippedText = Guid.NewGuid().ToString().Replace("-", string.Empty);
            return Task.FromResult(strippedText.Substring(0, length));
        }
    }
}
