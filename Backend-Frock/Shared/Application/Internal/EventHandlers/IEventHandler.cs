using Backend_Frock.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace Backend_Frock.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}