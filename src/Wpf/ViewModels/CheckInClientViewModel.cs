using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;
using Wpf.Dtos;


namespace Wpf.ViewModels;


public sealed class CheckInClientViewModel : InputScreenBase, ILoadable
{
    private readonly IReadOnlyApplicationDbContext _dbContext =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IReadOnlyApplicationDbContext>();


    public CheckInClientViewModel(IModelValidator<CheckInClientViewModel> validator)
    {
        Load();
        Validator = validator;
        Validate();
    }


    public string City { get; set; } = string.Empty;
    public BindableCollection<CheckInRoomDto> Rooms { get; set; } = new();
    public CheckInRoomDto? SelectedRoom { get; set; }
    public override bool CanAccept => base.CanAccept && SelectedRoom is not null;
    public LoadingController LoadingController { get; set; } = null!;


    public void Load() => LoadingController = LoadingController.StartNew(LoadTasks());


    private IReadOnlyCollection<Task> LoadTasks() =>
        new[]
        {
            _dbContext
                .Rooms
                .Where(room => room.RoomType!.MaxPeopleNumber - room.Clients.Count != 0)
                .Select(room => new CheckInRoomDto
                {
                    Number = room.Number!,
                    FloorNumber = room.FloorId, PricePerDay = room.RoomType!.PricePerDay,
                    Description = room.RoomType!.Description,
                    TotalPlaces = room.RoomType!.MaxPeopleNumber,
                    FreePlaces = room.RoomType!.MaxPeopleNumber - room.Clients.Count
                })
                .ToListAsync()
                .ContinueWith(task =>
                {
                    Rooms.Clear();
                    Rooms.AddRange(task.Result);
                })
        };
}
