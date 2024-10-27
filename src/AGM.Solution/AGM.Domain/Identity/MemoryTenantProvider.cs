using AGM.Domain.Abstractions;
using AGM.Domain.Entities;

namespace AGM.Domain.Identity
{
    public class MemoryTenantProvider : ITenantProvider
    {
        private Tenant Tenant { get; set; }
        public Tenant GetCurrentTenant() => Tenant;


        public void SetActiveTenant(Tenant tenant) => this.Tenant = tenant;

    }
}
