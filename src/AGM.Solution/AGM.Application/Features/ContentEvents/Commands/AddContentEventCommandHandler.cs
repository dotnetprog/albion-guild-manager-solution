﻿using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Commands
{
    public class AddContentEventCommandHandler : IRequestHandler<AddContentEventCommand, ContentEvent>
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly IDiscordIdentityManager _discordIdentityManager;
        private readonly IContentEventRepository _contentEventRepository;

        public AddContentEventCommandHandler(
            ITenantProvider tenantProvider,
            IDiscordIdentityManager discordIdentityManager,
            IContentEventRepository contentEventRepository)
        {
            _tenantProvider = tenantProvider;
            _discordIdentityManager = discordIdentityManager;
            _contentEventRepository = contentEventRepository;
        }

        public async Task<ContentEvent> Handle(AddContentEventCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _discordIdentityManager.GetCurrentUser();
            var tenant = _tenantProvider.GetCurrentTenant();

            var existingContentEvents = await _contentEventRepository.GetAll(request.StartsOn.AddHours(-1), cancellationToken);
            if (existingContentEvents.Any(t => t.SubTypeId == request.SubTypeId && t.AlbionMapId == request.AlbionMapId))
            {
                throw new InvalidOperationException("Another similar was found in the database. The event you try to register seems to be a duplicate.");
            }

            var contentEvent = new ContentEvent
            {
                AlbionMapId = request.AlbionMapId,
                TypeId = request.TypeId,
                SubTypeId = request.SubTypeId,
                StartsOn = request.StartsOn,
                TenantId = tenant.Id,
                CreatedByDiscordId = currentUser.DiscordUserId,
                ModifiedByByDiscordId = currentUser.DiscordUserId,
            };
            contentEvent.Id = await _contentEventRepository.Create(contentEvent, cancellationToken);
            return contentEvent;
        }
    }
}
