namespace Backend_Frock.Subscriptions.Interfaces.REST.Resources;

public record CreateSubscriptionResource(string ApprovalUrl);
 
public record SubscriptionStatusResource(
    bool IsActive,
    string Status,
    DateTime? ExpiresAt
);