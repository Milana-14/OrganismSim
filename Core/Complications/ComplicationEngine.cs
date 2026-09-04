namespace OrganismSim.Core;

public class ComplicationEngine
{
    private readonly List<IComplicationRule> _rules = new()
    {
        new HypoxicBrainInjuryComplication(),
        new AspirationComplication(),
        new CardiacEventComplication(),
        new SeizureInjuryComplication(),
    };

    public IReadOnlyList<string> Evaluate(Patient patient) =>
        _rules.Where(r => r.Applies(patient)).Select(r => r.Description).ToList();
}