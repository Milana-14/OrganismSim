namespace OrganismSim.Actions;

public interface IPlayerAction
{
    string Name { get; }
    int TimeCostSeconds { get; }
}