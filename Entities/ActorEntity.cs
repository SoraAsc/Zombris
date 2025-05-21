
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Zombris.Core.GameConfig;

namespace Zombris.Entities;

public class ActorEntity(string id, Point position, ActorEntityType type) : Entity(id, position, Color.Blue)
{
    private Texture2D actorTexture = TextureManager.Get("Sprites/Entities/Blue/blue_0");
    public ActorEntityType Type { get; private set; } = type;

    public void ChangeActorSprite(Texture2D texture, Color color)
    {
        actorTexture = texture;
        this.color = color;
    }

    public void ChangeActorType(ActorEntityType type, ActorEntity infectedBy)
    {
        Type = type;
        ExchangeCurrentComponents(infectedBy);
        ChangeActorSprite(infectedBy.actorTexture, infectedBy.color);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int cellSize = CellSize;
        int pieceSize = PieceSize;

        int x = Position.X * cellSize;
        int y = Position.Y * cellSize;

        int offset = (cellSize - pieceSize) / 2;

        var rectangle = new Rectangle(x + offset, y + offset, pieceSize, pieceSize);
        spriteBatch.Draw(actorTexture, rectangle, color);
    }

}