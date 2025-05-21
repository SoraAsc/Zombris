
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;
using static Zombris.Core.GameConfig;

namespace Zombris.Entities;

public class ActorEntity(string id, Point position, ActorEntityType type) : Entity(id, position, Color.Blue)
{
    public ActorEntityType Type { get; private set; } = type;

    public void ChangeActorType(ActorEntityType type, ActorEntity infectedBy)
    {
        Type = type;
        ExchangeCurrentComponents(infectedBy);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // base.Draw(spriteBatch);
        int cellSize = CellSize;
        int pieceSize = PieceSize;
        Color color = Type == ActorEntityType.Blue ? Color.Blue : Color.Green;
        Texture2D texture = new(SceneManager.GraphicsDevice, 1, 1);
        texture.SetData([color]);

        int x = Position.X * cellSize;
        int y = Position.Y * cellSize;

        int offset = (cellSize - pieceSize) / 2;

        var rectangle = new Rectangle(x + offset, y + offset, pieceSize, pieceSize);
        spriteBatch.Draw(texture, rectangle, color);
    }

}