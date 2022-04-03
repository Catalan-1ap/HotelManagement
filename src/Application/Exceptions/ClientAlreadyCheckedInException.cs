using System;


namespace Application.Exceptions;


public sealed class ClientAlreadyCheckedInException : Exception
{
    public string Passport { get; }


    public ClientAlreadyCheckedInException(string passport)
        : base($"Client with passport \"{passport}\" already checked-in")
    {
        Passport = passport;
    }
}
