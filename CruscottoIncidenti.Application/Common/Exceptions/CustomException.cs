using System;

namespace CruscottoIncidenti.Application.Common.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException() : base() {}

        public CustomException(string friendlyMessage) : base() 
            => FriendlyMessage = friendlyMessage;

        public CustomException(string name, object key)
           : base($"Entity \"{name}\" ({key}) already exists.") => Name = name;

        public string FriendlyMessage { get; private set; }

        public string Name { get; private set; }
    }
}
