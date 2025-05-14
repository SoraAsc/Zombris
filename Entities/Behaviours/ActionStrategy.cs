using Zombris.Core;
using Zombris.Entities.Factory;
using Zombris.GridSystem;

namespace Zombris.Entities.Behaviours;

public interface IActionStrategy 
{ 
    void Act(Entity e, Grid g); 
}

public class InfectStrategy(int range = 1) : IActionStrategy
{
    private readonly int range = range;

    public void Act(Entity e, Grid g)
    {
        var neigh = g.GetNeighbors(e.Position.X, e.Position.Y, range);
        foreach(var other in neigh)
        {
            if (other != null && !other.IsAZombie)
            {
                // conversão em ZombiePrime
                int px = other.Position.X, py = other.Position.Y;
                var z = FactoryManager.Create(GameConfig.ZombieType.ZombiePrime, px, py);
                g.Place(z, px, py);
            }
        }
    }
}