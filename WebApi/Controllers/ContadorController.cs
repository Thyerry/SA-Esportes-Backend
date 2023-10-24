using System.Diagnostics;
using Domain.Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContadorController : ControllerBase
    {
        private readonly IRabbitMQService _rabbitmqService;
        private readonly IMessageService _messageService;

        public ContadorController(IRabbitMQService rabbitmqService, IMessageService messageService)
        {
            _rabbitmqService = rabbitmqService;
            _messageService = messageService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendText(MessageDto text)
        {
            return Ok(_messageService.SendText(text));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetMessages()
        {
            return Ok();
        }
    }
}