using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zombris
{
    public static class TextureManager
    {
        // Dicionário onde as Textures ficam armazenadas pelo nome virtual (asset name)
        private static readonly Dictionary<string, Texture2D> textures = [];

        /// <summary>
        /// Carrega todas as Textures2D compiladas (.xnb) que estejam na pasta de Content do output.
        /// Ignora arquivos que não são texturas (como fontes).
        /// </summary>
        public static void LoadAll(ContentManager content)
        {
            textures.Clear();

            string contentDir = Path.Combine(AppContext.BaseDirectory, content.RootDirectory);

            if (!Directory.Exists(contentDir))
                throw new DirectoryNotFoundException($"Content directory not found: {contentDir}");

            // Encontra todos os .xnb recursivamente
            var files = Directory.GetFiles(contentDir, "*.xnb", SearchOption.AllDirectories);

            foreach (var fullPath in files)
            {
                try
                {
                    // Pega o caminho relativo dentro de Content e remove a extensão
                    string relativePath = Path.GetRelativePath(contentDir, fullPath);
                    string assetName = Path.ChangeExtension(relativePath, null)
                                             .Replace(Path.DirectorySeparatorChar, '/');

                    var texture = content.Load<Texture2D>(assetName);
                    textures[assetName] = texture;
                }
                catch (InvalidCastException) { continue; } // Ignora arquivos que não são texturas
            }
        }

        /// <summary>
        /// Recupera a Texture2D já carregada pelo key (asset name).
        /// </summary>
        public static Texture2D Get(string key)
        {
            if (textures.TryGetValue(key, out var tex)) return tex;
            throw new KeyNotFoundException($"Texture not found: {key}");
        }
    }
}
