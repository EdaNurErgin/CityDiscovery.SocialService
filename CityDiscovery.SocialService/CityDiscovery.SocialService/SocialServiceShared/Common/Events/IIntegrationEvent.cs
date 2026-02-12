using System;

namespace CityDiscovery.SocialService.SocialServiceShared.Common.Events
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}