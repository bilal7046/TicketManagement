using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Entities
{
    public class TicketType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }
    }
}