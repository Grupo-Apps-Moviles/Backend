using System.Net.Mime;
using Backend_Frock.Favorites.Domain.Model.Commands;
using Backend_Frock.Favorites.Domain.Model.Queries;
using Backend_Frock.Favorites.Domain.Services;
using Backend_Frock.Favorites.Interfaces.REST.Resources;
using Backend_Frock.Favorites.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Frock.Favorites.Interfaces.REST;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class FavoriteRoutesController : ControllerBase
{
    private readonly IFavoriteRouteCommandService _favoriteRouteCommandService;
    private readonly IFavoriteRouteQueryService _favoriteRouteQueryService;

    public FavoriteRoutesController(
        IFavoriteRouteCommandService favoriteRouteCommandService,
        IFavoriteRouteQueryService favoriteRouteQueryService)
    {
        _favoriteRouteCommandService = favoriteRouteCommandService;
        _favoriteRouteQueryService = favoriteRouteQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFavoriteRoute([FromBody] CreateFavoriteRouteResource resource)
    {
        var command = CreateFavoriteRouteCommandFromResourceAssembler.ToCommandFromResource(resource);
        var favoriteRoute = await _favoriteRouteCommandService.Handle(command);

        if (favoriteRoute is null) return BadRequest();

        var favoriteRouteResource = FavoriteRouteResourceFromEntityAssembler.ToResourceFromEntity(favoriteRoute);
        return CreatedAtAction(nameof(GetFavoriteRouteById), new { favoriteRouteId = favoriteRouteResource.Id }, favoriteRouteResource);
    }

    [HttpGet("{favoriteRouteId}")]
    public async Task<IActionResult> GetFavoriteRouteById(int favoriteRouteId)
    {
        var query = new GetFavoriteRouteByIdQuery(favoriteRouteId);
        var favoriteRoute = await _favoriteRouteQueryService.Handle(query);

        if (favoriteRoute is null) return NotFound();

        var favoriteRouteResource = FavoriteRouteResourceFromEntityAssembler.ToResourceFromEntity(favoriteRoute);
        return Ok(favoriteRouteResource);
    }

    [HttpGet("passenger/{passengerId}")]
    public async Task<IActionResult> GetAllFavoriteRoutesByPassengerId(int passengerId)
    {
        var query = new GetAllFavoriteRoutesByPassengerIdQuery(passengerId);
        var favoriteRoutes = await _favoriteRouteQueryService.Handle(query);

        var favoriteRouteResources = favoriteRoutes.Select(FavoriteRouteResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(favoriteRouteResources);
    }

    [HttpDelete("{favoriteRouteId}")]
    public async Task<IActionResult> DeleteFavoriteRoute(int favoriteRouteId)
    {
        var command = new DeleteFavoriteRouteCommand(favoriteRouteId);
        var result = await _favoriteRouteCommandService.Handle(command);

        if (!result) return NotFound();

        return NoContent();
    }
}