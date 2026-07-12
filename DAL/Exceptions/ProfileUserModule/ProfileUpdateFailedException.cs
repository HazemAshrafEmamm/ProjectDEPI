using System;
using System.Collections.Generic;

namespace DAL.Exceptions.ProfileUserModule
{
    public sealed class ProfileUpdateFailedException(List<string> errors) : Exception("Profile update failed.")
    {
        public List<string> Errors { get; } = errors;
    }
}
