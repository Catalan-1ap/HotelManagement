using System.Collections.Generic;


namespace Application.CQRS.DateReport;


public sealed record DateReportResponse(int ClientsCount, ICollection<DateReportRoomDetails> RoomsDetails);
