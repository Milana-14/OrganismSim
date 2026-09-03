using OrganismSim.Core;

namespace OrganismSim.Simulation;

public static class DisplayNames
{ 
    
    public static readonly Dictionary<SymptomType, string> Symptoms = new()
{
    [SymptomType.Hunger] = "Глад",
    [SymptomType.Sweating] = "Изпотяване",
    [SymptomType.Tremor] = "Треперене",
    [SymptomType.Salivation] = "Усилено слюноотделяне",
    [SymptomType.Palpitations] = "Сърцебиене",
    [SymptomType.RapidWeakPulse] = "Учестен и слаб пулс",
    [SymptomType.Panic] = "Паника",
    [SymptomType.Anxiety] = "Тревожност",
    [SymptomType.Restlessness] = "Безпокойство",
    [SymptomType.Agitation] = "Възбуда",
    [SymptomType.Paleness] = "Бледост",
    [SymptomType.Mydriasis] = "Разширени зеници",
    [SymptomType.Nausea] = "Гадене",
    [SymptomType.Vomiting] = "Повръщане",
    [SymptomType.Weakness] = "Слабост",
    [SymptomType.Headache] = "Главоболие",
    [SymptomType.Dizziness] = "Замайване",
    [SymptomType.Drowsiness] = "Сънливост",
    [SymptomType.Paresthesia] = "Мравучкане",
    [SymptomType.NumbnessAroundMouth] = "Изтръпване около устата",
    [SymptomType.BlurredVision] = "Замъглено зрение",
    [SymptomType.DoubleVision] = "Двойно виждане",
    [SymptomType.DifficultyConcentrating] = "Нарушена концентрация",
    [SymptomType.Confusion] = "Обърканост",
    [SymptomType.Disorientation] = "Дезориентация",
    [SymptomType.SlurredSpeech] = "Неясен говор",
    [SymptomType.CoordinationProblems] = "Нарушена координация",
    [SymptomType.Apathy] = "Апатия",
    [SymptomType.Aggression] = "Агресивност",
    [SymptomType.ReducedPainResponse] = "Намалена реакция на болка",
    [SymptomType.ReducedTouchResponse] = "Намалена реакция на допир",
    [SymptomType.ReducedTemperatureResponse] = "Намалена реакция на температура",
    [SymptomType.LossOfSwallowingReflex] = "Изчезване на гълтателния рефлекс",
    [SymptomType.LossOfSpeech] = "Загуба на речта",
    [SymptomType.Unresponsive] = "Не реагира на външни стимули",
    [SymptomType.Stupor] = "Оцепенение",
    [SymptomType.MuscleToneLoss] = "Загуба на мускулен тонус",
    [SymptomType.LowBloodPressure] = "Ниско кръвно налягане",
    [SymptomType.SlowBreathing] = "Забавено дишане",
    [SymptomType.LeftArmWeakness] = "Не може да движи лявата ръка",
    [SymptomType.Pain] = "Болка",
    [SymptomType.ShortnessOfBreath] = "Задух"
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
    [ParameterType.Consciousness] = "Съзнание",
    [ParameterType.BrainFunction] = "Мозъчна функция",
    [ParameterType.Stress] = "Стрес",
    [ParameterType.Pain] = "Болка",
    [ParameterType.Comfort] = "Комфорт",
};

public static readonly Dictionary<ConditionType, string> Conditions = new()
{
    [ConditionType.ExternalBleeding] = "Външно кървене",
    [ConditionType.Hemothorax] = "Хемоторакс",
    [ConditionType.IntraAbdominalBleeding] = "Вътрешно коремно кървене",
    [ConditionType.PericardialTamponade] = "Перикардна тампонада",
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
    [DeathCause.HyperglycemicCrisis] = "хипергликемична криза",
    [DeathCause.Hypothermia] = "хипотермия",
    [DeathCause.Hyperthermia] = "хипертермия",
    [DeathCause.BrainDeath] = "мозъчна смърт",
};
}