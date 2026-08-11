using System.Security.Cryptography;

namespace OrganismSim.Core;

public sealed class ExternalBleedingEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.ExternalBleeding;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BloodVolume, seconds * -severity * 0.074); // balance
    }
}

public sealed class HemothoraxEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Hemothorax;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BloodVolume, seconds * -severity * 0.25); // balance
        patient.Physiology.Adjust(ParameterType.PleuralBloodVolume, seconds * severity * 0.02); // balance
    }
}

public sealed class ImpairedCerebralBloodFlowEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.ImpairedCerebralBloodFlow;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        if (severity < 10)
        {
            patient.Pathology.Adjust(ConditionType.ImpairedCerebralBloodFlow, seconds * 0.003); // balance
        }
    }
}

public sealed class IntracranialProcessEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.IntracranialProcess;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.IntracranialPressure, seconds * severity * 0.4); // balance
    }
}

public sealed class AirwayObstructionEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.AirwayObstruction;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.RespiratoryRate, seconds * -severity * 0.2); // balance
        patient.Physiology.Adjust(ParameterType.Stress, seconds * severity * 0.2); // balance
    }
}

public sealed class LungInjuryEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.LungInjury;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BloodOxygenLevel, seconds * -severity * 0.15); // balance
        patient.Physiology.Adjust(ParameterType.BloodCo2Level, seconds * severity * 0.15); // balance
        patient.Physiology.Adjust(ParameterType.Stress, seconds * severity * 0.15); // balance
        patient.Physiology.Adjust(ParameterType.Pain, seconds * severity * 0.15); // balance
    }
}

public sealed class PulmonaryEdemaEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.PulmonaryEdema;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BloodOxygenLevel, seconds * -severity * 0.05); // balance
        patient.Physiology.Adjust(ParameterType.BloodCo2Level, seconds * severity * 0.05); // balance
    }
}

public sealed class HeartFailureEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.HeartFailure;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        if (severity < 10)
        {
            patient.Pathology.Adjust(ConditionType.HeartFailure, seconds * 0.008); // balance
        }
        
        patient.Physiology.Adjust(ParameterType.Pain, seconds * severity * 0.01); // balance

        if (patient.HeartFailureType == HeartFailureType.Left)
        {
            patient.Physiology.Adjust(ParameterType.PulmonaryCapillaryPressure, seconds * severity * 0.5); // balance
        }
    }
}

public sealed class ShockEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Shock;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.Stress, seconds * severity * 0.15); // balance

        var bpRange = ParameterRange.All[ParameterType.BloodPressure];
        bool decompensated = patient.Physiology.Get(ParameterType.BloodPressure) <= bpRange.LethalLow * 1.3; // balance - критично ниско въпреки компенсацията

        double direction = decompensated ? -1 : 1;
        patient.Physiology.Adjust(ParameterType.HeartRate, seconds * direction * severity * 0.15); // balance
        patient.Physiology.Adjust(ParameterType.RespiratoryRate, seconds * direction * severity * 0.15); // balance
    }
}

public sealed class HypoglycemiaEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Hypoglycemia;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BloodGlucoseLevel, seconds * -severity * 0.05); // balance
        patient.Physiology.Adjust(ParameterType.Stress, seconds * severity * 0.02); // balance
    }
}

public sealed class HyperglycemiaEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Hyperglycemia;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BloodGlucoseLevel, seconds * severity * 0.05); // balance
    }
}

public sealed class DehydrationEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Dehydration;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BloodVolume, seconds * -severity * 0.25); // balance
    }
}

public sealed class SeizureEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Seizure;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.Consciousness, seconds * -severity * 0.3); // balance
        patient.Physiology.Adjust(ParameterType.BloodOxygenLevel, seconds * -severity * 0.2); // balance
        patient.Pathology.Adjust(ConditionType.IncreasedSympatheticActivity, seconds * severity * 0.1); // balance

        patient.Pathology.Adjust(ConditionType.Seizure, seconds * -0.3); // balance (отшумяване)
    }
}

public sealed class IncreasedSympatheticActivityEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.IncreasedSympatheticActivity;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.HeartRate, seconds * severity * 0.25); // balance
        patient.Physiology.Adjust(ParameterType.PeripheralVascularResistance, seconds * severity * 0.25); // balance
        patient.Physiology.Adjust(ParameterType.RespiratoryRate, seconds * severity * 0.08); // balance
        patient.Physiology.Adjust(ParameterType.Stress, seconds * severity * 0.25); // balance
    }
}

public sealed class IncreasedParasympatheticActivityEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.IncreasedParasympatheticActivity;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.HeartRate, seconds * -severity * 0.25); // balance
        patient.Physiology.Adjust(ParameterType.PeripheralVascularResistance, seconds * -severity * 0.25); // balance
        patient.Physiology.Adjust(ParameterType.RespiratoryRate, seconds * -severity * 0.1); // balance
        patient.Physiology.Adjust(ParameterType.Stress, seconds * -severity * 0.25); // balance
    }
}






public sealed class HypothermiaEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Hypothermia;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BodyTemperature, seconds * -severity * 0.1); // balance
    }
}

public sealed class HyperthermiaEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Hyperthermia;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.BodyTemperature, seconds * severity * 0.1); // balance
    }
}

public sealed class MildAllergicReactionEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.MildAllergicReaction;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.Stress, seconds * severity * 0.1); // balance
        patient.Physiology.Adjust(ParameterType.Pain, seconds * severity * 0.1); // balance
    }
}

public sealed class AnaphylaxisEffect : IConditionEffect
{
    public ConditionType Type => ConditionType.Anaphylaxis;

    public void ApplyTick(Patient patient, int seconds, double severity)
    {
        patient.Physiology.Adjust(ParameterType.PeripheralVascularResistance, seconds * -severity * 0.3); // balance
        patient.Pathology.Adjust(ConditionType.AirwayObstruction, seconds * severity * 0.3); // balance
        patient.Physiology.Adjust(ParameterType.Stress, seconds * severity * 0.3); // balance
    }
}