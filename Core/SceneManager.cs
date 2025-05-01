using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zombris.Core;

public static class SceneManager
{
    public static ContentManager Content { get; set; }
    public static GraphicsDevice GraphicsDevice { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static IScene CurrentScene { get; private set; }

    public static void ChangeScene(IScene newScene)
    {
        CurrentScene?.UnloadContent();
        CurrentScene = newScene;
        CurrentScene.LoadContent();
    }
}
