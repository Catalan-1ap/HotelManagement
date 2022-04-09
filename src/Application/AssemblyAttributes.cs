using Fody;


// Weaving assembly and inject "Task.ConfigureAwait(false)"
[assembly: ConfigureAwait(continueOnCapturedContext: false)]
