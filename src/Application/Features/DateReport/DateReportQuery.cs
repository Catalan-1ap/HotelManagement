using System;
using MediatR;


namespace Application.Features.DateReport;


public sealed record DateReportQuery(DateTime From, DateTime To) : IRequest<DateReportResponse>;
