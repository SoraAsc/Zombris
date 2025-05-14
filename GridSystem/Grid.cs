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

    public Entity GetEntityAt(int i, int j) { lock(lockObj) {return entities.ContainsKey((i, j)) ? entities[(i, j)] : null; } }
    public static bool IsInside(int x, int y) => x >= 0 && x < GameConfig.GridWidth && y >= 0 && y < GameConfig.GridHeight;
    public bool CanMove(int x, int y) => IsInside(x, y) && (!entities.ContainsKey((x, y)) || entities[(x, y)] == null);

    public bool TryMove(Entity e, int nx, int ny)
    {
        lock(lockObj)
        {
            if (nx < 0 || ny < 0 || nx > (GameConfig.GridWidth - 1) || ny > (GameConfig.GridHeight - 1)) return false;
            if (entities.ContainsKey((nx, ny)) && entities[(nx, ny)] != null) return false;
            entities[(e.Position.X, e.Position.Y)] = null;
            entities[(nx, ny)] = e;
            e.ChangePosition(nx, ny);
            return true;
        }
    }

    public List<Entity> GetNeighbors(int x, int y, int range)
    {
        List<Entity> neighbors = [];
        lock (lockObj)
        {
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = -range; dy <= range; dy++)
                {
                    if(dx == 0 && dy == 0) continue;
                    int nx = x + dx, ny = y + dy;
                    if (entities.ContainsKey((nx, ny)) && IsInside(nx, ny))
                    {
                        var e = entities[(nx, ny)];
                        if (e != null) neighbors.Add(e);
                    }
                }
            }
        }
        return neighbors;
    }

    public void Place(Entity e, int x, int y)
    {
        lock(lockObj)
        {
            // if(entities.ContainsKey((x, y)) && entities[(x, y)] != null && e.IsRunning) entities[(x, y)]?.Stop();
            e.ChangePosition(x, y);
            entities[(x, y)] = e;
        }
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
        List<Entity> snapshot;
        lock (lockObj) { snapshot = [.. entities.Values]; }
        foreach (var e in snapshot) e?.Draw(spriteBatch);
    }
}