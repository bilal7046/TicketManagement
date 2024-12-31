using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public string StripeTransactionId { get; set; }

        [Required]
        public string StripeJsonResponse { get; set; }

        public Booking Booking { get; set; }
    }
}