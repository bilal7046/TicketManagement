using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.Enums;

namespace TicketManagement.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DOB { get; set; } = DateTime.Now;

        [Required]
        public string EventName { get; set; } = "My Event";

        [Required]
        public int TicketTypeId { get; set; }

        public TicketType TicketType { get; set; }
        public int? PromoCodeId { get; set; }
        public PromoCode PromoCode { get; set; }
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;
        public string SessionId { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;

        public decimal? Discount { get; set; }

        [NotMapped]
        public string PromoCodeStr { get; set; }
    }
}