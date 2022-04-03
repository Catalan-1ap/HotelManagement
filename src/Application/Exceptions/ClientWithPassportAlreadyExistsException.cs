using System;


namespace Application.Exceptions;


public sealed class ClientWithPassportAlreadyExistsException : Exception
{
    public string Passport { get; }


    public ClientWithPassportAlreadyExistsException(string passport)
        : base($"Client with passport \"{passport}\" already exists")
    {
        Passport = passport;
    }
}