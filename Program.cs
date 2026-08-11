// I have no idea what to do rn
// Pls send help

using System.Security.Cryptography;
using OrganismSim.Diagnoses;
using OrganismSim.Simulation;

namespace OrganismSim
{
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- Симулация на организъм при спешни ситуации ---");
            Console.WriteLine();

            IReadOnlyList<IDiagnosisProfile> profiles = DiagnosisFactory.AllProfiles();

            Console.WriteLine("Избери диагноза за тестване (отговори с цифра):");
            for (int i = 0; i < profiles.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}. {profiles[i].DisplayName}");
            }
            Console.WriteLine($"\t{profiles.Count + 1}. Случайна диагноза");

            DiagnosisType chosenType;
            var input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= profiles.Count)
            {
                chosenType = profiles[choice - 1].Type;
            }
            else
            {
                int random = RandomNumberGenerator.GetInt32(0, profiles.Count - 1);
                chosenType = profiles[random].Type;
            }

            var patient = DiagnosisFactory.Create(chosenType, "Баба Стоянка");
            IDiagnosisProfile profile = DiagnosisFactory.GetProfile(chosenType);
            var logger = new ConsoleGameLogger();

            var engine = new SimulationEngine(patient, profile, logger);
            engine.Run();
        }
    }
}
