using ABP_TestAssignment.Application.DTOs.Companies;

namespace ABP_TestAssignment.Application.IBusinessServices.Companies
{
    public interface ICompanyBS
    {
        Task<CompanyDTO?> GetByEmailAsync(string email);
        Task<OpRes<long>> RegisterCompanyAsync(CompanyRegistrationDTO companyDTO);
        Task<bool> IsEmailNotTakenAsync(string email);
        Task<CompanyDTO?> CheckCredentialsForLoginAsync(CompanyLogInDTO companyDTO);
    }
}
