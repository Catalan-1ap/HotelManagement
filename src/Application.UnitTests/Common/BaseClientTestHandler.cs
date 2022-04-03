using Domain.Entities;


namespace Application.UnitTests.Common;


public class BaseClientTestHandler : BaseTestHandler
{
    protected Client TestClient { get; }
    protected Room TestRoom { get; }


    protected BaseClientTestHandler()
    {
        TestClient = new()
        {
            City = "City",
            Passport = "Passport"
        };

        TestRoom = new()
        {
            Number = "42",
            RoomType = new()
            {
                MaxPeopleNumber = 1
            },
        };
    }
}
