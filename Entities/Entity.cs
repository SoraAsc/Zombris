
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;

namespace Zombris.Entities;

public abstract class Entity(Point Position, Color color, string id)
{
    protected string id = id;
    public Point Position { get; protected set; } = Position;
    protected bool isRunning = true;

    protected Thread moveThread;
    protected Color color = color;
    private static readonly Random random = new();

    public void Start()
    {
        moveThread = new(MoveLoop);
        moveThread.Start();
    }

    public void Stop()
    {
        isRunning = false;
        moveThread.Join();
    }

    private void MoveLoop()
    {
        while (isRunning)
        {
            Thread.Sleep(200); // 200 ms, i need to customize later
            int dx = random.Next(-1, 2);
            int dy = random.Next(-1, 2);
            Position += new Point(dx, dy);
            Position = new Point(
                MathHelper.Clamp(Position.X, 0, GameConfig.GridWidth - 1),
                MathHelper.Clamp(Position.Y, 0, GameConfig.GridHeight - 1)
            );
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