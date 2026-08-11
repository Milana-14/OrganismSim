using OrganismSim.Core;

namespace OrganismSim.Diagnoses;

public class HypoglycemiaProfile : IDiagnosisProfile
{
    public DiagnosisType Type => DiagnosisType.Hypoglycemia;
    public string DisplayName => "Хипогликемия (ниска кръвна захар)";

    public void Initialize(Patient patient)
    {
        patient.Pathology.SetSeverity(ConditionType.Hypoglycemia, 6);
    }
}