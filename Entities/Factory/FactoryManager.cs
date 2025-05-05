using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Zombris.Core;
using Zombris.Entities.Behaviours;
using Zombris.GridSystem;

namespace Zombris.Entities.Factory;

public static class FactoryManager
{
    private static int nextId = 1;
    private static readonly Random rng = new();
    public static Entity Create(EntityType type, int x, int y)
    {
        Entity e = type.ToString().StartsWith("Blue")
            ? new Blue(type + "_" + nextId++, new Point(x, y))
            : new Zombie(type + "_" + nextId++, new Point(x, y));
        switch (type)
        {
            case EntityType.Blue:
                e.AddComponent(new MovementComponent(new RandomMoveStrategy()));
                break;
            case EntityType.Zombie:
                e.AddComponent(new MovementComponent(new RandomMoveStrategy()));
                break;
        }
        return e;
    }

    public static List<Entity> CreateRandomEntities(Grid grid, int numBlue, int numZombies)
    {
        var all = new List<Entity>();
        for (int i = 0; i < numBlue; i++)
        {
            var type = (EntityType)rng.Next(0, 1); // Blue types
            int x, y;
            do { 
                x = rng.Next(GameConfig.GridWidth); 
                y = rng.Next(GameConfig.GridHeight); 
            } while (!grid.TryPlaceCheck(x, y));
            var e = Create(type, x, y);
            grid.Place(e, x, y);
            all.Add(e);
        }
        for (int i = 0; i < numZombies; i++)
        {
            var type = (EntityType)rng.Next(1, 2); // Zombie types
            int x, y;
            do { 
                x = rng.Next(GameConfig.GridWidth); 
                y = rng.Next(GameConfig.GridHeight); 
            } while (!grid.TryPlaceCheck(x, y));
            var e = Create(type, x, y);
            grid.Place(e, x, y);
            all.Add(e);
        }
        return all;
    }
}