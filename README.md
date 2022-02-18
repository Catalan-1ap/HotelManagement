
# Course project on ".NET Framework"

Used Stack:
- [Clean Architecture](https://github.com/ardalis/CleanArchitecture)
- WPF
-  Entity Framework Core
-  MVVM Pattern
- Git, Github

Nugets:
- [SmartEnum](https://github.com/ardalis/SmartEnum)
- [Bogus](https://github.com/bchavez/Bogus)
- [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- [Stylet](https://github.com/canton7/Stylet)

## Description

Suppose you create a software system designed for a hotel administrator. This system should provide information about the hotel rooms, the clients who live in the hotel and the employees who clean the rooms.

Suppose the number of hotel rooms is known, they are single, double and triple ones. The price per day is different.


The application must store the following information about each guest: passport number, full name, the city they came from, the check-in date, the assigned hotel room.

The application is supposed to keep the following information about the hotel employee: surname, first name, patronymic, the floor and the week day they clean. The hotel employee will clean all the rooms on one floor on certain weekdays. At the same time, he can clean other floors on different days,.

Working with the system involves obtaining the following information:
- About clients staying in a particular room
- About the clients arriving from a certain city
- About the employees who cleaned the room of the specified client on the specified weekday.
- Whether the hotel has vacant rooms and, if so, which of them are available.

The administrator must be able to perform the following duties:
- Hire or fire an employee at the hotel
- Change the working hours of the employee
- Check in or check out the client.

It should also be possible to issue a hotel bill to the client automatically and receive a report on the hotel's performance for a specified quarter of the current year.

Such report should contain the following information: the number of clients for the specified period, how many days each hotel room was vacant or occupied and the total income.
