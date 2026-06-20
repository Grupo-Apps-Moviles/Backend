using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.Favorites.Domain.Model.Commands;
using Backend_Frock.Favorites.Domain.Repositories;
using Backend_Frock.Favorites.Domain.Services;
using Backend_Frock.Shared.Domain.Repositories;

namespace Backend_Frock.Favorites.Application.Internal.CommandServices;

public class FavoriteRouteCommandService : IFavoriteRouteCommandService
{
    private readonly IFavoriteRouteRepository _favoriteRouteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FavoriteRouteCommandService(
        IFavoriteRouteRepository favoriteRouteRepository,
        IUnitOfWork unitOfWork)
    {
        _favoriteRouteRepository = favoriteRouteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FavoriteRoute?> Handle(CreateFavoriteRouteCommand command)
    {
        var existing = await _favoriteRouteRepository
            .FindByPassengerIdAndRouteIdAsync(command.PassengerId, command.RouteId);

        if (existing is not null) return null;

        var favoriteRoute = new FavoriteRoute(command.PassengerId, command.RouteId);

        try
        {
            await _favoriteRouteRepository.AddAsync(favoriteRoute);
            await _unitOfWork.CompleteAsync();
            return favoriteRoute;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the favorite route: {e.Message}");
            return null;
        }
    }

    public async Task<bool> Handle(DeleteFavoriteRouteCommand command)
    {
        var favoriteRoute = await _favoriteRouteRepository.FindByIdAsync(command.FavoriteRouteId);

        if (favoriteRoute is null) return false;

        try
        {
            _favoriteRouteRepository.Remove(favoriteRoute);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while deleting the favorite route: {e.Message}");
            return false;
        }
    }
}
