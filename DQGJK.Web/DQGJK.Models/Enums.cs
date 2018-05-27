namespace DQGJK.Models
{
    public enum Status
    {
        enable = 1,
        disable = 0
    }

    public enum AreaLevelType
    {
        Province = 1,
        City = 2,
        Country = 3
    }

    public enum OperateState
    {
        Error = -1,
        Before = 0,
        Sended = 1,
        Done = 2
    }

    public enum ExceptionType
    {
        offline = 0,
        humidity = 1,
        temperature = 2
    }
}
