using System.Security.Cryptography;
using OrganismSim.Core;

namespace OrganismSim.Diagnoses;

public class InternalBleedingProfile : IDiagnosisProfile
{
    public DiagnosisType Type => DiagnosisType.InternalBleeding;
    public string DisplayName => "Вътрешно кървене";

    public void Initialize(Patient patient)
    {
        patient.Pathology.SetSeverity(ConditionType.InternalBleeding, 7);
        patient.Physiology.Adjust(ParameterType.Pain, 5);
        patient.InternalBleedingType = InternalBleedingType.Hemothorax;
        //(InternalBleedingType)RandomNumberGenerator.GetInt32(Enum.GetValues<InternalBleedingType>().Length);
    }
}