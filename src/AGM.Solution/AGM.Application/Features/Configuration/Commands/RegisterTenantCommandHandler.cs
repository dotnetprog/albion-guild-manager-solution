using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.Configuration.Commands
{
    public class RegisterTenantCommandHandler : IRequestHandler<RegisterTenantCommand, Tenant>
    {
        private readonly ITenantRepository _tenantRepository;

        public RegisterTenantCommandHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Tenant> Handle(RegisterTenantCommand request, CancellationToken cancellationToken)
        {
            var existingTenant = await _tenantRepository.GetByDiscordId(request.DiscordServerId, cancellationToken);
            if (existingTenant != null)
            {

                existingTenant.Name = request.Name;
                await _tenantRepository.Update(existingTenant, cancellationToken);


                return existingTenant;
            }
            else
            {
                var tenant = new Tenant
                {
                    DiscordServerId = request.DiscordServerId,
                    Name = request.Name,
                };
                tenant.Id = await _tenantRepository.Create(tenant, cancellationToken);
                return tenant;
            }

        }
    }
}
