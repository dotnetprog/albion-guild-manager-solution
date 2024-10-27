// See https://aka.ms/new-console-template for more information
using AGM.Database;
using AGM.Database.Console.Commands;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using AGM.Domain.Identity;
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = CoconaApp.CreateBuilder();

builder.Services
    .ConfigureAGMDatabase(builder.Configuration)
    .AddScoped<ITenantProvider, MemoryTenantProvider>((sp) =>
    {
        var tp = new MemoryTenantProvider();
        tp.SetActiveTenant(new Tenant { Id = TenantId.Empty });
        return tp;
    })
    .AddLogging((builder) =>
            builder
            .AddFilter("Microsoft.EntityFrameworkCore.Database", LogLevel.Error)
            .AddConsole());

var app = builder.Build();
app.AddCommands<DatabaseCommands>();
await app.RunAsync();