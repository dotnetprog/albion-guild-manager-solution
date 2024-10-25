using AGM.Domain.Entities;

namespace AGM.Domain.Abstractions
{
    public interface ITenantProvider
    {
        Task<Tenant> GetCurrentTenant();
    }
}
