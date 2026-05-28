using System.Net.Mime;
using Backend_Frock.Reservations.Domain.Model.Queries;
using Backend_Frock.Reservations.Domain.Services;
using Backend_Frock.Reservations.Interfaces.REST.Resources;
using Backend_Frock.Reservations.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Frock.Reservations.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReservationsController : ControllerBase
{
    private readonly IReservationCommandService _reservationCommandService;
    private readonly IReservationQueryService _reservationQueryService;

    public ReservationsController(
        IReservationCommandService reservationCommandService, 
        IReservationQueryService reservationQueryService)
    {
        _reservationCommandService = reservationCommandService;
        _reservationQueryService = reservationQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationResource resource)
    {
        var command = CreateReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var reservation = await _reservationCommandService.Handle(command);

        if (reservation is null) return BadRequest();

        var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(reservation);
        return CreatedAtAction(nameof(GetReservationById), new { reservationId = reservationResource.Id }, reservationResource);
    }

    [HttpGet("{reservationId}")]
    public async Task<IActionResult> GetReservationById(int reservationId)
    {
        var getReservationByIdQuery = new GetReservationByIdQuery(reservationId);
        var reservation = await _reservationQueryService.Handle(getReservationByIdQuery);

        if (reservation is null) return NotFound();

        var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(reservation);
        return Ok(reservationResource);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllReservationsByUserId(int userId)
    {
        var getAllReservationsByUserIdQuery = new GetAllReservationsByUserIdQuery(userId);
        var reservations = await _reservationQueryService.Handle(getAllReservationsByUserIdQuery);

        var reservationResources = reservations.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reservationResources);
    }
}