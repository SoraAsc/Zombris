
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;
using Zombris.Entities.Behaviours;
using Zombris.GridSystem;

namespace Zombris.Entities;
public enum EntityType { Blue, Zombie }

public abstract class Entity(string id, Point Position, Color color)
{
    protected string id = id;
    public Point Position { get; protected set; } = Position;
    protected bool isRunning = true;

    private readonly Dictionary<Type, IEntityComponent> components = [];

    protected Thread moveThread;
    protected Color color = color;

    public void Start(Grid grid)
    {
        moveThread = new(() => Simulate(grid));
        moveThread.Start();
    }

    public void Stop()
    {
        isRunning = false;
        moveThread.Join();
    }

    public void ChangePosition(int x, int y)
    {
        Position = new Point(x, y);
        Position = new Point(
            MathHelper.Clamp(Position.X, 0, GameConfig.GridWidth - 1),
            MathHelper.Clamp(Position.Y, 0, GameConfig.GridHeight - 1)
        );
    }

    public void AddComponent<T>(T component) where T : IEntityComponent
    {
        components[typeof(T)] = component;
        component.Owner = this;
    }

    public T Get<T>() where T : class, IEntityComponent
    {
        components.TryGetValue(typeof(T), out var comp);
        return comp as T;
    }

    private void Simulate(Grid grid)
    {
        while (isRunning)
        {
            Get<MovementComponent>()?.Execute(grid);
            Thread.Sleep(200); // 200 ms, i need to customize later
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int cellSize = GameConfig.CellSize;
        int pieceSize = GameConfig.PieceSize;
        
        Texture2D texture = new(SceneManager.GraphicsDevice, 1, 1);
        texture.SetData([color]);

        int x = Position.X * cellSize;
        int y = Position.Y * cellSize;

        int offset = (cellSize - pieceSize) / 2;

        var rectangle = new Rectangle(x + offset, y + offset, pieceSize, pieceSize);
        spriteBatch.Draw(texture, rectangle, color);
    }
}