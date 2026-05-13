namespace Backend_Frock.Suscriptions.Domain.Model.Commands;

public record CreateSubscriptionCommand(
    string UserId,
    string PlanId,
    string PaypalId
);