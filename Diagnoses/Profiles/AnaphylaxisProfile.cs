using System.Security.Cryptography;
using OrganismSim.Core;

namespace OrganismSim.Diagnoses;

public class AnaphylaxisProfile : IDiagnosisProfile
{
    public DiagnosisType Type => DiagnosisType.Anaphylaxis;
    public string DisplayName => "Анафилаксия";
    public void Initialize(Patient patient)
    {
        patient.Pathology.SetSeverity(ConditionType.Anaphylaxis, 7);
        patient.AnaphylaxisPhenotype = (AnaphylaxisPhenotype)RandomNumberGenerator.GetInt32(Enum.GetValues<AnaphylaxisPhenotype>().Length);
    }
}