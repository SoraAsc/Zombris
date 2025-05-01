using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Zombris;

    public static class TextureManager
    {
        static readonly Dictionary<string, Texture2D> textures = [];

        // public static void LoadAll(ContentManager content)
        // {
        //     var assets = new[]
        //     {
        //         "Sprites/Tiles/tile0"
        //     };

        //     foreach (var asset in assets) textures[asset] = content.Load<Texture2D>(asset);
        // }

        public static void LoadAll(ContentManager content)
        {
            string root = content.RootDirectory;
            string spritesFolder = Path.Combine(root, "Sprites");

            if (!Directory.Exists(spritesFolder))
                throw new DirectoryNotFoundException($"Sprites folder not found: {spritesFolder}");

            var files = Directory.GetFiles(spritesFolder, "*.xnb", SearchOption.AllDirectories);
            foreach (var path in files)
            {
                // Derive asset name: relative path from root without extension
                string relative = Path.GetRelativePath(root, path).Replace(Path.DirectorySeparatorChar, '/');
                string assetName = relative[..^".xnb".Length];

                textures[assetName] = content.Load<Texture2D>(assetName);
            }
        }

        public static Texture2D Get(string key) => textures.TryGetValue(key, out var tex) 
            ? tex 
            : throw new KeyNotFoundException($"Texture '{key}' not found");
    }