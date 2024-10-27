using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.Configuration.Commands
{
    public class UpdateContentEventSettingsCommandHandler : IRequestHandler<UpdateContentEventSettingsCommand, Tenant>
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ITenantRepository _tenantRepository;
        public UpdateContentEventSettingsCommandHandler(
            ITenantProvider tenantProvider,
            ITenantRepository tenantRepository)
        {
            _tenantProvider = tenantProvider;
            _tenantRepository = tenantRepository;
        }


        public async Task<Tenant> Handle(UpdateContentEventSettingsCommand request, CancellationToken cancellationToken)
        {
            var contextTenant = _tenantProvider.GetCurrentTenant();

            var currentTenant = await _tenantRepository.GetByDiscordId(contextTenant.DiscordServerId);
            currentTenant.ChannelEventDiscordId = request.ChannelEventDiscordId;
            currentTenant.EventMessageDiscordId = request.EventMessageDiscordId;


            return await _tenantRepository.Update(currentTenant, cancellationToken);

        }
    }
}
