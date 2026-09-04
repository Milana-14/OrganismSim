namespace OrganismSim.Core;

public sealed class HypoxicBrainInjuryComplication : IComplicationRule
{
    public string Description => "с трайно увреждане на паметта и концентрацията заради продължителния кислороден/глюкозен глад на мозъка";
    public bool Applies(Patient patient) => patient.Exposure.SecondsSevereBrainDeficit > 90; // balance
}

public sealed class AspirationComplication : IComplicationRule
{
    public string Description => "с аспирационна пневмония вследствие задавяне по време на оказването на помощ";
    public bool Applies(Patient patient) => patient.Exposure.ChokingEvents > 0;
}

public sealed class CardiacEventComplication : IComplicationRule
{
    public string Description => "прекарала лек инфаркт вследствие тежката хипогликемия";
    public bool Applies(Patient patient) => patient.Pathology.GetSeverity(ConditionType.HeartFailure) > 0;
}

public sealed class SeizureInjuryComplication : IComplicationRule
{
    public string Description => "с леко нараняване (прехапан език/навяхване) вследствие гърча";
    public bool Applies(Patient patient) => patient.Exposure.SeizureEvents > 0;
}