using System.Collections.Generic;


namespace Application.Features.DateReport;


public sealed record DateReportResponse(int ClientsCount, ICollection<DateReportRoomDetails> RoomsDetails);
