using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.StorageContracts;
using Application.ValidationRules;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Application.CQRS.ClientEntity.CreateClient;


public sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public readonly IApplicationDbContext _dbContext;
    
    
    public CreateClientCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        
        RuleFor(c => c.Person)!
            .PersonMustNotBeNull();

        RuleFor(c => c.Person!.FirstName)
            !.FirstNameRule();

        RuleFor(c => c.Person!.SurName)
            !.SurNameRule();

        RuleFor(c => c.Person!.Patronymic)
            !.PatronymicRule();

        RuleFor(c => c.City)
            .NotEmpty().WithMessage($"{nameof(Client.City)} must not be empty")
            .MaximumLength(ClientStorageContract.CityMaxLength)
            .WithMessage(
                $"{nameof(Client.City)} maximum length is {ClientStorageContract.CityMaxLength}");

        RuleFor(c => c.Passport)
            .NotEmpty().WithMessage($"{nameof(Client.Passport)} must not be empty")
            .MaximumLength(ClientStorageContract.PassportMaxLength)
            .WithMessage(
                $"{nameof(Client.Passport)} maximum length is {ClientStorageContract.PassportMaxLength}")
            .MustAsync(DoesNotAlreadyExists).WithMessage("Client with this passport already exists");
    }


    private Task<bool> DoesNotAlreadyExists(string passport, CancellationToken token) =>
        _dbContext.Clients.AllAsync(c => c.Passport != passport, token);
}
