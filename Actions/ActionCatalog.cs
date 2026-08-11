namespace OrganismSim.Actions;

public static class ActionCatalog
{
    public static List<IPlayerAction> BuildDefault()
    {
        return new List<IPlayerAction>
        {
            new Wait1SecAction(),
            new Wait5SecAction(),
            new Wait10SecAction(),
            new Wait20SecAction(),
            new Wait1MinAction(),
        };
    }
}