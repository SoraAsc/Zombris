using System;
using Microsoft.Xna.Framework;
using Zombris.Core;
using Zombris.Entities.Factory;
using Zombris.GridSystem;

namespace Zombris.Scenes;

public class GameScene : IScene
{
    private static readonly Random rng = new();
    private Grid grid;

    public void LoadContent()
    {
        grid = new Grid(GameConfig.GridWidth, GameConfig.GridHeight, GameConfig.CellSize);
        grid.AddEntities(FactoryManager.CreateRandomEntities(grid, rng.Next(10, 30), rng.Next(10, 20)));

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
