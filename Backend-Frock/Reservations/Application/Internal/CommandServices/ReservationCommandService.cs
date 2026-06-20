using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Model.Commands;
using Backend_Frock.Reservations.Domain.Repositories;
using Backend_Frock.Reservations.Domain.Services;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Reservations.Application.Internal.CommandServices;

public class ReservationCommandService : IReservationCommandService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationCommandService(IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Reservation?> Handle(CreateReservationCommand command)
    {
        var reservation = new Reservation(
            command.UserId,
            command.RouteIds,
            command.Amount,
            command.PaypalTransactionId,
            command.DriverId
        );;
        
        try
        {
            await _reservationRepository.AddAsync(reservation);
            await _unitOfWork.CompleteAsync();
            return reservation;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the reservation: {e.Message}");
            return null;
        }
    }
}