using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BroadcastController
        : ControllerBase
    {
        private readonly IHubContext<TestHub> _hubContext;

        public BroadcastController(IHubContext<TestHub> hub)
        {
            _hubContext = hub;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Get()
        {
            await _hubContext.Clients.All.SendAsync("globalMessage", $"Broadcasting message to all users '{Guid.NewGuid()}'.");
            return Accepted();
        }
    }
}
