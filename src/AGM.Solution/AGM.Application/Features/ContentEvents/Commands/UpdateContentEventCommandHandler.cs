using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using AGM.Domain.Exceptions;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Commands
{
    public class UpdateContentEventCommandHandler : IRequestHandler<UpdateContentEventCommand>
    {
        private readonly IDiscordIdentityManager _discordIdentityManager;
        private readonly IContentEventRepository _contentEventRepository;
        public UpdateContentEventCommandHandler(IDiscordIdentityManager discordIdentityManager, IContentEventRepository contentEventRepository)
        {
            _discordIdentityManager = discordIdentityManager;
            _contentEventRepository = contentEventRepository;
        }


        public async Task Handle(UpdateContentEventCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _discordIdentityManager.GetCurrentUser();

            var existingContentEvent = await _contentEventRepository.GetById(request.ContentEventId);
            if (existingContentEvent == null)
            {
                throw new RecordNotFoundException<ContentEvent>(request.ContentEventId.Value);
            }

            existingContentEvent.StartsOn = request.StartsOn;
            existingContentEvent.AlbionMapId = request.AlbionMapId;
            existingContentEvent.TypeId = request.TypeId;
            existingContentEvent.SubTypeId = request.SubTypeId;
            existingContentEvent.ModifiedByByDiscordId = currentUser.DiscordUserId;

            await _contentEventRepository.Update(existingContentEvent, cancellationToken);
        }
    }
}
