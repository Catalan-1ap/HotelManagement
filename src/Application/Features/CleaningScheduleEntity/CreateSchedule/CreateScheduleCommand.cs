using Domain.Enums;
using MediatR;


namespace Application.Features.CleaningScheduleEntity.CreateSchedule;


public sealed record CreateScheduleCommand(int FloorNumber, int CleanerId, Weekday Workday) : IRequest;
