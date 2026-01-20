namespace CruscottoIncidenti.Common
{
    public enum Role
    {
        Admin = 0,
        Operator = 1,
        User = 2
    }

    public enum RequestType
    {
        Incident = 0,
        Request = 1,
        Bug = 2,
        Security = 3,
        Maintenance = 4
    }
}
