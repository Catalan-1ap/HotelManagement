using Domain.Enums;
using MediatR;


namespace Application.Features.CleaningScheduleEntity.RemoveSchedule;


public sealed record RemoveScheduleCommand(int FloorNumber, int CleanerId, Weekday Workday) : IRequest;
