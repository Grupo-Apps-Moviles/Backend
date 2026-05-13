using Backend_Frock.Suscriptions.Domain.Model.Aggregates;
using Backend_Frock.Suscriptions.Domain.Repositories;
using Backend_Frock.Suscriptions.Infrastructure.ExternalServices;

namespace Backend_Frock.Suscriptions.Application.Internal.CommandServices
{
    public class SubscriptionCommandService
    {
        private readonly ISubscriptionRepository _repository;
        private readonly PaypalService _paypalService;

        public SubscriptionCommandService(ISubscriptionRepository repository, PaypalService paypalService)
        {
            _repository = repository;
            _paypalService = paypalService;
        }

        public async Task<Subscription> Execute(CreateSubscriptionCommand command)
        {
            // 1. Obtener token de PayPal
            var token = await _paypalService.GetAccessToken();

            // 2. Lógica para registrar en MySQL (Estado Inicial)
            var subscription = new Subscription(command.UserId, command.PaypalId);
            await _repository.AddAsync(subscription);

            return subscription;
        }
    }
}
