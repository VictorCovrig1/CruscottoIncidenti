using System;
using CruscottoIncidenti.Application.Interfaces;

namespace CruscottoIncidenti.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
