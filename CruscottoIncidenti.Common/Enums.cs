namespace CruscottoIncidenti.Common
{
    public enum Role
    {
        Administrator = 1,
        Operator = 2,
        User = 3
    }

    public enum RequestType
    {
        Incident = 1,
        Request = 2,
        Bug = 3,
        Security = 4,
        Maintenance = 5
    }

    public enum Urgency
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
}
