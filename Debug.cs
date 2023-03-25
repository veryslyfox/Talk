static class Error
{
    public static void Add(string error)
    {
        Errors.Add(error);
    }
    public static List<string> Errors = new List<string>();
    public static void Write()
    {
        foreach (var item in Errors)
        {
            Console.WriteLine(item);
        }
    }
    public static T IsNull<T>(T value, string error)
    {
        if (value == null)
        {
            Add(error);
        }
        return value;
    }
    public static T IsNull<T>(T value, string error, T defaultValue)
    {
        if (value == null)
        {
            Add(error);
            return defaultValue;
        }
        return value;
    }
}