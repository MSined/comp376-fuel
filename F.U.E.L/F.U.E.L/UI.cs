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
        Vector2 blackboxScale = new Vector2(40,20);
        Vector2 blackboxPosition;
        int height, width;

        Texture2D minimapTexture;
        Texture2D unitsTexture;
        Texture2D cooldownBG;
        Rectangle playerIconRect = new Rectangle(5,0,5,5);
        Rectangle enemyIconRect = new Rectangle(10,0,5,5);
        Rectangle generatorIconRect = new Rectangle(15, 0, 5, 5);
        Rectangle brokenGeneratorIconRect = new Rectangle(20, 0, 5, 5);

        public UI(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Texture2D UITxt, Texture2D healthTexture, int height, int width, Texture2D minimapTexture, Texture2D unitsTexture)
        {
            UISprites = spriteBatch;
            graphicsDev = graphicsDevice;
            UITexture = UITxt;
            textureHeight = UITexture.Height;
            textureWidth = UITexture.Width;
            scale = new Vector2(((float)width / (float)textureWidth), ((float)width / (float)textureWidth));
            position = new Vector2(0, height - ((float)textureHeight * ((float)width / (float)textureWidth)));
            blackboxPosition = new Vector2(0, height - (((float)textureHeight * ((float)width / (float)textureWidth))/2));

            this.height = height;
            this.width = width;
            this.minimapTexture = minimapTexture;
            this.unitsTexture = unitsTexture;
        }

        public void drawUserInterface(List<Player> players, List<Enemy> enemies, List<Building> usableBuildings)
        {
            DrawMinimap(players, enemies, usableBuildings);
            UISprites.Draw(UITexture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.2f);
            UISprites.Draw(unitsTexture, blackboxPosition, null, Color.Black, 0.0f, Vector2.Zero, blackboxScale*scale, SpriteEffects.None, 0.4f);
        }

        public void drawCooldowns(Texture2D texture, double totalTime, double elapsedTime, int abilityNum)
        {
            //203x103
            //UISprites.Draw(texture, new Rectangle(((int)Math.Floor((float)(203) / (float)1000 * width)) + ((int)(((float)44 / (float)1000) * width) * abilityNum) + ((int)(((float)7 / (float)1000) * width) * abilityNum), (height - (int)(width / 1000f * 200f) + (int)Math.Floor((103 / 200f) * (width / 1000f * 200f))), (int)(((float)44 / (float)1000) * width), (int)(MathHelper.Clamp((float)(elapsedTime / totalTime), 0f, 1f) * (int)(((float)44 / (float)1000) * width))), Color.Black * 0.5f);
            UISprites.Draw(texture,
                new Vector2(((int)Math.Floor(203f * width / 1000)) + ((int)(44f * width / 1000) * abilityNum) + ((int)(7f * width / 1000) * abilityNum), (height - (int)(width / 1000f * 200f) + (int)Math.Floor((103 / 200f) * (width / 1000f * 200f)))),
                new Rectangle(0, 0, 44, (int)(MathHelper.Clamp((float)((totalTime - elapsedTime) / totalTime) * 44, 0, 44))),
                Color.Black*0.5f, 0f, Vector2.Zero,
                scale,
                SpriteEffects.None, 0.11f);
        }

        public void drawSelectedWeapon(Texture2D texture, int selectedNum) 
        {
            UISprites.Draw(texture,
                new Vector2 (((int)Math.Floor(203f * width/1000)) + ((int)(44f * width/1000) * selectedNum) + ((int)(7f * width/1000) * selectedNum), (height - (int)(width/1000f * 200f) + (int)Math.Floor((103 / 200f) * (width/1000f * 200f)))),
                new Rectangle(0, 0, 44, 44),
                Color.White,0f,Vector2.Zero,
                scale,
                SpriteEffects.None,0.1f);
        }

        public void drawSkills(Texture2D texture, Player player)
        {
            UISprites.Draw(texture,
                    new Vector2(((int)Math.Floor(203f * width / 1000)) + ((int)(44f * width / 1000) * (player.playerID * 4)) + ((int)(7f * width / 1000) * (player.playerID * 4)), (height - (int)(width / 1000f * 200f) + (int)Math.Floor((153 / 200f) * (width / 1000f * 200f)))),
                    new Rectangle(176, (player.playerID+1) * 44, 44, 44),
                    Color.White, 0f, Vector2.Zero,
                    scale,
                    SpriteEffects.None, 0.12f);
            
            for (int i = 0; i < 4; i++)
            {
                UISprites.Draw(texture,
                    new Vector2(((int)Math.Floor(203f * width / 1000)) + ((int)(44f * width / 1000) * (i + (player.playerID * 4))) + ((int)(7f * width / 1000) * (i + (player.playerID * 4))), (height - (int)(width / 1000f * 200f) + (int)Math.Floor((103 / 200f) * (width / 1000f * 200f)))),
                    new Rectangle(i*44, player.playerClass*44, 44, 44),
                    Color.White, 0f, Vector2.Zero,
                    scale,
                    SpriteEffects.None, 0.12f);
            }
        }

        private void DrawMinimap(List<Player> players, List<Enemy> enemies, List<Building> usableBuildings) 
        {
            UISprites.Draw(minimapTexture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.34f);
            foreach (Enemy e in enemies)
            {
                Vector2 minimapPosition = Vector2.Zero;
                minimapPosition.X = position.X + (100f + (e.position.X * 2.778f)) * scale.X;
                minimapPosition.Y = position.Y + (100f + (e.position.Z * 2.778f)) * scale.Y;
                UISprites.Draw(unitsTexture, minimapPosition, enemyIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.32f);
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
                        UISprites.Draw(unitsTexture, minimapPosition, generatorIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.31f);
                    }
					else
                    {
                        UISprites.Draw(unitsTexture, minimapPosition, brokenGeneratorIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.31f);
                    }
                }
            }
            foreach (Player p in players)
            {
                Vector2 minimapPosition = Vector2.Zero;
                minimapPosition.X = position.X + (100f + (p.position.X * 2.778f)) * scale.X;
                minimapPosition.Y = position.Y + (100f + (p.position.Z * 2.778f)) * scale.Y;
                UISprites.Draw(unitsTexture, minimapPosition, playerIconRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.3f);
            }
        }
    }
}
