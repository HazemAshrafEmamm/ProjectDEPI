namespace DAL.Exceptions
{
    public sealed class InvalidCredentialsException()
        : UnauthorizedException("Invalid email or password.")
    {
    }
}
