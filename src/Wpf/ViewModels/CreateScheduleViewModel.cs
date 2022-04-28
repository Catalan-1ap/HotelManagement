using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class CreateScheduleViewModel : InputScreenBase, ILoadable
{
    private readonly IReadOnlyApplicationDbContext _dbContext =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IReadOnlyApplicationDbContext>();

    private readonly IReadOnlyCollection<Weekday> _freeWeekdaysForCleaner;


    public CreateScheduleViewModel(Cleaner forWhom)
    {
        Load();
        _freeWeekdaysForCleaner = Enum
            .GetValues<Weekday>()
            .Except(forWhom.Workdays.Select(schedule => schedule.Weekday))
            .ToArray();
    }


    public LoadingController LoadingController { get; set; } = null!;
    public BindableCollection<Floor> Floors { get; set; } = new();
    public Floor? SelectedFloor { get; set; }
    public IEnumerable<Weekday> Weekdays => _freeWeekdaysForCleaner
        .Except(SelectedFloor?.Cleaners.Select(schedule => schedule.Weekday) ?? Array.Empty<Weekday>());
    public Weekday? SelectedWeekday { get; set; }
    public override bool CanAccept => base.CanAccept && SelectedFloor is not null && SelectedWeekday is not null;


    public void Load() => LoadingController = LoadingController.StartNew(LoadTasks());


    private IReadOnlyCollection<Task> LoadTasks() =>
        new[]
        {
            _dbContext
                .Floors
                .Include(floor => floor.Cleaners)
                .ThenInclude(schedule => schedule.Cleaner)
                .ToListAsync()
                .ContinueWith(task =>
                {
                    Floors.Clear();
                    Floors.AddRange(task.Result);
                })
        };
}
