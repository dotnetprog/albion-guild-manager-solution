﻿using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AGM.Application.Features.Logging.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            var requestGuid = Guid.NewGuid().ToString();
            var requestNameWithGuid = $"{requestName} [{requestGuid}]";
            _logger.LogInformation($"[START] {requestNameWithGuid}");
            var stopwatch = Stopwatch.StartNew();

            TResponse response;
            try
            {
                try
                {
                    _logger.LogDebug($"[PROPS] {requestNameWithGuid} {JsonConvert.SerializeObject(request)}");
                }
                catch (NotSupportedException)
                {
                    _logger.LogError($"[Serialization ERROR] {requestNameWithGuid} Could not serialize the request.");
                }

                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation(
                    $"[END] {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
            }



            return response;
        }
    }
}
