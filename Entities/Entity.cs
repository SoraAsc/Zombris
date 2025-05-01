
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;

namespace Zombris.Entities;

public abstract class Entity
{
    protected Point GridPosition;
    protected Color Color;

    protected bool isRunning = true;
    protected Thread moveThread;
    private static readonly Random random = new();

    public Entity(Point GridPosition, Color Color)
    {
        this.GridPosition = GridPosition;
        this.Color = Color;

        moveThread = new(MoveLoop);
        moveThread.Start();
    }

    private void MoveLoop()
    {
        while (isRunning)
        {
            Thread.Sleep(1000); // 1 second, i need to customiz later
            int dx = random.Next(-1, 2);
            int dy = random.Next(-1, 2);
            GridPosition += new Point(dx, dy);
            GridPosition = new Point(
                MathHelper.Clamp(GridPosition.X, 0, GameConfig.GridWidth - 1),
                MathHelper.Clamp(GridPosition.Y, 0, GameConfig.GridHeight - 1)
            );
        }

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int cellSize = GameConfig.CellSize;
        int pieceSize = GameConfig.PieceSize;
        
        Texture2D texture = new(SceneManager.GraphicsDevice, 1, 1);
        texture.SetData([Color]);

        int x = GridPosition.X * cellSize;
        int y = GridPosition.Y * cellSize;

        int offset = (cellSize - pieceSize) / 2;

        var rectangle = new Rectangle(x + offset, y + offset, pieceSize, pieceSize);
        spriteBatch.Draw(texture, rectangle, Color);
    }

    public void Stop()
    {
        isRunning = false;
        moveThread.Join();
    }
}