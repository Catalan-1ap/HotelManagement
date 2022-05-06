using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.CleanerEntity.CreateCleaner;
using Application.Features.CleanerEntity.RemoveCleaner;
using Application.Features.CleaningScheduleEntity.CreateSchedule;
using Application.Features.CleaningScheduleEntity.RemoveSchedule;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;
using Wpf.Dtos;
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
    public BindableCollection<CleanerWithNotifier> Cleaners { get; } = new();
    public CleanerWithNotifier? SelectedCleaner { get; set; }
    public CleaningScheduleWithNotifier? SelectedSchedule { get; set; }
    public bool CanRemove => SelectedCleaner is not null;
    public bool CanSchedule => SelectedCleaner is not null;
    public bool CanUnschedule => SelectedSchedule is not null;


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

        Cleaners.Add(newCleaner.Adapt<CleanerWithNotifier>());
    }


    public async Task Remove()
    {
        var request = new RemoveCleanerCommand(SelectedCleaner!.Id);

        await _mediator.Send(request);

        Cleaners.Remove(SelectedCleaner);
        SelectedCleaner = null;
    }


    public async Task Schedule()
    {
        var viewModel = new CreateScheduleViewModel(SelectedCleaner!.Adapt<Cleaner>());
        var dialogResult = this.ShowDialog(viewModel);

        if (dialogResult == false)
            return;

        var request = new CreateScheduleCommand(
            viewModel.SelectedFloor!.Number,
            SelectedCleaner!.Id,
            (Weekday)viewModel.SelectedWeekday!
        );
        var schedule = await _mediator.Send(request);

        SelectedCleaner.Workdays.Add(schedule.Adapt<CleaningScheduleWithNotifier>());
    }


    public async Task Unschedule()
    {
        var cleaner = SelectedSchedule!.Cleaner!;

        var request = new RemoveScheduleCommand(
            SelectedSchedule!.FloorId,
            cleaner.Id,
            SelectedSchedule.Weekday
        );
        await _mediator.Send(request);

        cleaner.Workdays.Remove(SelectedSchedule);
        UpdateCleaner(cleaner);
        SelectedSchedule = null;
    }


    private void UpdateCleaner(CleanerWithNotifier toUpdate)
    {
        var index = Cleaners.IndexOf(Cleaners.Single(c => c.Id == toUpdate.Id));

        Cleaners[index] = toUpdate;
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
                    Cleaners.AddRange(task.Result.Adapt<List<CleanerWithNotifier>>());
                })
        };
}
