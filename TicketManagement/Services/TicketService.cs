using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using TicketManagement.Data;
using TicketManagement.Entities;
using TicketManagement.Enums;
using TicketManagement.IService;

namespace TicketManagement.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketService(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> BookTicket(Booking booking)
        {
            var ticket = await _context.TicketTypes.FirstOrDefaultAsync(t => t.Id == booking.TicketTypeId && t.AvailableQuantity > 0);

            if (ticket == null)
                throw new Exception("Selected ticket is sold out");

            var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == booking.PromoCodeStr && p.IsActive);
            decimal discount = promo?.Discount ?? 0;
            decimal finalPrice = (ticket.Price - discount) * booking.Quantity;

            //add booking

            Booking newBooking = new Booking()
            {
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                DOB = booking.DOB,
                Email = booking.Email,
                TicketTypeId = ticket.Id,
                PromoCodeId = promo?.Id,
                TotalPrice = finalPrice,
                Discount = discount,
            };

            _context.Bookings.Add(newBooking);

            // Proceed to Stripe Payment
            string paymentUrl = ProcessStripePayment(finalPrice, booking.Email, newBooking.Id);

            if (paymentUrl == null)
                throw new Exception("Payment faild!");

            //add payment session
            newBooking.SessionId = _httpContextAccessor.HttpContext.Session.GetString("SessionId");

            // Reduce Ticket Quantity
            ticket.AvailableQuantity -= 1;
            _context.SaveChanges();

            return paymentUrl;
        }

        private string ProcessStripePayment(decimal amount, string email, int bookingId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = amount * 100, // Stripe uses cents
                        Currency = "gbp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Event Ticket"
                        }
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = $"{_configuration["AppUrl"]}/TicketBooking/Success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_configuration["AppUrl"]}/TicketBooking/Cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            _httpContextAccessor.HttpContext.Session.SetString("SessionId", session.Id);

            return session.Url;
        }

        public async Task<PromoCode> ValidatePromoCode(string code)
        {
            var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code && p.IsActive);
            return promo;
        }

        public async Task UpdatePaymentStatus(string sessionId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(z => z.SessionId == sessionId);

            booking.BookingStatus = BookingStatus.Paid;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketType>> GetTicketTypes()
        {
            return await _context.TicketTypes.AsNoTracking().ToListAsync();
        }

        public async Task<TicketType> CheckAvailability(int ticketId)
        {
            var ticketType = await _context.TicketTypes.FirstOrDefaultAsync(p => p.Id == ticketId);
            return ticketType;
        }
    }
}