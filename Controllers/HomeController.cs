using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Pro.Hubs;
using SignalR.Pro.Models;
using System.Diagnostics;

namespace SignalR.Pro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<RecevieHub> _receiveHub;

        public HomeController(ILogger<HomeController> logger, IHubContext<RecevieHub> receiveHub)
        {
            _logger = logger;
            _receiveHub = receiveHub;
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Receive()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SendData([FromBody] SendMsgDto dto)
        {
            await _receiveHub.Clients.Client(dto.ConnectionId).SendAsync("ReceiveMessage", $"{dto.ConnectionId}: {dto.Message}");
            _ = ReceiveData(dto.ConnectionId);
            return Ok("success");
        }


        private async Task ReceiveData(string connectionId)
        {
            await Task.Delay(5000);
            await _receiveHub.Clients.Client(connectionId).SendAsync("ReceiveData", $"{connectionId}: 等你很久了.........");
        }
    }
}