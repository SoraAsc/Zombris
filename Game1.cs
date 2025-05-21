using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zombris.Core;
using Zombris.Scenes;

namespace Zombris;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new(this)
        {
            PreferredBackBufferWidth = GameConfig.ScreenWidth,
            PreferredBackBufferHeight = GameConfig.ScreenHeight
        };
        // Window.IsBorderless = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);

        // Load all game textures in the future load only the texture used in scene
        TextureManager.LoadAll(Content);

        // Initialize the Scene Manager
        SceneManager.Content = Content;
        SceneManager.GraphicsDevice = GraphicsDevice;
        SceneManager.SpriteBatch = _spriteBatch;

        SceneManager.ChangeScene(new GameScene()); // Start with game scene
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        SceneManager.CurrentScene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSlateGray);

        _spriteBatch.Begin();
        SceneManager.CurrentScene.Draw(gameTime);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        SceneManager.CurrentScene.UnloadContent();
    }
}
