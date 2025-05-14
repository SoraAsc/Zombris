using System.Collections.Generic;
using Zombris.GridSystem;

namespace Zombris.Entities.Behaviours;

public interface IEntityComponent
{
    Entity Owner {get; set;}
}

// Movement behaviour
public class MovementComponent(IMovementStrategy strat) : IEntityComponent
{
    public Entity Owner { get; set; }
    private readonly IMovementStrategy strategy = strat;

    public void Execute(Grid grid)
    {
        var (nx, ny) = strategy.NextPosition(Owner, grid);
        grid.TryMove(Owner, nx, ny);
    }
}

public class BehaviorComponent(List<IActionStrategy> strats) : IEntityComponent
{
    public Entity Owner { get; set; }
    private readonly List<IActionStrategy> strategies = strats;

    public void Execute(Grid grid)
    {
        foreach (var strat in strategies) strat.Act(Owner, grid);
    }
}