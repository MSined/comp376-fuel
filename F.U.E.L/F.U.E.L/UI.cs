using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class UI
    {
        SpriteBatch UISprites;
        GraphicsDevice graphicsDev;
        Texture2D UITexture;
        int textureHeight;
        int textureWidth;
        Vector2 position;
        Vector2 scale;

        public UI(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Texture2D UITxt, Texture2D healthTexture, int height, int width)
        {
            UISprites = spriteBatch;
            graphicsDev = graphicsDevice;
            UITexture = UITxt;
            textureHeight = UITexture.Height;
            textureWidth = UITexture.Width;
            scale = new Vector2(((float)width / (float)textureWidth), ((float)width / (float)textureWidth));
            System.Diagnostics.Debug.WriteLine(scale);
            position = new Vector2(0, height - ((float)textureHeight * ((float)width / (float)textureWidth)));
        }

        public void drawUserInterface()
        {
            UISprites.Draw(UITexture, position, null, Color.White, 0.0f, new Vector2(0, 0), scale, SpriteEffects.None, 0.0f);

        }
    }
}
