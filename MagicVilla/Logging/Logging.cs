namespace MagicVilla.Logging;

public class Logging: ILogging
{
    public void Log(string message, string type)
    {
        if (type=="error")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error -"+message);
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}