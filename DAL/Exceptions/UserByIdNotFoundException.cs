using System;

namespace DAL.Exceptions
{
    public sealed class UserByIdNotFoundException(int userId) 
        : NotFoundException($"User with ID: '{userId}' was not found.")
    {
    }
}
