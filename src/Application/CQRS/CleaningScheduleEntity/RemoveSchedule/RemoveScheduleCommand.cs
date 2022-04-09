using Domain.Enums;
using MediatR;


namespace Application.CQRS.CleaningScheduleEntity.RemoveSchedule;


public sealed record RemoveScheduleCommand(int FloorNumber, int CleanerId, Weekday Workday) : IRequest;
