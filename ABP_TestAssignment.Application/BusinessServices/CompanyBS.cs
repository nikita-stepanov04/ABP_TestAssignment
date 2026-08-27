using ABP_TestAssignment.Application.Cryptography;
using ABP_TestAssignment.Application.DTOs.Companies;
using ABP_TestAssignment.Application.IBusinessServices;
using ABP_TestAssignment.Domain.Entities.Companies;
using ABP_TestAssignment.Infrastructure.IRepositories.Companies;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace ABP_TestAssignment.Application.BusinessServices
{
    public class CompanyBS : ICompanyBS
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _chatRep;
        private readonly AdminSettings _adminSettings;

        public CompanyBS(
            IMapper mapper,
            ICompanyRepository chatRep,
            IOptions<AdminSettings> adminSettings)
        {
            _mapper = mapper;
            _chatRep = chatRep;
            _adminSettings = adminSettings.Value;
        }

        public async Task<CompanyDTO?> GetByEmailAsync(string email)
        {
            var company = await _chatRep.GetByEmailAsync(email);
            return _mapper.Map<CompanyDTO?>(company);
        }

        public async Task<CompanyDTO?> CheckCredentialsForLoginAsync(CompanyLogInDTO companyDTO)
        {
            var company = await _chatRep.GetByEmailAsync(companyDTO.Email);

            if (company == null) return null;

            var passwordHashingData = Hashing.PBKDF2(companyDTO.Password, company.PasswordSalt);
            if (!passwordHashingData.Hash.SequenceEqual(company.PasswordHash))
                return null;

            return _mapper.Map<CompanyDTO>(company);
        }


        public async Task<bool> IsEmailNotTakenAsync(string email)
        {
            return (await _chatRep.GetByEmailAsync(email)) == null;
        }

        public async Task<OpRes<long>> RegisterCompanyAsync(CompanyRegistrationDTO companyDTO)
        {
            var company = _mapper.Map<Company>(companyDTO);

            if (!await IsEmailNotTakenAsync(company.Email))
            {
                return OpRes.Err<long>("Email is already taken");
            }
            else if (
                companyDTO.Role == Role.Admin &&
                companyDTO.MasterPassword != _adminSettings.AdminMasterPassword)
            {
                return OpRes.Err<long>("Invalid master password for admin registration");
            }

            var hashRes = Hashing.PBKDF2(companyDTO.Password);
            company.PasswordHash = hashRes.Hash;
            company.PasswordSalt = hashRes.Salt;
            company.CreatedAt = DateTime.UtcNow;

            await _chatRep.AddAsync(company);
            await _chatRep.SaveChangesAsync();
            return OpRes.Success(company.ID);
        }
    }
}
