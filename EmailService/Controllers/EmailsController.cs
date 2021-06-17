using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMictoservice.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class EmailsController : ControllerBase {

        private readonly ILogger<EmailsController> _logger;
        private EmailService _service;

        public EmailsController(ILogger<EmailsController> logger, EmailService service) {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGoalEmail(EmailDTO request) {
            List<string> emailsList = await _service.GetEmailReceivers(request.ToEveryone);

            if (emailsList.Count == 0)
                return Ok("No one subscribed to notification");

            bool isSuccesfulEmail = await _service.SendEmail(request.Text,emailsList);
            if(isSuccesfulEmail)
                return Ok("Emails were send");
            return BadRequest("Notification were not send");
        }
    }
}
