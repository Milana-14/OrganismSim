namespace OrganismSim.Simulation;

public class ConsoleGameLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[Log] {message}");
    }

    public void LogParameter(string type, double value)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"[Parameter] {type}: {value}");
        Console.ResetColor();
    }
    
    public void LogCondition(string type, double severity)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"[Condition] {type}: {severity}");
        Console.ResetColor();
    }

    public void LogSymptomAppeared(string symptomName)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[Log] Нов симптом: {symptomName}");
        Console.ResetColor();
    }

    public void LogSymptomResolved(string symptomName)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"[Log] Симптомът отшумя: {symptomName}");
        Console.ResetColor();
    }

    public void LogDeath(string patientName, string cause)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Log] {patientName} е починал/а. Причина: {cause}");
        Console.ResetColor();
    }

    public void LogStabilized(string patientName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[Log] {patientName} е стабилизиран/а.");
        Console.ResetColor();
    }
}