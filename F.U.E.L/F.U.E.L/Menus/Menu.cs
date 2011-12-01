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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class Menu
    {
        public enum ButtonEvents
        {
            None,
            Close,
            Save,
            Quit,
            Go_Deeper
        }public ButtonEvents ButtonEvent { get; set; }   //Feedback event to MenuManager

        //protected Input inputHandle;                //Handle inputs
        protected SpriteFont titleFont;             //larger font to draw titles
        protected SpriteFont subTitleFont;             //larger font to draw titles
        protected SpriteFont textFont;              //Font to draw menu text

        protected Texture2D backgroundImage;        //The image to show on background of menu
        protected Song bgm;                         //An audio track to player while open
        protected SoundEffect menuOpen;             //Plays when menu first opens.
        protected SoundEffect menuClose;            //Plays when menu closes.
        protected string title;                     //Title at top of menu

        /// <summary>
        /// Constructs a base menu item.
        /// </summary>
        /// <param name="title">The 'name' of the menu, shows in title at top</param>
        public Menu(string title)
        {
            //inputHandle = new Input();
            this.title = title;
            this.ButtonEvent = ButtonEvents.None;
        }

        /// <summary>
        /// Load the menus content
        /// </summary>
        /// <param name="bgTexturePath">Path to texture in relation to Content Manager</param>
        /// <param name="bgmPath">Path to bgm file in relation to Content Manager</param>
        public virtual void Load(ContentManager Content, string bgTexturePath, string bgmPath, string menuOpenPath, string menuClosePath)
        {
            titleFont = Content.Load<SpriteFont>(@"ScreenManagerAssets\MenuFonts\menuFontTitle");
            subTitleFont = Content.Load<SpriteFont>(@"ScreenManagerAssets\MenuFonts\menuFontSubTitle");
            textFont = Content.Load<SpriteFont>(@"ScreenManagerAssets\MenuFonts\menuFont");
            //Load in our image and audio files
            backgroundImage = Content.Load<Texture2D>(bgTexturePath);
            bgm = Content.Load<Song>(bgmPath);
            menuOpen = Content.Load<SoundEffect>(menuOpenPath);
            menuClose = Content.Load<SoundEffect>(menuClosePath);
        }

        public virtual void Update(KeyboardState keyboard, GamePadState gamepad, int playerIndex)
        {
            //inputHandle.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch, int height, int width)
        {
            //spriteBatch.Draw(backgroundImage, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, width, height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

            Vector2 titlePosition = new Vector2(width / 2 - (titleFont.MeasureString(title).X / 2), 50);
            Vector2 titlePositionOffset = new Vector2(3 + width / 2 - (titleFont.MeasureString(title).X / 2), 50 + 3);
            Vector2 titlePosition2 = new Vector2(width / 2 - (subTitleFont.MeasureString("Press Start To Choose Your Class").X / 2), 130);
            Vector2 titlePosition2Offset = new Vector2(3 + width / 2 - (subTitleFont.MeasureString("Press Start To Choose Your Class").X / 2), 130 + 3);
            spriteBatch.DrawString(titleFont, title, titlePosition, Color.RoyalBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            spriteBatch.DrawString(titleFont, title, titlePositionOffset, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (this.title.Equals("Character Menu"))
            {
                spriteBatch.DrawString(subTitleFont, "Press Start To Choose Your Class", titlePosition2, Color.RoyalBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
                spriteBatch.DrawString(subTitleFont, "Press Start To Choose Your Class", titlePosition2Offset, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

        }

        /// <summary>
        /// This occurs when the menu is opened.
        /// </summary>
        /// <param name="playSoundEffect">Should sound effect be played during this call</param>
        public virtual void Open(bool playSoundEffect)
        {
            if (playSoundEffect)
                menuOpen.Play();
        }

        /// <summary>
        /// This occurs when menu is closed
        /// </summary>
        /// <param name="playSoundEffect">Should sound effect be played during this call</param>
        public virtual void Close(bool playSoundEffect)
        {
            if (playSoundEffect)
                menuClose.Play();
        }

        public void PlayBGM(bool loop)
        {
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(bgm);
        }

        public void StopBGM()
        {
            MediaPlayer.Stop();
        }
    }
}
