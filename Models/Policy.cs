using System.ComponentModel.DataAnnotations;

namespace BackendTob.Models
{
    public class Policy
    {
        [Key]
        public int Id { get; set; }

        public string? PolicyNumber { get; set; }

        [Required]
        public string BeneficiaryName { get; set; }

        [Required]
        public string CarBrand { get; set; }

        [Required]
        public string CarType { get; set; }

        [Required]
        public decimal TSI { get; set; }

        [Required]
        public decimal PremiumRate { get; set; }

        public decimal PremiumAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
