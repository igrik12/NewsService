using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAPI.Models;
using NewsService.Actors;
using NewsService.API;
using Proto;
using Proto.DependencyInjection;

namespace NewsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {

        private readonly ILogger<NewsController> _logger;
        private readonly ActorSystem _actorSystem;

        public NewsController(ILogger<NewsController> logger, ActorSystem actorSystem)
        {
            _logger = logger;
            _actorSystem = actorSystem;
        }

        [HttpGet("top")]
        public async Task<ArticlesResult> Get([FromBody] Top request)
        {
            var props = _actorSystem.DI().PropsFor<NewsApiActor>();
            var pid = _actorSystem.Root.Spawn(props);
            var res = await _actorSystem.Root.RequestAsync<ArticlesResult>(pid, request);
            await _actorSystem.Root.StopAsync(pid);
            return res;
        }

        [HttpGet("everything")]
        public async Task<ArticlesResult> Get([FromBody] Everything request)
        {
            var props = _actorSystem.DI().PropsFor<NewsApiActor>();
            var pid = _actorSystem.Root.Spawn(props);
            var res = await _actorSystem.Root.RequestAsync<ArticlesResult>(pid, request);
            await _actorSystem.Root.StopAsync(pid);
            return res;
        }
    }
}
