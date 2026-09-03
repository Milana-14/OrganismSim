using OrganismSim.Core;

namespace OrganismSim.PlayerActions;

public sealed class GiveWater : IPlayerAction
{
    public string Name => "Дай вода";

    public ActionResult Execute(Patient patient)
    {
        double consciousness = patient.Physiology.Get(ParameterType.Consciousness);

        double impairment = Math.Clamp((70 - consciousness) / 70.0, 0, 1);
        double chokeProbability = consciousness <= 2 ? 1.0 : Math.Pow(impairment, 1.5); // balance

        if (Random.Shared.NextDouble() < chokeProbability)
        {
            patient.Pathology.Adjust(ConditionType.AirwayObstruction, chokeProbability * 6); // balance
            return new ActionResult(ActionOutcome.Complication, "Бабата се задавя с водата.");
        }

        patient.Pathology.Adjust(ConditionType.Dehydration, -3);
        return new ActionResult(ActionOutcome.Success, "Дадохте вода.");
    }
}

public sealed class GiveJuice : IPlayerAction
{
    public string Name => "Дай 150 мл сок";

    public ActionResult Execute(Patient patient)
    {
        double consciousness = patient.Physiology.Get(ParameterType.Consciousness);

        double impairment = Math.Clamp((70 - consciousness) / 70.0, 0, 1);
        double chokeProbability = consciousness <= 2 ? 1.0 : Math.Pow(impairment, 1.5); // balance

        if (Random.Shared.NextDouble() < chokeProbability)
        {
            patient.Pathology.Adjust(ConditionType.AirwayObstruction, chokeProbability * 6); // balance
            return new ActionResult(ActionOutcome.Complication, "Бабата се задавя със сока.");
        }

        patient.Physiology.Adjust(ParameterType.BloodGlucoseLevel, 6); // balance
        patient.Absorptions.Enqueue(ParameterType.BloodGlucoseLevel, 32, 30); // balance
        
        return new ActionResult(ActionOutcome.Success, "Дадохте 150 мл сок.");
    }
}

public sealed class GiveBiscuit : IPlayerAction
{
    public string Name => "Дай бисквитка";

    public ActionResult Execute(Patient patient)
    {
        double consciousness = patient.Physiology.Get(ParameterType.Consciousness);

        double impairment = Math.Clamp((70 - consciousness) / 70.0, 0, 1);
        double chokeProbability = consciousness <= 2 ? 1.0 : Math.Pow(impairment, 1.5); // balance

        if (Random.Shared.NextDouble() < chokeProbability)
        {
            patient.Pathology.Adjust(ConditionType.AirwayObstruction, chokeProbability * 6); // balance
            return new ActionResult(ActionOutcome.Complication, "Бабата се задавя с бисквитката.");
        }

        patient.Absorptions.Enqueue(ParameterType.BloodGlucoseLevel, 34, 60); // balance
        return new ActionResult(ActionOutcome.Success, "Дадохте бисквитка.");
    }
}