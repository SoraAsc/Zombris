
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;

namespace Zombris.Entities;

public abstract class Entity(Point GridPosition)
{
    public Point GridPosition { get; set; } = GridPosition;
    public static Color Color => Color.Blue;

    public virtual void Draw(SpriteBatch spriteBatch)
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
}