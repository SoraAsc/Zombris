using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombris.GridSystem;

public class Grid(int width, int height, int cellSize)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    public int CellSize { get; } = cellSize;

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D texture = TextureManager.Get("Sprites/Tiles/tile0");
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                spriteBatch.Draw(texture, new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize), Color.White);
    }

}