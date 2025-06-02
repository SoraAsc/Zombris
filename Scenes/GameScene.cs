using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombris.Core;
using Zombris.Entities;
using Zombris.Entities.Factory;
using Zombris.GridSystem;

namespace Zombris.Scenes;

public class GameScene : IScene
{
    private static readonly Random rng = new();
    private Grid grid;
    private bool gameOver = false;
    private string gameOverMessage = "";
    private SpriteFont gameFont;

    public void LoadContent()
    {
        grid = new Grid(GameConfig.GridWidth, GameConfig.GridHeight, GameConfig.CellSize);
        grid.AddEntities(FactoryManager.CreateRandomEntities(grid, rng.Next(10, 30), rng.Next(10, 20)));
        gameFont = SceneManager.Content.Load<SpriteFont>("Fonts/GameFont");

        grid.StartAllEntities();
    }

    public void Update(GameTime gameTime) 
    { 
        if (!gameOver) CheckEndConditions();
    }

    private void CheckEndConditions()
    {
        // Get all entities from the grid
        var entities = grid.GetAllEntities();
        
        // Check if any blue actor reached the right edge
        bool blueReachedEnd = entities.Any(e => 
            e is ActorEntity actor && 
            actor.Type == GameConfig.ActorEntityType.Blue && 
            actor.Position.X >= GameConfig.GridWidth - 1);

        // Check if all blue actors are gone
        bool noBlueLeft = !entities.Any(e => 
            e is ActorEntity actor && 
            actor.Type == GameConfig.ActorEntityType.Blue);

        if (blueReachedEnd || noBlueLeft)
        {
            gameOver = true;
            gameOverMessage = blueReachedEnd ? "Blue Team Wins!" : "Zombies Win!";

            grid.StopAllEntities();
        }
    }

    public void Draw(GameTime gameTime)
    {
        var sb = SceneManager.SpriteBatch;
        grid.Draw(sb);

        if (gameOver)
        {
            // Draw game over message with a blue color that's visible on white background
            var textSize = gameFont.MeasureString(gameOverMessage);
            var textPosition = new Vector2(
                (GameConfig.ScreenWidth - textSize.X) / 2,
                (GameConfig.ScreenHeight - textSize.Y) / 2
            );
            sb.DrawString(gameFont, gameOverMessage, textPosition, new Color(0, 0, 139));
        }
    }

    public void UnloadContent() => grid.StopAllEntities();
}
