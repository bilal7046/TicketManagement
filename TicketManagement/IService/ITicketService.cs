using TicketManagement.Entities;

namespace TicketManagement.IService
{
    public interface ITicketService
    {
        Task<string> BookTicket(Booking booking);

        Task UpdatePaymentStatus(string sessionId);

        Task<PromoCode> ValidatePromoCode(string code);

        Task<IEnumerable<TicketType>> GetTicketTypes();

        Task<TicketType> CheckAvailability(int ticketId);
    }
}