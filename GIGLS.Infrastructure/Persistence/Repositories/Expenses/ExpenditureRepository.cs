using GIGLS.Core.Domain.Expenses;
using GIGLS.Core.IRepositories.Expenses;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Expenses;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.Infrastructure.Persistence.Repositories.Expenses
{
    public class ExpenditureRepository : Repository<Expenditure, GIGLSContext>, IExpenditureRepository
    {
        private GIGLSContext _context;

        public ExpenditureRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
        
        public Task<List<ExpenditureDTO>> GetExpenditures(ExpenditureFilterCriteria expenditureFilterCriteria, int[] serviceCentreIds)
        {
            var expenditures = _context.Expenditure.AsQueryable().Where(x => x.IsDeleted == false);
            
            if(serviceCentreIds.Length > 0)
            {
                expenditures = expenditures.Where(x => serviceCentreIds.Contains(x.ServiceCentreId));
            }

            if (expenditureFilterCriteria.ExpenseTypeId > 0)
            {
                expenditures = expenditures.Where(x => x.ExpenseTypeId == expenditureFilterCriteria.ExpenseTypeId);
            }

            if (expenditureFilterCriteria.ServiceCentreId > 0)
            {
                expenditures = expenditures.Where(x => x.ServiceCentreId == expenditureFilterCriteria.ServiceCentreId);
            }

            if(expenditureFilterCriteria.StationId > 0)
            {
                var serviceCentre = _context.ServiceCentre.Where(x => x.StationId == expenditureFilterCriteria.StationId);
                expenditures = expenditures.Where(y => serviceCentre.Any(x => y.ServiceCentreId == x.ServiceCentreId));
            }

            if(expenditureFilterCriteria.StateId > 0)
            {
                var station = _context.Station.Where(s => s.StateId == expenditureFilterCriteria.StateId);
                var serviceCentre = _context.ServiceCentre.Where(w => station.Any(x => w.StationId == x.StationId));
                expenditures = expenditures.Where(y => serviceCentre.Any(x => y.ServiceCentreId == x.ServiceCentreId));
            }
            
            var queryDate = expenditureFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            expenditures = expenditures.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            List<ExpenditureDTO> expendituresDto = (from e in expenditures
                                                    select new ExpenditureDTO
                                                    {
                                                        ExpenditureId = e.ExpenditureId,
                                                        Amount = e.Amount,
                                                        Description = e.Description,
                                                        ServiceCentreId = e.ServiceCentreId,
                                                        ServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == e.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                        {
                                                            Code = x.Code,
                                                            Name = x.Name
                                                        }).FirstOrDefault(),
                                                        DateCreated = e.DateCreated,
                                                        DateModified = e.DateModified,
                                                        ExpenseTypeId = e.ExpenseTypeId,
                                                        ExpenseType = Context.ExpenseType.Where(c => c.ExpenseTypeId == e.ExpenseTypeId).Select(x => new ExpenseTypeDTO
                                                        {
                                                            ExpenseTypeName = x.ExpenseTypeName
                                                        }).FirstOrDefault(),
                                                        UserId = e.UserId,
                                                        CreatedBy = Context.Users.Where(u => u.Id == e.UserId).Select(s => s.LastName + " " + s.FirstName).FirstOrDefault()
                                                    }).OrderByDescending(x => x.DateCreated).ToList();
            return Task.FromResult(expendituresDto);        }        
    }
}