using Zombris.Core;
using Zombris.GridSystem;

namespace Zombris.Entities.Behaviours;

public interface IActionStrategy
{
    void Act(Entity e, Grid g); 
    IActionStrategy Clone();
}

public class InfectStrategy(int range = 1) : IActionStrategy
{
    private readonly int range = range;

    public void Act(Entity e, Grid g)
    {
        var neigh = g.GetNeighbors(e.Position.X, e.Position.Y, range);
        foreach(var other in neigh)
        {
            ActorEntity actor = (ActorEntity)other;
            if (actor != null && actor.Type != GameConfig.ActorEntityType.Zombie)
                actor.ChangeActorType(GameConfig.ActorEntityType.Zombie, (ActorEntity)e);    
        }
    }

    public IActionStrategy Clone() => new InfectStrategy();
    
}