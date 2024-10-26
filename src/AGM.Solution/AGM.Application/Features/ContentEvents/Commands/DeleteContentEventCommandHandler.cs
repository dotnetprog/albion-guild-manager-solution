using AGM.Domain.Abstractions;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Commands
{
    public class DeleteContentEventCommandHandler : IRequestHandler<DeleteContentEventCommand>
    {
        private readonly IContentEventRepository _contentEventRepository;

        public DeleteContentEventCommandHandler(IContentEventRepository contentEventRepository)
        {
            _contentEventRepository = contentEventRepository;
        }

        public async Task Handle(DeleteContentEventCommand request, CancellationToken cancellationToken)
        {
            await _contentEventRepository.Delete(request.ContentEventId, cancellationToken);
        }
    }
}
