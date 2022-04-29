using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Application.Features.ClientEntity.CheckInClient;
using Application.Features.ClientEntity.CheckOutClient;
using Application.Features.ClientEntity.CreateClient;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;
using Wpf.Extensions;


namespace Wpf.ViewModels;


public sealed class ManageClientsViewModel : TabScreen, ILoadable
{
    private readonly IReadOnlyApplicationDbContext _dbContext =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IReadOnlyApplicationDbContext>();

    private IMediator _mediator = null!;


    public ManageClientsViewModel() : base("Редактирование клиентов") { }

    public LoadingController LoadingController { get; set; } = null!;
    public BindableCollection<Client> Clients { get; set; } = new();
    public Client? SelectedClient { get; set; }
    public bool CanCheckIn => SelectedClient is not null && SelectedClient.IsCheckout;
    public bool CanCheckOut => SelectedClient is not null && SelectedClient.IsCheckout == false;


    public void Load()
    {
        _mediator = Bootstrapper.GlobalServiceProvider.GetRequiredService<IMediator>();

        LoadingController = LoadingController.StartNew(LoadTasks());
    }


    public async Task Create()
    {
        var (dialogResult, viewModel) = this.ShowDialog<CreateClientViewModel>();

        if (dialogResult == false)
            return;

        var command = new CreateClientCommand(
            viewModel.Passport,
            viewModel.FirstName,
            viewModel.SurName,
            viewModel.Patronymic
        );
        var newClient = await _mediator.Send(command);

        Clients.Add(newClient);
    }


    public async Task CheckIn()
    {
        var (dialogResult, viewModel) = this.ShowDialog<CheckInClientViewModel>();

        if (dialogResult == false)
            return;

        var command = new CheckInClientCommand(
            SelectedClient!.Passport!,
            viewModel.City,
            viewModel.SelectedRoom!.Number!
        );
        var updatedClient = await _mediator.Send(command);

        Clients.Remove(SelectedClient);
        Clients.Add(updatedClient);
    }


    public async Task CheckOut()
    {
        var command = new CheckOutClientCommand(SelectedClient!.Passport!);

        var report = await _mediator.Send(command);

        UpdateCheckOutedClients(report);

        ShowMessageBoxWithCheckOutDetails(report);
    }


    private static void ShowMessageBoxWithCheckOutDetails(CheckOutClientCommandResponse report)
    {
        var partyInfo = report.Party.Count switch
        {
            0 => string.Empty,
            _ => $"Cопровождающие:\n{string.Join("\n", report.Party.Select(c => $"\t{c.SurName} {c.FirstName} {c.Patronymic}"))}"
        };

        Bootstrapper.GlobalServiceProvider
            .GetRequiredService<IWindowManager>()
            .ShowMessageBox(
                messageBoxText: $@"
Записано на счет: {report.Payer.Passport}
Личные данные: {report.Payer.SurName} {report.Payer.FirstName} {report.Payer.Patronymic}

{partyInfo}


Проведено дней: {report.DaysNumber} 
Стоимость комнаты: {report.PricePerDay} 
К оплате: {report.TotalPrice}
",
                caption: $"Отчет по комнате {report.RoomNumber}",
                buttons: MessageBoxButton.OK,
                icon: MessageBoxImage.Information,
                textAlignment: TextAlignment.Justify);
    }


    private void UpdateCheckOutedClients(CheckOutClientCommandResponse report)
    {
        var toRemove = report.Party
            .Append(report.Payer)
            .ToHashSet();

        var toRemovePassports = toRemove
            .Select(client => client.Passport);

        var updatedClients = Clients
            .ExceptBy(toRemovePassports, client => client.Passport)
            .Concat(toRemove);

        Clients = new(updatedClients);
    }


    private IReadOnlyCollection<Task> LoadTasks() =>
        new[]
        {
            _dbContext
                .Clients
                .ToListAsync()
                .ContinueWith(task =>
                {
                    Clients.Clear();
                    Clients.AddRange(task.Result);
                })
        };
}
