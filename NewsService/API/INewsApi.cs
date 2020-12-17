using System;
using System.Threading.Tasks;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace NewsService.API
{
    public interface INewsApi
    {
        Task<ArticlesResult> GetNews(INewsRequest request);
    }

    public class NewsApi : INewsApi
    {
        private readonly NewsApiClient _client;

        public NewsApi(NewsApiClient client)
        {
            _client = client;
        }
        public Task<ArticlesResult> GetNews(INewsRequest request)
        {
            return request switch
            {
                Everything everything => _client.GetEverythingAsync(new EverythingRequest()
                {
                    Q = everything.Query,
                    PageSize = everything.PageSize,
                    Page = everything.Page,
                    Language = everything.Language,
                    From = everything.From,
                    To = everything.To,
                    SortBy = everything.SortBy
                }),
                Top top => _client.GetTopHeadlinesAsync(new TopHeadlinesRequest()
                {
                    Q = top.Query,
                    Category = top.Category,
                    Country = top.Country,
                    Language = top.Language,
                }),
                _ => throw new ArgumentOutOfRangeException(nameof(request))
            };
        }
    }

    public interface INewsRequest
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Languages Language { get; set; }
    }

    public abstract class NewsRequest : INewsRequest
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Languages Language { get; set; }

        protected NewsRequest()
        {

        }

        protected NewsRequest(string query)
        {
            Query = query;
        }

        protected NewsRequest(string query, int pageSize = 20, int page = 0, Languages language = Languages.EN) : this(query)
        {
            Page = page;
            PageSize = pageSize;
            Language = language;
        }
    }

    public class Top : NewsRequest
    {
        public Categories Category { get; set; }
        public Countries Country { get; set; }

        public Top()
        {

        }

        public Top(string query) : base(query)
        {
        }

        public Top(string query, Categories category = Categories.Technology, Countries country = Countries.GB) : this(query)
        {
            Category = category;
            Country = country;
        }
    }

    public class Everything : NewsRequest
    {
        public Everything()
        {

        }
        public Everything(string query) : base(query)
        {
        }

        public Everything(string query, SortBys sortBy = SortBys.Popularity) : this(query)
        {
            SortBy = sortBy;
        }

        public DateTime? From { get; set; } = DateTime.Now.AddHours(12);
        public DateTime? To { get; set; } = DateTime.Now;
        public SortBys? SortBy { get; set; }
    }

    public enum RequestType
    {
        TopHeadlines,
        Everything
    }
}
