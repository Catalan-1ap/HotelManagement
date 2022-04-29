using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class RoomReportsViewModel : TabScreen, ILoadable
{
    private readonly IReadOnlyApplicationDbContext _dbContext =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IReadOnlyApplicationDbContext>();


    public RoomReportsViewModel() : base("Отчеты по комнатам") { }


    public LoadingController LoadingController { get; set; } = null!;
    public BindableCollection<RoomReport> Reports { get; set; } = new();


    public void Load() => LoadingController = LoadingController.StartNew(LoadTasks());


    private IReadOnlyCollection<Task> LoadTasks() =>
        new[]
        {
            _dbContext
                .RoomReports
                .ToListAsync()
                .ContinueWith(task =>
                {
                    Reports.Clear();
                    Reports.AddRange(task.Result);
                })
        };
}
