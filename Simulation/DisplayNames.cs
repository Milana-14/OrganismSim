using OrganismSim.Core;

namespace OrganismSim.Simulation;

public static class DisplayNames
{
        public static readonly Dictionary<SymptomType, string> Symptoms = new()
        {
            [SymptomType.Tremor] = "Треперене",
            [SymptomType.Sweating] = "Изпотяване",
            [SymptomType.Hunger] = "Глад",
            [SymptomType.Dizziness] = "Замайване",
            [SymptomType.Palpitations] = "Сърцебиене",
            [SymptomType.Confusion] = "Обърканост",
            [SymptomType.Drowsiness] = "Сънливост",
            [SymptomType.LossOfConsciousness] = "Загуба на съзнание",
            [SymptomType.LeftArmWeakness] = "Не може да движи лявата ръка",
            [SymptomType.Paleness] = "Бледост",
            [SymptomType.Panic] = "Паника",
            [SymptomType.SlurredSpeech] = "Неясен говор",
            [SymptomType.Pain] = "Болка",
            [SymptomType.ShortnessOfBreath] = "Задух",
            [SymptomType.RapidWeakPulse] = "Учестен и слаб пулс",
        };

public static readonly Dictionary<ParameterType, string> Parameters = new()
{
    [ParameterType.BloodVolume] = "Обем на кръвта",
    [ParameterType.BloodPressure] = "Кръвно налягане",
    [ParameterType.PeripheralVascularResistance] = "Периферно съдово съпротивление",
    [ParameterType.HeartRate] = "Сърдечна честота",
    [ParameterType.CerebralBloodFlow] = "Мозъчно кръвоснабдяване",
    [ParameterType.PeripheralOrganBloodFlow] = "Кръвоснабдяване на периферните органи",
    [ParameterType.IntracranialPressure] = "Вътречерепно налягане",
    [ParameterType.PleuralBloodVolume] = "Обем на кръв в плевралната кухина",
    [ParameterType.RespiratoryRate] = "Дихателна честота",
    [ParameterType.BloodOxygenLevel] = "Ниво на кислород в кръвта",
    [ParameterType.BloodCo2Level] = "Ниво на CO₂ в кръвта",
    [ParameterType.PulmonaryCapillaryPressure] = "Налягане в белодробните капиляри",
    [ParameterType.BloodGlucoseLevel] = "Ниво на кръвната захар",
    [ParameterType.BodyTemperature] = "Телесна температура",
    [ParameterType.Stress] = "Стрес",
    [ParameterType.Pain] = "Болка",
    [ParameterType.Consciousness] = "Съзнание",
    [ParameterType.BrainFunction] = "Мозъчна функция",
};

public static readonly Dictionary<ConditionType, string> Conditions = new()
{
    [ConditionType.ExternalBleeding] = "Външно кървене",
    [ConditionType.Hemothorax] = "Хемоторакс",
    [ConditionType.IntracranialProcess] = "Вътречерепен процес",
    [ConditionType.AirwayObstruction] = "Запушване на дихателните пътища",
    [ConditionType.LungInjury] = "Увреждане на белите дробове",
    [ConditionType.PulmonaryEdema] = "Белодробен оток",
    [ConditionType.HeartFailure] = "Сърдечна недостатъчност",
    [ConditionType.ImpairedCerebralBloodFlow] = "Нарушено мозъчно кръвоснабдяване",
    [ConditionType.Shock] = "Шок",
    [ConditionType.Hypoglycemia] = "Хипогликемия",
    [ConditionType.Hyperglycemia] = "Хипергликемия",
    [ConditionType.Dehydration] = "Обезводняване",
    [ConditionType.Seizure] = "Гърч",
    [ConditionType.IncreasedSympatheticActivity] = "Увеличена симпатикова активност",
    [ConditionType.IncreasedParasympatheticActivity] = "Увеличена парасимпатикова активност",
    [ConditionType.Hypothermia] = "Хипотермия",
    [ConditionType.Hyperthermia] = "Хипертермия",
    [ConditionType.MildAllergicReaction] = "Лека алергична реакция",
    [ConditionType.Anaphylaxis] = "Анафилаксия",
    [ConditionType.Fracture111] = "Счупване"
};

public static readonly Dictionary<DeathCause, string> DeathCauses = new()
{
    [DeathCause.None] = "-",

    [DeathCause.BloodLoss] = "масивна загуба на кръв",
    [DeathCause.CirculatoryCollapse] = "циркулаторен колапс",
    [DeathCause.HypertensiveCrisis] = "хипертонична криза",
    [DeathCause.CardiacArrest] = "сърдечен арест",
    [DeathCause.LethalArrhythmia] = "фатална аритмия",
    [DeathCause.CerebralIschemia] = "критично намалено мозъчно кръвоснабдяване",
    [DeathCause.OrganFailure] = "органна недостатъчност",
    [DeathCause.IntracranialPressure] = "критично вътречерепно налягане",
    [DeathCause.TensionHemothorax] = "тензионен хемоторакс",
    [DeathCause.RespiratoryArrest] = "спиране на дишането",
    [DeathCause.RespiratoryFailure] = "дихателна недостатъчност",
    [DeathCause.Hypoxia] = "тежка хипоксия",
    [DeathCause.HypocapnicShock] = "критично ниско ниво на CO₂",
    [DeathCause.HypercapnicRespiratoryFailure] = "тежка хиперкапния",
    [DeathCause.HypoglycemicCrisis] = "хипогликемична криза",
    [DeathCause.HyperglycemicCrisis] = "хипергликемична криза",
    [DeathCause.Hypothermia] = "хипотермия",
    [DeathCause.Hyperthermia] = "хипертермия",
    [DeathCause.BrainDeath] = "мозъчна смърт",
};
}