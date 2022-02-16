
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

Let us create a software system designed for a hotel administrator. This system should provide information about the hotel rooms, the clients who live in the hotel and the employees who clean the rooms.

Suppose that the number of rooms in the hotel is known, and there are three types of rooms: single, double and triple, which are different costs per day.

The application must store the following information about each guest: passport number, full name, the city from which he came, the date of settlement in the hotel, the allocated hotel room.

The application shall keep the following information about the hotel employee: surname, first name, patronymic, where (floor) and when (day of the week) they clean. The hotel employee will clean all the rooms on one floor on certain days of the week. At the same time, on different days, he can clean other floors.

Working with the system involves obtaining the following information:
1. About clients staying in a given room
2. About the clients arriving from a given city
3. About the employees who cleaned the room of the specified client on the specified day of the week
4.	Whether the hotel has vacancies and available rooms and, if so, how many and which rooms are available.

The administrator must be able to perform the following operations:
- Hire or fire an employee at the hotel
- Change the working hours of the employee
- Check-in or check out the client.

It should also be possible to issue a hotel bill to the client automatically and receive a report on the hotel's activity for a specified quarter of the current year.

Such a report should contain the following information: the number of clients for the specified period, how many days the client occupied each hotel room and vacant, and the total income.