using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class MainTabViewModel : TabScreen
{
    private readonly IReadOnlyApplicationDbContext _dbContext;


    public MainTabViewModel(IReadOnlyApplicationDbContext dbContext) : base("Главная вкладка")
    {
        _dbContext = dbContext;
        LoadingController = new(Loading());
    }


    public string Header { get; set; } = string.Empty;
    public BindableCollection<Room> Rooms { get; set; } = new();
    public LoadingController LoadingController { get; }


    private ICollection<Task> Loading() =>
        new List<Task>
        {
            _dbContext.Rooms
                .ToListAsync()
                .ContinueWith(task => Rooms.AddRange(task.Result)),
            Task
                .Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));

                    return "Header";
                })
                .ContinueWith(task => Header = task.Result)
        };
}
