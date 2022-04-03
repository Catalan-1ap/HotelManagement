using Domain.Entities;
using Domain.Enums;


namespace Application.UnitTests.Common;


public class BaseCleanerTestHandler : BaseTestHandler
{
    protected Cleaner TestObject { get; }


    protected BaseCleanerTestHandler()
    {
        var cleaner = new Cleaner
        {
            Person = new()
            {
                FirstName = "firstname",
                SurName = "surname",
                Patronymic = "patronymic"
            }
        };

        var floor = new Floor { Number = 1 };
        cleaner.Workdays.Add(new()
        {
            Floor = floor,
            Cleaner = cleaner,
            Weekday = Weekday.Monday
        });
        cleaner.Workdays.Add(new()
        {
            Floor = floor,
            Cleaner = cleaner,
            Weekday = Weekday.Saturday
        });


        TestObject = cleaner;
    }
}
