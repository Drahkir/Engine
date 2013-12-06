using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tao.DevIl;
using Tao.OpenGl;

namespace Engine
{
    public class TextureManager : IDisposable
    {
        private readonly Dictionary<string, Texture> _textureDatabase = new Dictionary<string, Texture>();

        public Texture Get(string textureId)
        {
            return _textureDatabase[textureId];
        }

        public void LoadTexture(string textureId, string path)
        {
            int devilId = 0;
            Il.ilGenImages(1, out devilId);
            Il.ilBindImage(devilId); // set as the active texture.

            if (!Il.ilLoadImage(path))
            {
                Debug.Assert(false,
                    "Could not open file, [" + path + "].");
            }

            Ilu.iluFlipImage();

            int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            int openGLId = Ilut.ilutGLBindTexImage();

            Debug.Assert(openGLId != 0);
            Il.ilDeleteImages(1, ref devilId);

            _textureDatabase.Add(textureId, new Texture(openGLId, width, height));
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (Texture t in _textureDatabase.Values)
            {
                Gl.glDeleteTextures(1, new[] {t.Id});
            }
        }

        #endregion
    }
}