using System.ComponentModel.DataAnnotations;

namespace CompanyEmployee.BlazorUI.Models.Company
{
    public class CompanyVM
    {
        public Guid Id { get; init; } /*init to enable the XML serializer of field*/
        [Required]
        public string? Name { get; init; }
        [Required]
        public string? FullAddress { get; init; }
    }
}
