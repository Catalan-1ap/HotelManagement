using System.Collections.Generic;
using Domain.Entities;
using MediatR;


namespace Application.CleanerCQRS.Commands.UpdateCleanerCommand;


public sealed record UpdateCleanerCommand(Cleaner Cleaner) : IRequest<Cleaner>;