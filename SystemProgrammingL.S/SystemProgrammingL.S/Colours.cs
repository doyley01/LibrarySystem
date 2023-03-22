
[Serializable]
public class Colours
{
    public void Confirmation(string message)
    {
      
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine(message);
        Console.ResetColor();
    }
    public void Answers(string answer)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(answer);
        Console.ResetColor();
    }

    public void AskQuestions(string question)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(question);  
        Console.ResetColor();
    }

    public void Titles(string title)
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    public void Error(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ResetColor();
    }
}