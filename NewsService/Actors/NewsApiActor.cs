using System.Threading.Tasks;
using NewsService.API;
using Proto;

namespace NewsService.Actors
{
    public class NewsApiActor : IActor
    {
        private readonly INewsApi _api;

        public NewsApiActor(INewsApi api)
        {
            _api = api;
        }
        public Task ReceiveAsync(IContext context)
        {
            return context.Message switch
            {
                INewsRequest request => HandleRequest(request, context),
                _ => Task.CompletedTask
            };

        }

        private async Task HandleRequest(INewsRequest request, IContext context)
        {
            var response = await _api.GetNews(request);
            context.Respond(response);
        }
    }
}
