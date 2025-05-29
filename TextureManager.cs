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
        private static readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Carrega todas as Textures2D compiladas (.xnb) que estejam na pasta de Content do output.
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
                // Pega o caminho relativo dentro de Content e remove a extensão
                string relativePath = Path.GetRelativePath(contentDir, fullPath);
                string assetName = Path.ChangeExtension(relativePath, null)
                                         .Replace(Path.DirectorySeparatorChar, '/');

                // Carrega via ContentManager usando o nome de asset (sem .xnb)
                textures[assetName] = content.Load<Texture2D>(assetName);
            }
        }

        /// <summary>
        /// Recupera a Texture2D já carregada pelo key (asset name).
        /// </summary>
        public static Texture2D Get(string key)
        {
            if (textures.TryGetValue(key, out var tex))
                return tex;

            throw new KeyNotFoundException($"Texture '{key}' not found. " +
                                           "Certifique-se de que ela foi compilada e carregada em LoadAll.");
        }
    }
}
