using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyChanged;


namespace Wpf.Common;


[AddINotifyPropertyChangedInterface]
public class LoadingController
{
    private LoadingController(IReadOnlyCollection<Task> loadingTasks) =>
        Task
            .WhenAll(loadingTasks)
            .ContinueWith(_ => IsLoading = false);


    public bool IsLoading { get; set; } = true;
    public bool IsLoaded => !IsLoading;


    public static LoadingController StartNew(IReadOnlyCollection<Task> loadingTasks) => new(loadingTasks);
}
