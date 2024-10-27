﻿using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Discord;

namespace AGM.DiscordBot.Factory
{
    public class WorkerScopedDiscordFactory : IScopedDiscordFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WorkerScopedDiscordFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IServiceScope> Create(IGuild Guild, IGuildUser User)
        {
            var guidid = Guild?.Id ?? default(ulong);
            var tenant = new Tenant
            {
                DiscordServerId = guidid,
                Name = Guild?.Name
            };

            var scope = _serviceScopeFactory.CreateScope();
            var tenantProvider = scope.ServiceProvider.GetService<ITenantProvider>();
            tenantProvider.SetActiveTenant(tenant);
            var IdentityManager = scope.ServiceProvider.GetService<IDiscordIdentityManager>() as SocketDiscordIdentityManager;
            var tenantRepo = scope.ServiceProvider.GetService<ITenantRepository>();
            var currentTenant = await tenantRepo.GetByDiscordId(guidid);
            if (currentTenant != null)
            {
                tenantProvider.SetActiveTenant(currentTenant);
            }
            IdentityManager.SetUser(User);
            return scope;
        }
    }
}