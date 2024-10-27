using AGM.Domain.Entities;

namespace AGM.Domain.Abstractions
{
    public interface ITenantProvider
    {
        Tenant GetCurrentTenant();
        void SetActiveTenant(Tenant tenant);
    }
}
