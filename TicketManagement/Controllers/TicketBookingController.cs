using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;
using System.Net.Http;
using System.Text;
using TicketManagement.Entities;
using TicketManagement.IService;

namespace TicketManagement.Controllers
{
    public class TicketBookingController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public TicketBookingController(ITicketService ticketService, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _ticketService = ticketService;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BookTicket()
        {
            await LoadData();

            return View(new Booking());
        }

        private async Task LoadData()
        {
            var ticketTypes = await _ticketService.GetTicketTypes();
            ViewData["TicketTypes"] = ticketTypes;
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket(Booking booking)
        {
            await LoadData();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (booking.TicketTypeId == 0)
            {
                ModelState.AddModelError("", "Please select ticket type");
                return View();
            }
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var apiUrl = $"{_configuration["AppUrl"]}/api/ticket/BookTicket";

                var content = new StringContent(
                    JsonConvert.SerializeObject(booking),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var paymentUrl = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(paymentUrl))
                    {
                        return Redirect(paymentUrl);
                    }
                }

                ModelState.AddModelError(string.Empty, "Failed to process the booking. Please try again.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while booking the ticket. Please try again.");
            }

            return View();
        }

        public async Task<IActionResult> Success(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                var service = new SessionService();
                Session session = service.Get(sessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    await _ticketService.UpdatePaymentStatus(sessionId);
                }
            }
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}