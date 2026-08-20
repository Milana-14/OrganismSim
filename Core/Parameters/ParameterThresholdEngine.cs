namespace OrganismSim.Core;

public sealed class ParameterThresholdEngine
{
    private readonly List<IParameterThresholdEffect> _effects = new()
    {
        new HypoperfusionBrainDamageEffect(),
        new SystemicHypoperfusionEffect(),
        new ConsciousnessMetabolicEffect(),
        new RespiratoryDriveEffect(),
        new HyperperfusionIcpEffect(),
        new ComfortDecayEffect(),
        new AutonomicReflexEffect(),
        new PulmonaryCapillaryPressureTrigger(),
        new OsmoticDehydrationTrigger(),
        new GasExchangeEffect(),
        new StressRegulationEffect(),
        new AirwayObstructionCapEffect(),
    };

    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        foreach (var effect in _effects)
        {
            effect.ApplyTick(patient, snapshot, seconds);
        }
    }
}