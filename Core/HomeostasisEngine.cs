namespace OrganismSim.Core;

public sealed class HomeostasisEngine
{
    public void ApplyTick(Patient patient, int seconds)
    {
        foreach (var parameterType in ParameterRange.All.Keys)
        {
            if (DerivedParameterRegistry.All.ContainsKey(parameterType)) continue;

            ParameterRange range = ParameterRange.All[parameterType];
            double value = patient.Physiology.Get(parameterType);

            double target;
            if (value < range.NormalMin) target = range.NormalMin;
            else if (value > range.NormalMax) target = range.NormalMax;
            else continue;

            double pull = (target - value) * RecoveryRateFor(parameterType) * seconds;
            patient.Physiology.Adjust(parameterType, pull);
        }
    }
    
    private static double RecoveryRateFor(ParameterType type)
    {
        return type == ParameterType.IntracranialPressure ? 0.05 : 0.01; // balance
    }
}