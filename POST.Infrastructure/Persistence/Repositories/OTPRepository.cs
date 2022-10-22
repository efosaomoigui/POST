using System;
using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class OTPRepository : Repository<OTP, GIGLSContext>, IOTPRepository
    {
        public OTPRepository(GIGLSContext context) : base(context)
        {
        }

       
        public async Task<OTP> IsOTPValid(int OTP)
        {
            var message = new OTP();
            DateTime LatestTime = DateTime.Now;
            try
            {
                message = Context.OTP.Where(x => x.Otp == OTP && x.IsValid==false).FirstOrDefault();
                if (message == null)
                {
                    throw new GenericException("Invalid OTP");
                }
                else if (message != null)
                {
                    TimeSpan span = LatestTime.Subtract(message.DateCreated);
                    int difference = Convert.ToInt32(span.TotalMinutes);
                    if (difference < 5)
                    {
                        message.IsValid = true;
                    }
                    else
                    {
                        throw new GenericException("OTP has expired!.Kindly click on Resendotp.");
                    }
                    return await Task.FromResult(message);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return await Task.FromResult(message);
        }
        
    }
}
