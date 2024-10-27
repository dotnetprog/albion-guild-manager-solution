namespace AGM.Application.Features.Caching.Abstractions
{
    public interface ICacheableQuery
    {
        public string CacheKey { get; }
        public bool BypassCache { get; }
    }
}

