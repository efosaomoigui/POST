using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core;
using System.Collections.Generic;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Customers
{
    public class CompanyContactPersonService : ICompanyContactPersonService
    {
        private readonly IUnitOfWork _uow;

        public CompanyContactPersonService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<object> AddContactPerson(CompanyContactPersonDTO contactPerson)
        {
            try
            {
                var company = await _uow.Company.GetAsync(contactPerson.CompanyId);
                if (company == null)
                {
                    throw new GenericException("Company Not Exist");
                }
                var NewContactPerson = new CompanyContactPerson
                {
                    FirstName = contactPerson.FirstName,
                    LastName = contactPerson.LastName,
                    Email = contactPerson.Email,
                    Designation = contactPerson.Designation,
                    PhoneNumber = contactPerson.PhoneNumber,
                    CompanyId = company.CompanyId
                };
                _uow.CompanyContactPerson.Add(NewContactPerson);
                _uow.Complete();
                return new { id = NewContactPerson.CompanyContactPersonId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteContactPerson(int id)
        {
            try
            {
                var person = await _uow.CompanyContactPerson.GetAsync(id);
                if (person == null)
                {
                    throw new GenericException("Contact Person Not Exist");
                }
                _uow.CompanyContactPerson.Remove(person);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CompanyContactPersonDTO> GetContactPersonById(int id)
        {
            try
            {
                var person = await _uow.CompanyContactPerson.GetAsync(id);
                if (person == null)
                {
                    throw new GenericException("Contact Person Not Exist");
                }

                //using mapping here
                CompanyContactPersonDTO personDto = new CompanyContactPersonDTO();
                personDto.CompanyContactPersonId = person.CompanyContactPersonId;
                personDto.FirstName = person.FirstName;
                personDto.LastName = person.LastName;
                personDto.Email = person.Email;
                personDto.PhoneNumber = person.PhoneNumber;
                personDto.DateModified = person.DateModified;
                personDto.DateCreated = person.DateCreated;
                personDto.CompanyId = person.CompanyId;

                //company Detail
                if (personDto.CompanyId != 0)
                {
                    var company = await _uow.Company.GetAsync(personDto.CompanyId);
                    personDto.CompanyAddress = company.Address;
                    personDto.CompanyCity = company.City;
                    personDto.CompanyType = company.CompanyType;
                    personDto.CompanyDiscount = company.Discount;
                    personDto.CompanyEmail = company.Email;
                    personDto.CompanyIndustry = company.Industry;
                    personDto.CompanyName = company.Name;
                    personDto.CompanyPhoneNumber = company.PhoneNumber;
                    personDto.CompanyState = company.State;
                    personDto.CompanyRcNumber = company.RcNumber;
                    personDto.CompanyStatus = company.CompanyStatus;
                }
                return personDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CompanyContactPersonDTO>> GetContactPersons()
        {
            try
            {
                return await _uow.CompanyContactPerson.GetCompanyContactPerson();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateContactPerson(int contactPersonId, CompanyContactPersonDTO contactPerson)
        {
            var person = await _uow.CompanyContactPerson.GetAsync(contactPersonId);
            if (person == null || contactPersonId != contactPerson.CompanyContactPersonId)
            {
                throw new GenericException("Contact Person Not Exist");
            }

            var company = await _uow.Company.GetAsync(contactPerson.CompanyId);
            if (company == null )
            {
                throw new GenericException("Company Not Exist");
            }

            person.FirstName = contactPerson.FirstName;
            person.LastName = contactPerson.LastName;
            person.Email = contactPerson.Email;
            person.Designation = contactPerson.Designation;
            person.PhoneNumber = contactPerson.PhoneNumber;
            person.CompanyId = contactPerson.CompanyId;
            _uow.Complete();
        }
    }
}
