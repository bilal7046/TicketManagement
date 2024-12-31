using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Entities
{
    public class PromoCode
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}