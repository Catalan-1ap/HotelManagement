using Domain.Entities;


namespace Application.UnitTests.Common;


public class BaseClientTestHandler : BaseTestHandler
{
    protected Client TestObject { get; }


    protected BaseClientTestHandler() 
    {
        var client = new Client
        {
            Person = new()
            {
                FirstName = "firstname",
                SurName = "surname",
                Patronymic = "patronymic"
            },
            City = "City",
            Passport = "Passport",
        };
        
        TestObject = client;
    }
}
