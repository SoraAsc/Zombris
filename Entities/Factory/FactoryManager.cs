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

    public static Entity Create(GameConfig.ZombieType type, int x, int y)
    {
        Entity e = new ActorEntity(type + "_" + nextId++, new Point(x, y), GameConfig.ActorEntityType.Zombie);
        switch (type)
        {
            case GameConfig.ZombieType.ZombiePrime:
                e.InitializeValues(1, 500);
                e.AddComponent(new MovementComponent(new ZombiePrimeChaseStrategy()));
                e.AddComponent(new BehaviorComponent([new InfectStrategy(1)]));
                break;
        }
        return e;
    }

    public static Entity Create(GameConfig.BlueType type, int x, int y)
    {
        Entity e = new ActorEntity(type + "_" + nextId++, new Point(x, y), GameConfig.ActorEntityType.Blue);
        switch (type)
        {
            case GameConfig.BlueType.BluePrime:
                e.InitializeValues(1, 500);
                e.AddComponent(new MovementComponent(new BlueSmartMoveStrategy()));
                break;
        }
        return e;
    }

    public static List<Entity> CreateRandomEntities(Grid grid, int numBlue, int numZombies)
    {
        var all = new List<Entity>();
        for (int i = 0; i < numBlue; i++)
        {
            int length = Enum.GetNames(typeof(GameConfig.BlueType)).Length;
            var type = (GameConfig.BlueType)rng.Next(0, length); // Blue types
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
            int length = Enum.GetNames(typeof(GameConfig.ZombieType)).Length;
            var type = (GameConfig.ZombieType)rng.Next(0, length); // Zombie types
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