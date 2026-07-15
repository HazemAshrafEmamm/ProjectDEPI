using System;

namespace DAL.Exceptions
{
    public sealed class ConversionBlockedException(string reason)
        : BusinessRuleException($"User type conversion blocked: {reason}")
    {
    }
}
