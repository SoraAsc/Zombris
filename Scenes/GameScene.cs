using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Zombris.Core;
using Zombris.Entities;
using Zombris.GridSystem;

namespace Zombris.Scenes;

public class GameScene : IScene
{
    private Grid grid;

    public void LoadContent()
    {
        grid = new Grid(GameConfig.GridWidth, GameConfig.GridHeight, GameConfig.CellSize);
        List<Entity> entities = [
            new Zombie(new Point(9, 9)),
            new Blue(new Point(0, 0))
        ];
        grid.AddEntities(entities);

        grid.StartAllEntities();
    }

    public void Update(GameTime gameTime) { }

    public void Draw(GameTime gameTime)
    {
        var sb = SceneManager.SpriteBatch;
        grid.Draw(sb);
    }

    public void UnloadContent() 
    { 
        grid.StopAllEntities();
    }
    
}
