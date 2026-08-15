using OrganismSim.Core;

namespace OrganismSim.Core;

public sealed class HypoperfusionBrainDamageEffect : IParameterThresholdEffect
{
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double flowDeficit = Deficit(snapshot, ParameterType.CerebralBloodFlow);
        double oxygenDeficit = Deficit(snapshot, ParameterType.BloodOxygenLevel);

        double damage = seconds * (flowDeficit * 0.017 + oxygenDeficit * 0.03); // balance
        if (damage <= 0) return;

        patient.Physiology.Adjust(ParameterType.BrainFunction, -damage);
        patient.Physiology.Adjust(ParameterType.Consciousness, -damage);
    }

    private static double Deficit(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        return Math.Max(0, range.NormalMin - snapshot[type]);
    }
}

public sealed class ConsciousnessMetabolicEffect : IParameterThresholdEffect
{
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double damage = GlucoseDamage(snapshot) + TemperatureDamage(snapshot);
        if (damage <= 0) return;

        double delta = seconds * damage;
        patient.Physiology.Adjust(ParameterType.BrainFunction, -delta * 0.7);
        patient.Physiology.Adjust(ParameterType.Consciousness, -delta);
    }

    private static double GlucoseDamage(IReadOnlyDictionary<ParameterType, double> snapshot)
    {
        var range = ParameterRange.All[ParameterType.BloodGlucoseLevel];
        double value = snapshot[ParameterType.BloodGlucoseLevel];

        if (value < range.NormalMin) return (range.NormalMin - value) * 0.02; // balance

        if (value > range.NormalMax)
        {
            double excess = value - range.NormalMax;
            double lethalSpan = range.LethalHigh - range.NormalMax;
            double normalized = excess / lethalSpan;
            return normalized * normalized * 0.7; // balance
        }

        return 0;
    }

    private static double TemperatureDamage(IReadOnlyDictionary<ParameterType, double> snapshot)
    {
        var range = ParameterRange.All[ParameterType.BodyTemperature];
        double value = snapshot[ParameterType.BodyTemperature];
        
        if (value < range.NormalMin) return (range.NormalMin - value) * 0.3; // balance
        if (value > range.NormalMax) return (value - range.NormalMax) * 0.3; // balance
        return 0;
    }
}

public sealed class RespiratoryDriveEffect : IParameterThresholdEffect
{
    private const double SevereThreshold = 15; // balance

    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double deficit = Math.Max(0, SevereThreshold - snapshot[ParameterType.BrainFunction]);
        if (deficit <= 0) return;

        patient.Physiology.Adjust(ParameterType.RespiratoryRate, seconds * -deficit * 0.1); // balance
    }
}

public sealed class GasExchangeEffect : IParameterThresholdEffect
{
    private const double ApproachRatePerSecond = 0.03; // balance

    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double targetCo2 = ComputeEquilibrium(patient, snapshot);
        double currentCo2 = snapshot[ParameterType.BloodCo2Level];

        patient.Physiology.Adjust(ParameterType.BloodCo2Level, (targetCo2 - currentCo2) * ApproachRatePerSecond * seconds);
    }

    private static double ComputeEquilibrium(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot)
    {
        const double referenceRate = 16; // balance
        const double referenceCo2 = 40;  // balance
        const double minRate = 1;
        const double maxTargetCo2 = 100; // balance

        double lungInjury = patient.Pathology.GetSeverity(ConditionType.LungInjury);
        double pulmonaryEdema = patient.Pathology.GetSeverity(ConditionType.PulmonaryEdema);

        double exchangeEfficiency = 1
            - lungInjury / 10.0 * 0.15     // balance
            - pulmonaryEdema / 10.0 * 0.1; // balance
        exchangeEfficiency = Math.Clamp(exchangeEfficiency, 0.3, 1);

        double effectiveRate = Math.Max(minRate, snapshot[ParameterType.RespiratoryRate] * exchangeEfficiency);

        return Math.Min(maxTargetCo2, referenceCo2 * referenceRate / effectiveRate);
    }
}

public sealed class HyperperfusionIcpEffect : IParameterThresholdEffect
{
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        var range = ParameterRange.All[ParameterType.CerebralBloodFlow];
        double excess = Math.Max(0, snapshot[ParameterType.CerebralBloodFlow] - range.NormalMax);
        if (excess <= 0) return;

        patient.Physiology.Adjust(ParameterType.IntracranialPressure, seconds * excess * 0.05); // balance
    }
}

public sealed class StressRegulationEffect : IParameterThresholdEffect
{
    private const double ApproachRatePerSecond = 0.05; // balance

    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double target = ComputeTarget(patient, snapshot);
        double current = snapshot[ParameterType.Stress];

