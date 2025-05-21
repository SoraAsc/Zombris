using System.Collections.Generic;
using System.Linq;
using Zombris.GridSystem;

namespace Zombris.Entities.Behaviours;

public interface IEntityComponent
{
    Entity Owner { get; set; }
    IEntityComponent Clone();
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

    public IEntityComponent Clone() => new MovementComponent(strategy.Clone());
}

public class BehaviorComponent(List<IActionStrategy> strats) : IEntityComponent
{
    public Entity Owner { get; set; }
    private readonly List<IActionStrategy> strategies = strats;

    public void Execute(Grid grid)
    {
        foreach (var strat in strategies) strat.Act(Owner, grid);
    }

    public IEntityComponent Clone()
    {
        // Cria uma nova lista de estratégias clonadas
        var clonedStrategies = strategies
            .Select(s => s.Clone())
            .ToList();

        return new BehaviorComponent(clonedStrategies);
    }
}