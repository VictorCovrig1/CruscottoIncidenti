using System;

namespace CruscottoIncidenti.Application.Common.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException() : base() {}

        public CustomException(string friendlyMessage) : base() 
            => FriendlyMessage = friendlyMessage;

        public string FriendlyMessage { get; private set; }
    }
}
