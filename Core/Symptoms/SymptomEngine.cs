using System.Data;
using OrganismSim.Core;

namespace OrganismSim.Core;

public sealed class SymptomEngine
{
    private readonly List<ISymptomRule> _rules = new()
    {
        new TremorRule(),
        new SweatingRule(),
        new HungerRule(),
        new DizzinessRule(),
        new PalpitationsRule(),
        new ConfusionRule(),
        new DrowsinessRule(),
        new LossOfConsciousnessRule(),
        new LeftArmWeaknessRule(),
        new PalenessRule(),
        new PanicRule(),
        new SlurredSpeechRule(),
        new PainRule(),
        new ShortnessOfBreathRule(),
        new RapidWeakPulseRule(),
    };

    public HashSet<SymptomType> Evaluate(Patient patient)
    {
        return _rules.Where(r => r.IsActive(patient)).Select(r => r.Type).ToHashSet();
    }
}