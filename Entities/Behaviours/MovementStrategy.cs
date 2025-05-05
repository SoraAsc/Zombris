using System;
using Zombris.GridSystem;

namespace Zombris.Entities.Behaviours;

public interface IMovementStrategy 
{ 
    (int, int) NextPosition(Entity e, Grid g); 
}

public class RandomMoveStrategy : IMovementStrategy
{
    private static readonly Random rng = new();
    
    public (int, int) NextPosition(Entity e, Grid g)
    {
        var opts = new (int dx, int dy)[] { (0,1), (1,0), (0,-1), (-1,0) };
        var (dx, dy) = opts[rng.Next(opts.Length)]; 
        return (e.Position.X + dx, e.Position.Y + dy);
    }
}