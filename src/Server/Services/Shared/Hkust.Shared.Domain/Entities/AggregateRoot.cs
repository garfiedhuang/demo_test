﻿using Hkust.Infra.Core.DependencyInjection;
using Hkust.Infra.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace Hkust.Shared.Domain.Entities;

public abstract class AggregateRoot : DomainEntity, IConcurrency, IEfEntity<long>
{
    public byte[] RowVersion { get; set; }

    public Lazy<IEventPublisher> EventPublisher => new(() =>
    {
        var httpContext = InfraHelper.Accessor.GetCurrentHttpContext();
        if (httpContext is not null)
            return httpContext.RequestServices.GetRequiredService<IEventPublisher>();
        if (ServiceLocator.Provider is not null)
            return ServiceLocator.Provider.GetRequiredService<IEventPublisher>();
        throw new NotImplementedException(nameof(IEventPublisher));
    });
}