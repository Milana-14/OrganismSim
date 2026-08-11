using OrganismSim.Core;

namespace OrganismSim.Diagnoses;

public class BleedingProfile : IDiagnosisProfile
{
    public DiagnosisType Type => DiagnosisType.Bleeding;
    public string DisplayName => "Силно кървене";

    public void Initialize(Patient patient)
    {
        patient.Pathology.SetSeverity(ConditionType.ExternalBleeding, 7);
        patient.Physiology.Adjust(ParameterType.Pain, 5);
    }
}