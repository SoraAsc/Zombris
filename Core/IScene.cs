using Microsoft.Xna.Framework;

namespace Zombris.Core;

public interface IScene 
{
    void LoadContent();
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);
    void UnloadContent();
}