namespace Application.Exceptions;


public sealed class RoomCrowdedException : BusinessException
{
    public RoomCrowdedException(string roomNumber, string clientPassport)
        : base(
            $"Room with number {roomNumber} is crowded, client with passport {clientPassport} cannot check-in in this room")
    {
        RoomNumber = roomNumber;
        ClientPassport = clientPassport;
    }


    public string RoomNumber { get; }
    public string ClientPassport { get; }
}
