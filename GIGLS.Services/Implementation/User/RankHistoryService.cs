using System;
using System.Threading.Tasks;
using GIGLS.Core;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IRepositories.User;
using GIGLS.Core.IServices.Stores;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.User
{
    public class RankHistoryService : IRankHistoryService
    {

        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public RankHistoryService(IUserService userService,IUnitOfWork uow)
        {
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task AddRankHistory(RankHistoryDTO history)
        {

            _uow.RankHistory.Add(new Core.Domain.RankHistory 
            {
                RankHistoryId = history.RankHistoryId,
                CustomerName = history.CustomerName,
                CustomerCode = history.CustomerCode,
                RankType = history.RankType,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            });
            await _uow.CompleteAsync();
        }
    }
}
