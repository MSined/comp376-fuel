﻿///Modified version of the Menu system by:
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
            titleFont = Content.Load<SpriteFont>(@"ScreenManagerAssets\MenuFonts\menuFont");
            textFont = Content.Load<SpriteFont>(@"ScreenManagerAssets\MenuFonts\menuFont");
            //Load in our image and audio files
            backgroundImage = Content.Load<Texture2D>(bgTexturePath);
            bgm = Content.Load<Song>(bgmPath);
            menuOpen = Content.Load<SoundEffect>(menuOpenPath);
            menuClose = Content.Load<SoundEffect>(menuClosePath);
        }

        public virtual void Update(MouseState mouse, KeyboardState keyboard)
        {
            //inputHandle.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch, int height, int width)
        {
            //spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, width, height), Color.White);

            Vector2 titlePosition = new Vector2(width / 2 - (titleFont.MeasureString(title).X / 2), 50);
            spriteBatch.DrawString(titleFont, title, titlePosition, Color.Black);
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
