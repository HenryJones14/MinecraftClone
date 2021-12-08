using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone
{
    class TextureSingle
    {
        public int Handle;

        public TextureSingle(string texturePath)
        {
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            Bitmap image = new Bitmap(texturePath);
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }

    class TextureArray
    {
        public int Handle;

        public TextureArray(string[] texturePaths)
        {
            CreateTextureArray(texturePaths.Length);
            for (int i = 0; i < texturePaths.Length; i++)
            {
                AddTextureToArray(texturePaths[i], i);
            }
        }

        private void CreateTextureArray(int TextureCount)
        {
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, Handle);

            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba, 64, 64, TextureCount, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
        }

        private void AddTextureToArray(string Path, int index)
        {
            Bitmap image = new Bitmap(Path);
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, index, 64, 64, 1, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2DArray, Handle);
        }
    }
}
