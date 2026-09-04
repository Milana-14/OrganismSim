using OrganismSim.PlayerActions;
using OrganismSim.Core;
using OrganismSim.Diagnoses;

namespace OrganismSim.Simulation;

public sealed class SimulationEngine
{
    private const int AmbulanceEtaSeconds = 300;
    
    private readonly Patient _patient;
    private readonly IDiagnosisProfile _profile; 
    private readonly ConsoleGameLogger _logger;
    private readonly List<IWaitPlayerAction> _waitActions; 
    private readonly List<IPlayerAction> _actions;
    
    private bool _ambulanceCalled;
    private int _ambulanceEtaRemaining;
    private int _elapsedSeconds;

        public SimulationEngine(Patient patient, IDiagnosisProfile profile, ConsoleGameLogger logger)
        {
            _patient = patient;
            _profile = profile;
            _logger = logger;
            _waitActions = ActionCatalog.WaitBuildDefault();
            _actions = ActionCatalog.BuildDefault();
        }

        public void Run()
        {
            PrintIntro();

            SimulationOutcome? outcome = null;
            PrintStatus();
            while (outcome is null)
            {
                var choice = ReadMenuChoice();

                if (choice == -1)
                {
                    Console.WriteLine("Прекратяваш сценария.");
                    return;
                }

                if (choice == 0)
                {
                    outcome = HandleCallAmbulance();
                }
                else
                {
                    IWaitPlayerAction waitAction;
                    IPlayerAction action;
                    
                    if (choice <= _waitActions.Count)
                    {
                        waitAction = _waitActions[choice - 1];
                        outcome = AdvanceTime(waitAction.TimeCostSeconds);
                    }
                    else
                    {
                        action = _actions[choice - _waitActions.Count - 1];
                        action.Execute(_patient);
                    }
                    
                    Console.WriteLine();
                    PrintStatus();
                }
            }
            PrintFinalReport(outcome.Value);
        }
        
        private void PrintIntro()
        {
            Console.WriteLine("=======================================");
            Console.WriteLine($" Диагноза: {_profile.DisplayName}");
            Console.WriteLine("=======================================");
            Console.WriteLine();
        }
        
        private void PrintStatus()
        {
            Console.WriteLine();
            Console.WriteLine($"--- Изминало време: {_elapsedSeconds} сек ---");

            if (_patient.ActiveSymptoms.Count == 0)
            {
                Console.WriteLine("Не забелязваш видими симптоми в момента.");
            }
            else
            {
                Console.WriteLine("Видими симптоми: " + string.Join(", ", _patient.ActiveSymptoms.Select(s => DisplayNames.Symptoms[s])));
            }

            if (_ambulanceCalled)
            {
                Console.WriteLine($"Линейка пристига след ~{_ambulanceEtaRemaining} сек.");
            }

            Console.WriteLine();
            Console.WriteLine("Колко време изчакваш?");
            Console.WriteLine("  0. Обади се на 112 / линейка");
            for (int i = 0; i < _waitActions.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {_waitActions[i].Name}");
            }
            for (int i = 0; i < _actions.Count; i++)
            {
                Console.WriteLine($"  {i + _waitActions.Count + 1}. {_actions[i].Name}");
            }
            Console.WriteLine("  Q. Изход");
        }
        
