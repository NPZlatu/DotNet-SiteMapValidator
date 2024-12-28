using System.Text.Json;

public static class Util
{

    public static void Dump(object obj)
    {
        Console.WriteLine(JsonSerializer.Serialize(obj));
    }

    public static void Dump(string str)
    {
        Console.WriteLine(str);
    }

}
