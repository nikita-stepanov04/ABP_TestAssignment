using System.ComponentModel.DataAnnotations;

namespace ABP_TestAssignment.Application
{
    public class AdminSettings
    {
        [Required] public string AdminMasterPassword { get; set; } = null!;
    }
}
