using OrganismSim.Core;

namespace OrganismSim.Core;

public sealed class HypoperfusionBrainDamageEffect : IParameterThresholdEffect
{
    // Единен път за увреждане на мозъка - от недостатъчен кръвоток (инсулт,
    // шок, сърдечна недостатъчност) И от недостатъчен кислород (задушаване,
    // увреждане на бели дробове) - двата паралелни входа от diagram-а.
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double flowDeficit = Deficit(snapshot, ParameterType.CerebralBloodFlow);
        double oxygenDeficit = Deficit(snapshot, ParameterType.BloodOxygenLevel);

        double damage = seconds * (flowDeficit * 0.015 + oxygenDeficit * 0.03); // balance
        if (damage <= 0) return;

        patient.Physiology.Adjust(ParameterType.BrainFunction, -damage);
        patient.Physiology.Adjust(ParameterType.Consciousness, -damage);
    }

    private static double Deficit(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        return Math.Max(0, range.NormalMin - snapshot[type]);
    }
}

public sealed class ConsciousnessMetabolicEffect : IParameterThresholdEffect
{
    // Мозъкът е чувствителен и към две метаболитни отклонения, отделни от
    // кръвотока/кислорода: кръвна захар и телесна температура извън
    // нормата - и в двете посоки увреждат мозъчната функция.
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double damage = OutOfRangeSeverity(snapshot, ParameterType.BloodGlucoseLevel) * 0.02   // balance
                      + OutOfRangeSeverity(snapshot, ParameterType.BodyTemperature) * 0.3;      // balance
        if (damage <= 0) return;

        double delta = seconds * damage;
        patient.Physiology.Adjust(ParameterType.BrainFunction, -delta * 0.7); // balance
        patient.Physiology.Adjust(ParameterType.Consciousness, -delta); // balance
    }

    private static double OutOfRangeSeverity(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        double value = snapshot[type];
        if (value < range.NormalMin) return range.NormalMin - value;
        if (value > range.NormalMax) return value - range.NormalMax;
        return 0;
    }
}

public sealed class RespiratoryDriveEffect : IParameterThresholdEffect
{
    // Дихателният център е устойчив - потиска се едва при ТЕЖКА церебрална
    // дисфункция, не при лека. Прагът затова не е NormalMin (80), а много
    // по-нисък - около 30, представляващ близка до терминална увреда.
    private const double SevereThreshold = 30; // balance

    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double deficit = Math.Max(0, SevereThreshold - snapshot[ParameterType.BrainFunction]);
        if (deficit <= 0) return;

        patient.Physiology.Adjust(ParameterType.RespiratoryRate, seconds * -deficit * 0.1); // balance
    }
}

public sealed class GasExchangeEffect : IParameterThresholdEffect
{
    // CO2 не прескача мигновено до ново равновесие - тялото има голям
    // резервоар (кръв + тъкани, буферирани от бикарбонатната система),
    // затова реалното ниво се движи ПОСТЕПЕННО към целта, не веднага.
    // Кислородът остава derived (BloodOxygenLevelRule) - неговият резервоар
    // е много по-малък и реално де-сатурира бързо, за разлика от CO2.
    private const double ApproachRatePerSecond = 0.03; // balance

    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double targetCo2 = ComputeEquilibrium(patient, snapshot);
        double currentCo2 = snapshot[ParameterType.BloodCo2Level];

        patient.Physiology.Adjust(ParameterType.BloodCo2Level, (targetCo2 - currentCo2) * ApproachRatePerSecond * seconds);
    }

    private static double ComputeEquilibrium(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot)
    {
        const double referenceRate = 16; // balance
        const double referenceCo2 = 40;  // balance
        const double minRate = 1;

        double lungInjury = patient.Pathology.GetSeverity(ConditionType.LungInjury);
        double pulmonaryEdema = patient.Pathology.GetSeverity(ConditionType.PulmonaryEdema);

        // Десетократно по-малък принос от белодробна патология тук, отколкото
        // при кислорода (виж BloodOxygenLevelRule) - CO2 дифундира много
        // по-лесно през мембраната, затова увреждането ѝ бута основно O2.
        double exchangeEfficiency = 1
            - lungInjury / 10.0 * 0.15     // balance
            - pulmonaryEdema / 10.0 * 0.1; // balance
        exchangeEfficiency = Math.Clamp(exchangeEfficiency, 0.3, 1);

        double effectiveRate = Math.Max(minRate, snapshot[ParameterType.RespiratoryRate] * exchangeEfficiency);

        return referenceCo2 * referenceRate / effectiveRate;
    }
}

public sealed class HyperperfusionIcpEffect : IParameterThresholdEffect
{
    // Прекомерен мозъчен кръвоток повишава вътречерепното налягане - това
    // намалява CPP (виж CerebralBloodFlowRule) и връща потока обратно
    // надолу - самокоригиращ се кръг, не хардкоднат таван.
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        var range = ParameterRange.All[ParameterType.CerebralBloodFlow];
        double excess = Math.Max(0, snapshot[ParameterType.CerebralBloodFlow] - range.NormalMax);
        if (excess <= 0) return;

