using Domain.Entities;
using Domain.Enums;
using PropertyChanged;


namespace Wpf.Dtos;


[AddINotifyPropertyChangedInterface]
public sealed class CleaningScheduleWithNotifier
{
    public Weekday Weekday { get; set; }

    public int FloorId { get; set; }
    public Floor? Floor { get; set; }

    public int CleanerId { get; set; }
    public CleanerWithNotifier? Cleaner { get; set; }
}
