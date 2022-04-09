using System.Runtime.CompilerServices;
using Fody;


// Visible for Tests
[assembly: InternalsVisibleTo("Application.UnitTests")]


// Weaving assembly and inject "Task.ConfigureAwait(false)"
[assembly: ConfigureAwait(continueOnCapturedContext: false)]
