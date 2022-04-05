using System;
using MediatR;


namespace Application.CQRS.DateReport;


public sealed record DateReportQuery(DateTime From, DateTime To) : IRequest<DateReportResponse>;