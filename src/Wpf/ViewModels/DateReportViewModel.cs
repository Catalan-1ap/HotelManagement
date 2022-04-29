using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Markup;
using Application.Features.DateReport;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PropertyChanged;
using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class DateReportViewModel : TabScreen
{
    private readonly IReadOnlyApplicationDbContext _dbContext =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IReadOnlyApplicationDbContext>();

    private readonly IMediator _mediator =
        Bootstrapper.GlobalServiceProvider.GetRequiredService<IMediator>();


    public DateReportViewModel(IModelValidator<DateReportViewModel> validator) : base("Отчет по дате") => Validator = validator;

    public static XmlLanguage DatePickerLanguage => XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag);


    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool CanAccept => !HasErrors;
    public DateReportResponse? DateReportResponse { get; set; }


    public async Task Accept()
    {
        if (Validate() == false)
            return;

        var query = new DateReportQuery((DateTime)From!, (DateTime)To!);
        DateReportResponse = await _mediator.Send(query);
    }


    [SuppressPropertyChangedWarnings]
    protected override void OnValidationStateChanged(IEnumerable<string> changedProperties)
    {
        base.OnValidationStateChanged(changedProperties);

        NotifyOfPropertyChange(() => CanAccept);
    }
}
