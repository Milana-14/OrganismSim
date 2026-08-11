namespace OrganismSim.Core;

public sealed class SeizureRiskRule : IRiskRule
{
    public string Name => "Риск от гърч";
    public double ReferenceWindowSeconds => 60; // balance

    public double ComputeProbability(IReadOnlyDictionary<ParameterType, double> snapshot, PathologicalState pathology, Patient patient)
    {
        double brainDeficit = Deficit(snapshot, ParameterType.BrainFunction) / 100.0;
        double glucoseDeficit = Deficit(snapshot, ParameterType.BloodGlucoseLevel) / 50.0;
        double icpSeverity = pathology.GetSeverity(ConditionType.IntracranialProcess) / 10.0;

        double score = brainDeficit * 0.4 + glucoseDeficit * 0.3 + icpSeverity * 0.3; // balance
        return Math.Clamp(score, 0, 1);
    }

    public void Trigger(Patient patient)
    {
        patient.Pathology.SetSeverity(ConditionType.Seizure, 6); // balance (начална тежест на пристъпа)
    }

    private static double Deficit(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        return Math.Max(0, range.NormalMin - snapshot[type]);
    }
}

public sealed class ChokingRiskRule : IRiskRule
{
    public string Name => "Риск от задушаване";
    public double ReferenceWindowSeconds => 60; // balance

    public double ComputeProbability(IReadOnlyDictionary<ParameterType, double> snapshot, PathologicalState pathology, Patient patient)
    {
        bool unconscious = snapshot[ParameterType.Consciousness] <= 2;
        bool supine = patient.Posture == PatientPosture.Supine;

        return unconscious && supine ? 0.3 : 0; // balance
    }

    public void Trigger(Patient patient)
    {
        patient.Pathology.Adjust(ConditionType.AirwayObstruction, 5); // balance
    }
} // ами ако пострадалият вече е започнал да се задушава, но играчът го премести в странично положение? 
// Май трябва да спре да се задушава... но това не сме го направили в кода


public sealed class LethalArrhythmiaRiskRule : IRiskRule
{
    public string Name => "Риск от животозастрашаваща аритмия";
    public double ReferenceWindowSeconds => 300; // balance

    public double ComputeProbability(IReadOnlyDictionary<ParameterType, double> snapshot, PathologicalState pathology, Patient patient)
    {
        double heartFailureSeverity = pathology.GetSeverity(ConditionType.HeartFailure);
        if (heartFailureSeverity <= 0) return 0;

        return Math.Clamp(heartFailureSeverity / 10.0 * 0.4, 0, 1); // balance
    }

    public void Trigger(Patient patient)
    {
        patient.Physiology.Set(ParameterType.HeartRate, 210); // balance
    }
}