namespace Backend_Frock.Suscriptions.Interfaces.REST.Transform;

using Backend_Frock.Suscriptions.Domain.Model.Aggregates;

public static class SubscriptionResourceFromEntityAssembler
{
    public static object ToResourceFromEntity(Subscription entity)
    {
        return new
        {
            Id = entity.Id,
            User = entity.UserId,
            PaypalId = entity.PaypalSubscriptionId,
            CurrentStatus = entity.Status,
            DateCreated = entity.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }
}