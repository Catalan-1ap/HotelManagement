namespace Application.Exceptions;


public sealed class ClientWithPassportAlreadyExistsException : BusinessException
{
    public ClientWithPassportAlreadyExistsException(string passport)
        : base($"Client with passport \"{passport}\" already exists") =>
        Passport = passport;


    public string Passport { get; }
}
