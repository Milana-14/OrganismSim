using System.Runtime.InteropServices;
using OrganismSim.Core;

namespace OrganismSim.Diagnoses;

public static class DiagnosisFactory
{
    private static readonly Dictionary<DiagnosisType, IDiagnosisProfile> Profiles = new()
    {
        [DiagnosisType.Hypoglycemia] = new HypoglycemiaProfile(),
        [DiagnosisType.Bleeding] = new BleedingProfile(),
        [DiagnosisType.MyocardialInfarction] = new MyocardialInfarctionProfile(),
        [DiagnosisType.Stroke] = new StrokeProfile(),
        [DiagnosisType.Fall] = new FallProfile(),
    };
    public static IDiagnosisProfile GetProfile(DiagnosisType type) => Profiles[type];
    public static IReadOnlyList<IDiagnosisProfile> AllProfiles() => Profiles.Values.ToList();
    public static Patient Create(DiagnosisType type,  string patientName)
    {
        Patient patient = new Patient(patientName);
        Profiles[type].Initialize(patient);
        return patient;
    }
}