using OrganismSim.Core;

namespace OrganismSim.Diagnoses;

public class AnaphylaxisProfile : IDiagnosisProfile
{
    public DiagnosisType Type => DiagnosisType.Anaphylaxis;
    public string DisplayName => "Анафилаксия";
    public void Initialize(Patient patient)
    {
        patient.Pathology.SetSeverity(ConditionType.Anaphylaxis, 7);
    }
}