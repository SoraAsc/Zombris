
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;
using Zombris.Entities.Behaviours;
using Zombris.GridSystem;

namespace Zombris.Entities;

public abstract class Entity(string id, Point Position, Color color)
{
    protected string id = id;
    public Point Position { get; protected set; } = Position;
    public bool IsRunning {get; protected set;} = false;

    private readonly Dictionary<Type, IEntityComponent> components = [];

    // Attributes
    protected int currentHp = 1;
    protected int maxHp = 1;
    protected int speedMs = 200;

    protected Thread moveThread;
    protected Color color = color;

    public void Start(Grid grid)
    {
        IsRunning = true;
        moveThread = new(() => Simulate(grid));
        moveThread.Start();
    }

    public void Stop()
    {
        IsRunning = false;
        if(moveThread != null && moveThread.IsAlive) moveThread.Interrupt();
    }

    public void InitializeValues(int hp, int speedMs)
    {
        maxHp = currentHp = hp;
        this.speedMs = speedMs;
    }

    public void ChangePosition(int x, int y)
    {
        Position = new Point(x, y);
        Position = new Point(
            MathHelper.Clamp(Position.X, 0, GameConfig.GridWidth - 1),
            MathHelper.Clamp(Position.Y, 0, GameConfig.GridHeight - 1)
        );
    }

    public void AddComponent(IEntityComponent component)
    {
        components[component.GetType()] = component;
        component.Owner = this;
    }

    protected void ExchangeCurrentComponents(Entity otherEntity)
    {
        InitializeValues(otherEntity.maxHp, otherEntity.speedMs);
        components.Clear();
        foreach (Type key in otherEntity.components.Keys) AddComponent(otherEntity.components[key].Clone());
    }

    public T Get<T>() where T : class, IEntityComponent
    {
        components.TryGetValue(typeof(T), out var comp);
        return comp as T;
    }

    private void Simulate(Grid grid)
    {
        while (IsRunning)
        {
            Get<MovementComponent>()?.Execute(grid);
            Get<BehaviorComponent>()?.Execute(grid);
            Thread.Sleep(speedMs);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
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