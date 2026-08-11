namespace OrganismSim.Actions;

public sealed class Wait1SecAction : IPlayerAction
{
    public string Name => "Изчакай 1 секунда";
    public int TimeCostSeconds => 1;
}

public sealed class Wait5SecAction : IPlayerAction
{
    public string Name => "Изчакай 5 секунди";
    public int TimeCostSeconds => 5;
}

public sealed class Wait10SecAction : IPlayerAction
{
    public string Name => "Изчакай 10 секунди";
    public int TimeCostSeconds => 10;
}

public sealed class Wait20SecAction : IPlayerAction
{
    public string Name => "Изчакай 20 секунди";
    public int TimeCostSeconds => 20;
}

public sealed class Wait1MinAction : IPlayerAction
{
    public string Name => "Изчакай 1 минута";
    public int TimeCostSeconds => 60;
}