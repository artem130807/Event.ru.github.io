using EventRep.Application.Events.Commands.AssignExecutor;
using EventRep.Application.Events.Commands.CreateEvent;
using EventRep.Application.Events.Commands.CreateEventFeedback;
using EventRep.Application.Events.Queries.GetEventById;
using EventRep.Application.Events.Queries.GetEvents;
using EventRep.Domain.Contracts.Pagination;
using EventRep.Domain.Enums;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventRep.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetEventByIdQuery(id),
            cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : Failure(result, StatusCodes.Status404NotFound);
    }

    [HttpGet]
    public async Task<IActionResult> GetPage(
        [FromQuery] int pageNumber = PageParams.DefaultPageNumber,
        [FromQuery] int pageSize = PageParams.DefaultPageSize,
        [FromQuery] EventStatus? status = null,
        [FromQuery] Guid? customerId = null,
        [FromQuery] Guid? executorId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetEventsQuery(
            new PageParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            status,
            customerId,
            executorId);

        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : Failure(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateEventCommand(
            request.Name,
            request.CustomerId,
            request.TimeStart,
            request.TimeEnd,
            request.FullDate);

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(
                nameof(GetById),
                new { id = result.Value },
                new { id = result.Value })
            : Failure(result);
    }

    [HttpPost("{eventId:guid}/feedbacks")]
    public async Task<IActionResult> CreateFeedback(
        Guid eventId,
        CreateEventFeedbackRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateEventFeedbackCommand(eventId, request.ExecutorId),
            cancellationToken);

        return result.IsSuccess
            ? Created(
                $"/api/events/{eventId}/feedbacks/{result.Value}",
                new { id = result.Value })
            : Failure(result);
    }

    [HttpPut("{eventId:guid}/executor")]
    public async Task<IActionResult> AssignExecutor(
        Guid eventId,
        AssignExecutorRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new AssignExecutorCommand(eventId, request.ExecutorId),
            cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Failure(result);
    }

    private ObjectResult Failure(
        IResultBase result,
        int statusCode = StatusCodes.Status400BadRequest) =>
        Problem(
            statusCode: statusCode,
            title: "Операция не выполнена",
            detail: string.Join("; ", result.Errors.Select(error => error.Message)));
}

public sealed record CreateEventRequest(
    string Name,
    Guid CustomerId,
    TimeSpan TimeStart,
    TimeSpan TimeEnd,
    DateTime FullDate);

public sealed record CreateEventFeedbackRequest(Guid ExecutorId);

public sealed record AssignExecutorRequest(Guid ExecutorId);
