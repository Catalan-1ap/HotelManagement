using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.CleanerEntity.CreateCleaner;
using Application.Features.CleanerEntity.RemoveCleaner;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;
using Wpf.Extensions;


namespace Wpf.ViewModels;


public sealed class ManageCleanersViewModel : TabScreen, ILoadable
{
    private readonly IReadOnlyApplicationDbContext _dbContext =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IReadOnlyApplicationDbContext>();

    private readonly IMediator _mediator =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IMediator>();


    public ManageCleanersViewModel() : base("Редактирование работников") { }

    public LoadingController LoadingController { get; set; } = null!;
    public BindableCollection<Cleaner> Cleaners { get; } = new();
    public Cleaner? SelectedCleaner { get; set; }
    public bool CanRemove => SelectedCleaner is not null;


    public void Load() => LoadingController = LoadingController.StartNew(LoadTasks());


    public async Task Add()
    {
        var (dialogResult, viewModel) = this.ShowDialog<CreateCleanerViewModel>();

        if (dialogResult == false)
            return;

        var command = new CreateCleanerCommand(
            viewModel.FirstName,
            viewModel.SurName,
            viewModel.Patronymic
        );
        var newCleaner = await _mediator.Send(command);

        Cleaners.Add(newCleaner);
    }


    public async Task Remove()
    {
        var request = new RemoveCleanerCommand(SelectedCleaner!.Id);

        await _mediator.Send(request);

        Cleaners.Remove(SelectedCleaner);
        SelectedCleaner = null;
    }


    private IReadOnlyCollection<Task> LoadTasks() =>
        new[]
        {
            _dbContext
                .Cleaners
                .Include(c => c.Workdays)
                .ToListAsync()
                .ContinueWith(task =>
                {
                    Cleaners.Clear();
                    Cleaners.AddRange(task.Result);
                })
        };
}
