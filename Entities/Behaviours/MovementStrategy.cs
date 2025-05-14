using System;
using System.Collections.Generic;
using Zombris.Core;
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
        var opts = new (int dx, int dy)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var (dx, dy) = opts[rng.Next(opts.Length)]; 
        return (e.Position.X + dx, e.Position.Y + dy);
    }
}

// Persegue o Blue mais próximo
public class ZombiePrimeChaseStrategy : IMovementStrategy
{
    public (int, int) NextPosition(Entity e, Grid g)
    {
        // busca o Blue mais próximo
        Entity target = null;
        int bestDist = int.MaxValue;
        for(int i = 0; i < GameConfig.GridWidth; i++) for(int j = 0; j < GameConfig.GridHeight; j++)
        {
            var other = g.GetEntityAt(i, j);
            if (other != null && !other.IsAZombie)
            {
                int d = Math.Abs(e.Position.X - i) + Math.Abs(e.Position.Y - j);
                if (d < bestDist) { bestDist = d; target = other; }
            }
        }
        if (target != null)
        {
            int dx = Math.Sign(target.Position.X - e.Position.X);
            int dy = Math.Sign(target.Position.Y - e.Position.Y);
            // tenta mover na direção horizontal primeiro
            if (dx != 0 && g.CanMove(e.Position.X + dx, e.Position.Y)) return (e.Position.X + dx, e.Position.Y);
            if (dy != 0 && g.CanMove(e.Position.X, e.Position.Y + dy)) return (e.Position.X, e.Position.Y + dy);
        }
        // fallback aleatório
        return new RandomMoveStrategy().NextPosition(e, g);
    }
}

// Tenta avançar à direita evitando zumbis, caso possível
public class BlueSmartMoveStrategy(int range = 1) : IMovementStrategy
{
    public (int, int) NextPosition(Entity e, Grid g)
    {
        int ux = e.Position.X, uy = e.Position.Y;
        // posições candidatas em ordem:
        var candidates = new List<(int x,int y)>{ (ux + 1, uy), (ux + 1, uy - 1), (ux + 1, uy + 1) };
        foreach(var (nx, ny) in candidates)
        {
            if (Grid.IsInside(nx, ny) && g.CanMove(nx, ny))
            {
                // evita se houver Zombie adjacente à célula de destino
                bool safe = true;
                foreach(var adj in g.GetNeighbors(nx, ny, range)) if (adj != null && adj.IsAZombie) { safe = false; break; }
                if (safe) return (nx, ny);
            }
        }
        // se não achou caminho seguro, afasta-se do zumbi mais próximo
        Entity nearest = null; int bestDist = int.MaxValue;
        for(int i = 0; i < GameConfig.GridWidth; i++) for(int j = 0; j < GameConfig.GridHeight; j++)
        {
            var other = g.GetEntityAt(i, j);
            if (other != null && other.IsAZombie)
            {
                int d = Math.Abs(ux - i) + Math.Abs(uy - j);
                if (d < bestDist) { bestDist = d; nearest = other; }
            }
        }
        if (nearest != null)
        {
            int dx = Math.Sign(ux - nearest.Position.X);
            int dy = Math.Sign(uy - nearest.Position.Y);
            int tx = ux + dx, ty = uy + dy;
            if (Grid.IsInside(tx, ty) && g.CanMove(tx, ty)) return (tx, ty);
        }
        return new RandomMoveStrategy().NextPosition(e, g); // fallback aleatório
    }
}