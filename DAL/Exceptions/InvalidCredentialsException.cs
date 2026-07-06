namespace DAL.Exceptions
{
    public sealed class InvalidCredentialsException()
        : UnauthorizedAccessException("Invalid email or password.")
    {
    }
}
