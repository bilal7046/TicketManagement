using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Entities;
using TicketManagement.IService;

namespace TicketManagement.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> BookTicket(Booking booking)
        {
            var paymentUrl = await _ticketService.BookTicket(booking);

            return Ok(paymentUrl);
        }

        [HttpGet("check-availability/{ticketId}")]
        public async Task<IActionResult> CheckAvailability(int ticketId)
        {
            var ticketType = await _ticketService.CheckAvailability(ticketId);

            return Ok(ticketType);
        }

        [HttpGet("validate-promo-code")]
        public async Task<IActionResult> ValidatePromoCode(string promoCode)
        {
            // Replace this with your database query to validate the promo code
            var promo = await _ticketService.ValidatePromoCode(promoCode);

            if (promo == null)
            {
                return BadRequest("Invalid or expired promo code.");
            }

            return Ok(new { Discount = promo.Discount });
        }
    }
}