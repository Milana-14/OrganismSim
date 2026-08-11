namespace OrganismSim.Core;

public sealed class TremorRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Tremor;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.BloodGlucoseLevel) < 65;
    }

    public sealed class SweatingRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Sweating;
        public bool IsActive(Patient p) =>
            p.Physiology.Get(ParameterType.BloodGlucoseLevel) < 60 || p.Pathology.GetSeverity(ConditionType.Shock) > 3;
    }

    public sealed class HungerRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Hunger;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.BloodGlucoseLevel) < 70;
    }

    public sealed class DizzinessRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Dizziness;
        public bool IsActive(Patient p) =>
            p.Physiology.Get(ParameterType.BloodPressure) < 90 || p.Physiology.Get(ParameterType.BloodOxygenLevel) < 92;
    }

    public sealed class PalpitationsRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Palpitations;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.HeartRate) > 110;
    }

    public sealed class ConfusionRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Confusion;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.Consciousness) < 70;
    }

    public sealed class DrowsinessRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Drowsiness;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.Consciousness) < 50;
    }

    public sealed class LossOfConsciousnessRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.LossOfConsciousness;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.Consciousness) < 20;
    }

    public sealed class LeftArmWeaknessRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.LeftArmWeakness;
        public bool IsActive(Patient p) => p.Pathology.GetSeverity(ConditionType.ImpairedCerebralBloodFlow) > 3;
    }

    public sealed class PalenessRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Paleness;
        public bool IsActive(Patient p) =>
            p.Physiology.Get(ParameterType.BloodVolume) < 85 || p.Pathology.GetSeverity(ConditionType.Shock) > 2;
    }

    public sealed class PanicRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Panic;
        public bool IsActive(Patient p) =>
            p.Physiology.Get(ParameterType.Pain) > 6 || p.Physiology.Get(ParameterType.BloodOxygenLevel) < 80;
    }

    public sealed class SlurredSpeechRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.SlurredSpeech;
        public bool IsActive(Patient p) =>
            p.Pathology.GetSeverity(ConditionType.ImpairedCerebralBloodFlow) > 2 || p.Physiology.Get(ParameterType.Consciousness) < 40;
    }

    public sealed class PainRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.Pain;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.Pain) > 2;
    }

    public sealed class ShortnessOfBreathRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.ShortnessOfBreath;
        public bool IsActive(Patient p) => p.Physiology.Get(ParameterType.BloodOxygenLevel) < 90;
    }

    public sealed class RapidWeakPulseRule : ISymptomRule
    {
        public SymptomType Type => SymptomType.RapidWeakPulse;
        public bool IsActive(Patient p) =>
            p.Physiology.Get(ParameterType.HeartRate) > 100 && p.Physiology.Get(ParameterType.BloodPressure) < 100;
    }