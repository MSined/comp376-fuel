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
        int height, width;

        Texture2D minimapTexture;
        Texture2D unitsTexture;
        Rectangle playerIconRect = new Rectangle(0,0,5,5);
        Rectangle enemyIconRect = new Rectangle(5,0,5,5);
        Rectangle generatorIconRect = new Rectangle(10, 0, 5, 5);
        Rectangle brokenGeneratorIconRect = new Rectangle(15, 0, 5, 5);

        public UI(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Texture2D UITxt, Texture2D healthTexture, int height, int width, Texture2D minimapTexture, Texture2D unitsTexture)
        {
            UISprites = spriteBatch;
            graphicsDev = graphicsDevice;
            UITexture = UITxt;
            textureHeight = UITexture.Height;
            textureWidth = UITexture.Width;
            scale = new Vector2(((float)width / (float)textureWidth), ((float)width / (float)textureWidth));
            position = new Vector2(0, height - ((float)textureHeight * ((float)width / (float)textureWidth)));

            this.height = height;
            this.width = width;
            this.minimapTexture = minimapTexture;
            this.unitsTexture = unitsTexture;
        }

        public void drawUserInterface(List<Player> players, List<Enemy> enemies, List<Building> usableBuildings)
        {
            DrawMinimap(players, enemies, usableBuildings);
            UISprites.Draw(UITexture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawMinimap(List<Player> players, List<Enemy> enemies, List<Building> usableBuildings) 
        {
            UISprites.Draw(minimapTexture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            foreach (Enemy e in enemies)
            {
                Vector2 minimapPosition = Vector2.Zero;
                minimapPosition.X = position.X + (100f + (e.position.X * 2.778f)) * scale.X;
                minimapPosition.Y = position.Y + (100f + (e.position.Z * 2.778f)) * scale.Y;
                UISprites.Draw(unitsTexture, minimapPosition, enemyIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }
            foreach (Building b in usableBuildings)
            {
                if (b is Generator)
                {
                    Generator g = (Generator)b;
                    Vector2 minimapPosition = Vector2.Zero;
                    minimapPosition.X = position.X + (100f + (g.position.X * 2.778f)) * scale.X;
                    minimapPosition.Y = position.Y + (100f + (g.position.Z * 2.778f)) * scale.Y;
                    if (g.functional)
                    {
                        UISprites.Draw(unitsTexture, minimapPosition, generatorIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                    }
					else
                    {
                        UISprites.Draw(unitsTexture, minimapPosition, brokenGeneratorIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                    }
                }
            }
            foreach (Player p in players)
            {
                Vector2 minimapPosition = Vector2.Zero;
                minimapPosition.X = position.X + (100f + (p.position.X * 2.778f)) * scale.X;
                minimapPosition.Y = position.Y + (100f + (p.position.Z * 2.778f)) * scale.Y;
                UISprites.Draw(unitsTexture, minimapPosition, playerIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}