        patient.Physiology.Adjust(ParameterType.IntracranialPressure, seconds * excess * 0.05); // balance
    }
}

public sealed class PainStressEffect : IParameterThresholdEffect
{
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        var range = ParameterRange.All[ParameterType.Pain];
        double excess = Math.Max(0, snapshot[ParameterType.Pain] - range.NormalMax);
        if (excess <= 0) return;

        patient.Physiology.Adjust(ParameterType.Stress, seconds * excess * 0.3); // balance
    }
}

public sealed class AutonomicReflexEffect : IParameterThresholdEffect
{
    private const double ApproachRatePerSecond = 0.05; // balance - колко бързо severity приближава целта
    // Барорецепторен + хеморецепторен + метаболитен рефлекс, обединени в
    // едно място, защото в diagram-а ~8 различни параметъра се събират в
    // едни и същи 2 състояния (Sympathetic/Parasympathetic) - реалната
    // физиология също минава през общ автономен път, не 8 независими стрелки.
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        double drive = 0; // положително -> симпатикус, отрицателно -> парасимпатикус

        drive += LowDeficitRatio(snapshot, ParameterType.BloodPressure) * 0.3; // баро рефлекс
        drive += LowDeficitRatio(snapshot, ParameterType.BloodOxygenLevel) * 0.3; // хемо рефлекс
        drive += LowDeficitRatio(snapshot, ParameterType.BloodGlucoseLevel) * 0.2; // адренергичен отговор
        drive += LowDeficitRatio(snapshot, ParameterType.BodyTemperature) * 0.15; // треперене
        drive -= HighExcessRatio(snapshot, ParameterType.BloodPressure) * 0.3; // високо кръвно потиска симпатикус

        double targetSympathetic = Math.Clamp(drive, 0, 1) * 10; // balance
        double targetParasympathetic = Math.Clamp(-drive, 0, 1) * 10; // balance

        double currentSympathetic = patient.Pathology.GetSeverity(ConditionType.IncreasedSympatheticActivity);
        double currentParasympathetic = patient.Pathology.GetSeverity(ConditionType.IncreasedParasympatheticActivity);

        patient.Pathology.Adjust(ConditionType.IncreasedSympatheticActivity,
            (targetSympathetic - currentSympathetic) * ApproachRatePerSecond * seconds);
        patient.Pathology.Adjust(ConditionType.IncreasedParasympatheticActivity,
            (targetParasympathetic - currentParasympathetic) * ApproachRatePerSecond * seconds);
    }

    private static double LowDeficitRatio(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        double width = Math.Max(1, range.NormalMax - range.NormalMin);
        return Math.Max(0, (range.NormalMin - snapshot[type]) / width);
    }

    private static double HighExcessRatio(IReadOnlyDictionary<ParameterType, double> snapshot, ParameterType type)
    {
        var range = ParameterRange.All[type];
        double width = Math.Max(1, range.NormalMax - range.NormalMin);
        return Math.Max(0, (snapshot[type] - range.NormalMax) / width);
    }
}

public sealed class PulmonaryCapillaryPressureTrigger : IParameterThresholdEffect
{
    // Високо капилярно налягане в белите дробове -> течност излиза в
    // алвеолите -> задейства/задълбочава белодробен оток.
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        var range = ParameterRange.All[ParameterType.PulmonaryCapillaryPressure];
        double excess = Math.Max(0, snapshot[ParameterType.PulmonaryCapillaryPressure] - range.NormalMax);
        if (excess <= 0) return;

        patient.Pathology.Adjust(ConditionType.PulmonaryEdema, seconds * excess * 0.05); // balance
    }
}

public sealed class OsmoticDehydrationTrigger : IParameterThresholdEffect
{
    // Висока кръвна захар (осмотична диуреза) и висока температура
    // (изпотяване) водят до загуба на течности -> задейства/задълбочава
    // обезводняване.
    public void ApplyTick(Patient patient, IReadOnlyDictionary<ParameterType, double> snapshot, int seconds)
    {
        var glucoseRange = ParameterRange.All[ParameterType.BloodGlucoseLevel];
        var tempRange = ParameterRange.All[ParameterType.BodyTemperature];

        double glucoseExcess = Math.Max(0, snapshot[ParameterType.BloodGlucoseLevel] - glucoseRange.NormalMax);
        double tempExcess = Math.Max(0, snapshot[ParameterType.BodyTemperature] - tempRange.NormalMax);

        double delta = seconds * (glucoseExcess * 0.002 + tempExcess * 0.05); // balance
        if (delta <= 0) return;

        patient.Pathology.Adjust(ConditionType.Dehydration, delta);
    }
}
