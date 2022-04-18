using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyChanged;


namespace Wpf.Common;


[AddINotifyPropertyChangedInterface]
public class LoadingController
{
    public LoadingController(ICollection<Task> loadingTasks) => Task.WhenAll(loadingTasks).ContinueWith(_ => IsLoading = false);

    public bool IsLoading { get; set; } = true;
    public bool IsLoaded => !IsLoading;
}
