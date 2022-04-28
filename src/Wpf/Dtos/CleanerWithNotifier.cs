using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PropertyChanged;
using Stylet;


namespace Wpf.Dtos;


[AddINotifyPropertyChangedInterface]
public class CleanerWithNotifier
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;

    public BindableCollection<CleaningScheduleWithNotifier> Workdays { get; set; } = new();
}