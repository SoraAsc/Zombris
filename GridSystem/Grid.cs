using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;
using Zombris.Entities;

namespace Zombris.GridSystem;

public class Grid(int width, int height, int cellSize)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    public int CellSize { get; } = cellSize;

    private readonly Dictionary<(int x, int y), Entity> entities = [];

    public bool GameOver { get; private set; } = false;
    private readonly object lockObj = new();

    public void AddEntities(List<Entity> entities)
    {
        foreach (var e in entities) this.entities[(e.Position.X, e.Position.Y)] = e;
    }

    public void StartAllEntities()
    {
        foreach (var e in entities.Values.ToList()) e?.Start(this);
    }

    public void StopAllEntities()
    {
        GameOver = true;
        foreach (var e in entities.Values.ToList()) e?.Stop();
    }

    public bool TryMove(Entity e, int nx, int ny)
    {
        lock(lockObj)
        {
            if (nx < 0 || ny < 0 || nx >= (GameConfig.GridWidth - 1) || ny >= (GameConfig.GridHeight - 1)) return false;
            if (entities.ContainsKey((nx, ny)) && entities[(nx, ny)] != null) return false;
            entities[(e.Position.X, e.Position.Y)] = null;
            entities[(nx, ny)] = e;
            e.ChangePosition(nx, ny);
            return true;
        }
    }

    public void Place(Entity e, int x, int y)
    {
        lock(lockObj) entities[(x, y)] = e;
    }

    public void Draw(SpriteBatch spriteBatch, Color? tileColor = null, bool showLines = true)
    {
        var color = tileColor ?? Color.White;
        Texture2D texture = TextureManager.Get("Sprites/Tiles/white");
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Rectangle cellRect = new(x * CellSize, y * CellSize, CellSize, CellSize);
                spriteBatch.Draw(texture, cellRect, color);

                if (showLines) DrawCellBorder(spriteBatch, cellRect, Color.Black * 0.5f);
            }
        }
        DrawEntities(spriteBatch);
    }

    private static void DrawCellBorder(SpriteBatch spriteBatch, Rectangle rect, Color color)
    {
        Texture2D pixel = TextureManager.Get("Sprites/Tiles/white");
        int thickness = 1;

        // Top
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Left
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // Right
        spriteBatch.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
    }

    private void DrawEntities(SpriteBatch spriteBatch)
    {
        foreach (var e in entities.Values.ToList()) e?.Draw(spriteBatch);
    }
}