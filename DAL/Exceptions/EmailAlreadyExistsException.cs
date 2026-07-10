using System;

namespace DAL.Exceptions
{
    public sealed class EmailAlreadyExistsException(string email) 
        : ConflictException($"A user with email '{email}' already exists.")
    {
    }
}
