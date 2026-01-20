using System;

namespace CruscottoIncidenti.Application.Common.Exceptions
{
    public class DublicatedEntityException : Exception
    {
        public DublicatedEntityException() : base() {}

        public DublicatedEntityException(string friendlyMessage) : base() 
            => FriendlyMessage = friendlyMessage;

        public DublicatedEntityException(string name, object key)
           : base($"Entity \"{name}\" ({key}) already exists.") => Name = name;

        public string FriendlyMessage { get; private set; }

        public string Name { get; private set; }
    }
}
