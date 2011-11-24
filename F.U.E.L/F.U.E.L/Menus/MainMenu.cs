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
using Microsoft.Xna.Framework.Input;

namespace F.U.E.L
{
    class MainMenu : Menu
    {
        List<Button> buttons;

        public MainMenu(string title)
            : base(title)
        {
            buttons = new List<Button>();
        }

        public void LoadButtons(ContentManager Content, int[] id, List<Rectangle> bounds, List<string> text)
        {
            for (int i = 0; i < id.Count(); i++)
            {
                this.buttons.Add(new Button(id[i], bounds[i], text[i]));
                buttons[i].Load(Content);
            }
        }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            base.Update(mouse, keyboard);

            //if (inputHandle.isMouseLeftNew())
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                foreach (Button b in buttons)
                {
                    if (b.getDimensions().Contains(new Point(mouse.X, mouse.Y)))
                        ButtonPush(b.getID());
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int height, int width)
        {
            //Call base first, to get background image and title
            base.Draw(spriteBatch, height, width);

            //Then draw our main menu buttons
            foreach (Button b in buttons)
            {
                b.Draw(spriteBatch, textFont);
            }
        }

        public void ButtonPush(int id)
        {
            switch (id)
            {
                case 1:
                    ButtonEvent = ButtonEvents.Close;
                    break;
                case 2:
                    this.ButtonEvent = ButtonEvents.Save;
                    break;
                case 3:
                    this.ButtonEvent = ButtonEvents.Quit;
                    break;
            }
        }
    }
}
