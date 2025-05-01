using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Zombris.Core;
using Zombris.Entities;
using Zombris.GridSystem;

namespace Zombris.Scenes;

public class GameScene : IScene
{
    private Grid grid;
    private List<Entity> entities;

    public void LoadContent()
    {
        grid = new Grid(GameConfig.GridWidth, GameConfig.GridHeight, GameConfig.CellSize);
        entities = [
            new Zombie(new Point(9, 9)),
            new Blue(new Point(0, 0))
        ];
    }

    public void Update(GameTime gameTime) { }

    public void Draw(GameTime gameTime)
    {
        var sb = SceneManager.SpriteBatch;
        grid.Draw(sb);
        foreach(var e in entities) e.Draw(sb);
    }

    public void UnloadContent() 
    { 
        StopEntities();
    }

    private void StopEntities()
    {
        foreach(var e in entities) e.Stop();
    }
}
