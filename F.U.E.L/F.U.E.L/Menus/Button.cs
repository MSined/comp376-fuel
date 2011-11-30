///Modified version of the Menu system by:
///=============================================================///
///Author:  Jonathan Deaves ("garfunkle")                       ///
///Date:    13-June-2011                                        ///
///Version: 0.1a                                                ///
///=============================================================///

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace F.U.E.L
{
    class Button
    {
        int buttonID;

        Texture2D texture;
        Rectangle dimensions;
        bool isSelected = false;

        string label;
        Vector2 position;

        public Button(int ID, Rectangle inputDimensions, string inputText)
        {
            buttonID = ID;
            dimensions = inputDimensions;
            label = inputText;
        }

        public int getID()
        {
            return buttonID;
        }

        public void setSelected(bool input)
        {
            isSelected = input;
        }

        public bool getSelected()
        {
            return isSelected;
        }

        public Rectangle getDimensions()
        {
            return dimensions;
        }

        public void Load(ContentManager content)
        {
            this.texture = content.Load<Texture2D>(@"ScreenManagerAssets/Textures/Button");
        }

        public string getText()
        {
            return label;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            position = new Vector2(dimensions.X, dimensions.Y);
            position += new Vector2((dimensions.Width / 2) - (spriteFont.MeasureString(label).X / 2), (dimensions.Height / 2) - (spriteFont.MeasureString(label).Y / 2)) - new Vector2(texture.Width / 2, 0);

            spriteBatch.Draw(texture, new Vector2(dimensions.X, dimensions.Y) - new Vector2(texture.Width / 2, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.25f);
            if (isSelected)
            {
                spriteBatch.DrawString(spriteFont, label, position, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            //if (isSelectedFinal)
            //{
            //    spriteBatch.DrawString(spriteFont, label, position, Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            //}
            else
                spriteBatch.DrawString(spriteFont, label, position, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