        patient.Physiology.Adjust(ParameterType.Stress, (target - current) * ApproachRatePerSecond * seconds);
    }

    private static double ComputeTarget(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot)
    {
        const double baseline = 1; // balance
        double target = baseline;

        target += patient.Pathology.GetSeverity(ConditionType.AirwayObstruction) * 0.2; // balance
        target += patient.Pathology.GetSeverity(ConditionType.LungInjury) * 0.15; // balance
        target += patient.Pathology.GetSeverity(ConditionType.Shock) * 0.15; // balance
        target += patient.Pathology.GetSeverity(ConditionType.Hypoglycemia) * 0.1; // balance
        target += patient.Pathology.GetSeverity(ConditionType.Anaphylaxis) * 0.3; // balance
        target += patient.Pathology.GetSeverity(ConditionType.MildAllergicReaction) * 0.1; // balance
        target += patient.Pathology.GetSeverity(ConditionType.IncreasedSympatheticActivity) * 0.25; // balance
        target -= patient.Pathology.GetSeverity(ConditionType.IncreasedParasympatheticActivity) * 0.25; // balance

        double painExcess = Math.Max(0, snapshot[ParameterType.Pain] - ParameterRange.All[ParameterType.Pain].NormalMax);
        target += painExcess * 0.3; // balance

        return Math.Clamp(target, 0, 10);
    }
}

public sealed class AutonomicReflexEffect : IParameterThresholdEffect
{
    private const double ApproachRatePerSecond = 0.05; // balance
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double drive = 0; // положително: симпатикус, отрицателно: парасимпатикус

        drive += LowDeficitRatio(snapshot, ParameterType.BloodPressure) * 0.3; // баро рефлекс
        drive += LowDeficitRatio(snapshot, ParameterType.BloodOxygenLevel) * 0.3; // хемо рефлекс
        drive += LowDeficitRatio(snapshot, ParameterType.BloodGlucoseLevel) * 0.2; // адренергичен отговор
        drive += LowDeficitRatio(snapshot, ParameterType.BodyTemperature) * 0.15; // треперене
        drive -= HighExcessRatio(snapshot, ParameterType.BloodPressure) * 0.3; // високо кръвно потиска симпатикус

        double targetSympathetic = Math.Clamp(drive, 0, 1) * 10; // balance
        double targetParasympathetic = Math.Clamp(-drive, 0, 1) * 10; // balance

        double currentSympathetic = patient.Pathology.GetSeverity(ConditionType.IncreasedSympatheticActivity);
        double currentParasympathetic = patient.Pathology.GetSeverity(ConditionType.IncreasedParasympatheticActivity);

        patient.Pathology.Adjust(ConditionType.IncreasedSympatheticActivity,
            (targetSympathetic - currentSympathetic) * ApproachRatePerSecond * seconds);
        patient.Pathology.Adjust(ConditionType.IncreasedParasympatheticActivity,
            (targetParasympathetic - currentParasympathetic) * ApproachRatePerSecond * seconds);
    }

    private static double LowDeficitRatio(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        double width = Math.Max(1, range.NormalMax - range.NormalMin);
        return Math.Max(0, (range.NormalMin - snapshot[type]) / width);
    }

    private static double HighExcessRatio(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        double width = Math.Max(1, range.NormalMax - range.NormalMin);
        return Math.Max(0, (snapshot[type] - range.NormalMax) / width);
    }
}

public sealed class PulmonaryCapillaryPressureTrigger : IParameterThresholdEffect
{
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        var range = ParameterRange.All[ParameterType.PulmonaryCapillaryPressure];
        double excess = Math.Max(0, snapshot[ParameterType.PulmonaryCapillaryPressure] - range.NormalMax);
        if (excess <= 0) return;

        patient.Pathology.Adjust(ConditionType.PulmonaryEdema, seconds * excess * 0.05); // balance
    }
}

public sealed class OsmoticDehydrationTrigger : IParameterThresholdEffect
{
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        var glucoseRange = ParameterRange.All[ParameterType.BloodGlucoseLevel];
        var tempRange = ParameterRange.All[ParameterType.BodyTemperature];

        double glucoseExcess = Math.Max(0, snapshot[ParameterType.BloodGlucoseLevel] - glucoseRange.NormalMax);
        double tempExcess = Math.Max(0, snapshot[ParameterType.BodyTemperature] - tempRange.NormalMax);

        double delta = seconds * (glucoseExcess * 0.00002 + tempExcess * 0.002); // balance
        if (delta <= 0) return;

        patient.Pathology.Adjust(ConditionType.Dehydration, delta);
    }
}

public sealed class AirwayObstructionCapEffect : IParameterThresholdEffect
{
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double severity = patient.Pathology.GetSeverity(ConditionType.AirwayObstruction);
        if (severity <= 0) return;

        var range = ParameterRange.All[ParameterType.RespiratoryRate];
        double maxAchievableRate = range.NormalMax * (1 - severity / 10.0);
        double current = patient.Physiology.Get(ParameterType.RespiratoryRate);

        if (current > maxAchievableRate)
        {
            patient.Physiology.Set(ParameterType.RespiratoryRate, maxAchievableRate);
        }
    }
}
