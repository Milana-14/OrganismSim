namespace OrganismSim.Core;

public sealed class ParameterThresholdEngine
{
    private readonly List<IParameterThresholdEffect> _effects = new()
    {
        new HypoperfusionBrainDamageEffect(),
        new ConsciousnessMetabolicEffect(),
        new RespiratoryDriveEffect(),
        new HyperperfusionIcpEffect(),
        new StressRegulationEffect(),
        new AutonomicReflexEffect(),
        new PulmonaryCapillaryPressureTrigger(),
        new OsmoticDehydrationTrigger(),
        new GasExchangeEffect(),
    };

    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        foreach (var effect in _effects)
        {
            effect.ApplyTick(patient, snapshot, seconds);
        }
    }
}