        private int ReadMenuChoice()
        {
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "Q", StringComparison.OrdinalIgnoreCase))
                {
                    return -1;
                }

                if (int.TryParse(input, out int waitChoice) && waitChoice >= 0 && waitChoice <= _waitActions.Count)
                {
                    return waitChoice;
                }
                
                if (int.TryParse(input, out int choice) && choice >= _waitActions.Count && choice <= _waitActions.Count + _actions.Count)
                {
                    return choice;
                }

                Console.WriteLine("Невалиден избор, опитай отново.");
            }
        }

        private SimulationOutcome? HandleCallAmbulance()
        {
            if (_ambulanceCalled)
            {
                Console.WriteLine("Вече си извикал/а линейка. Изчакваш пристигането й.");
                return AdvanceTime(10);
            }

            _ambulanceCalled = true;
            _ambulanceEtaRemaining = AmbulanceEtaSeconds;
            _logger.Log($"Линейка е повикана. Очаквано време на пристигане: {AmbulanceEtaSeconds / 60} мин.");
            return AdvanceTime(20);
        }

        private SimulationOutcome? AdvanceTime(int seconds)
        {
            IReadOnlyDictionary<ParameterType, double> parameters = new Dictionary<ParameterType, double>();
            IReadOnlyDictionary<ConditionType, double> conditions = new Dictionary<ConditionType, double>();
            
            for (int i = 0; i < seconds; i++)
            {
                var (newSymptoms, resolvedSymptoms,
                    par, con) = _patient.Tick(1);
                _elapsedSeconds++;

                parameters = par;
                conditions = con;
                
                foreach (var s in newSymptoms) _logger.LogSymptomAppeared(DisplayNames.Symptoms[s]);
                foreach (var s in resolvedSymptoms) _logger.LogSymptomResolved(DisplayNames.Symptoms[s]);

                if (!_patient.IsAlive)
                {
                    _logger.LogDeath(_patient.Name, DisplayNames.DeathCauses[_patient.CauseOfDeath]);
                    return SimulationOutcome.Died;
                }

                if (_ambulanceCalled)
                {
                    _ambulanceEtaRemaining--;
                    if (_ambulanceEtaRemaining <= 0)
                    {
                        return SimulationOutcome.AmbulanceArrivedAlive;
                    }
                }

                if (_patient.IsStabilized() && !_ambulanceCalled)
                {
                    _logger.LogStabilized(_patient.Name);
                    return SimulationOutcome.Stabilized;
                }
            }
            
            foreach (var p in parameters) _logger.LogParameter(DisplayNames.Parameters[p.Key], p.Value);
            foreach (var c in conditions) _logger.LogCondition(DisplayNames.Conditions[c.Key], c.Value);

            return null;
        }

        private void PrintFinalReport(SimulationOutcome outcome)
        {
            Console.WriteLine();
            Console.WriteLine("=========== КРАЙ НА СЦЕНАРИЯ ===========");
            Console.WriteLine($"Резултат: {DescribeOutcome(outcome)}");
            Console.WriteLine($"Диагноза: {_profile.DisplayName}");
            Console.WriteLine($"Общо изминало време: {_elapsedSeconds} сек");
            Console.WriteLine();

            Console.WriteLine("Физиологични параметри:");
            foreach (var p in _patient.Physiology.Snapshot())
            {
                _logger.LogParameter(DisplayNames.Parameters[p.Key], p.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Патологични състояния:");
            foreach (var c in _patient.Pathology.Snapshot())
            {
                _logger.LogCondition(DisplayNames.Conditions[c.Key], c.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Симптоми при край:");
            if (_patient.ActiveSymptoms.Count == 0)
            {
                Console.WriteLine("  (няма)");
            }
            else
            {
                foreach (var s in _patient.ActiveSymptoms)
                {
                    Console.WriteLine($"  {DisplayNames.Symptoms[s]}");
                }
            }

            Console.WriteLine("=========================================");
        }

        private string DescribeOutcome(SimulationOutcome outcome)
        {
            if (outcome == SimulationOutcome.Died) return "Пострадалият е починал";
            else if (outcome == SimulationOutcome.Stabilized) return "Пострадалият е стабилизиран";
            else if (outcome == SimulationOutcome.AmbulanceArrivedAlive)
            {
                var complications = new ComplicationEngine().Evaluate(_patient); 
                return complications.Count == 0 
                    ? "Линейката пристигна, пострадалият е жив"
                    : $"Линейката пристигна, пострадалият е жив, но {string.Join(", ", complications)}";
            }
            return "Неизвестен";
        }
}