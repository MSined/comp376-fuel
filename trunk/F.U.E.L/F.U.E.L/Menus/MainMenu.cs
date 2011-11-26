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
        bool wKeyDown = false;
        bool sKeyDown = false;

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
                if (i == 0)
                    buttons[i].setSelected(true);
                buttons[i].Load(Content);
            }
        }

        public override void Update(KeyboardState keyboard)
        {
            

            //if (inputHandle.isMouseLeftNew())
            //if (mouse.LeftButton == ButtonState.Pressed)
            //{
            //    foreach (Button b in buttons)
            //    {
            //        if (b.getDimensions().Contains(new Point(mouse.X, mouse.Y)))
            //            ButtonPush(b.getID());
            //    }
            //}

            if (keyboard.IsKeyDown(Keys.W) && !wKeyDown)
            {
                int temp = 0;
                wKeyDown = true;
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].getSelected())
                    {
                        buttons[i].setSelected(false);
                        int position = Math.Abs((i-1) % buttons.Count);
                        temp = buttons[position].getID();
                    }
                }

                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].getID() == temp)
                    {
                        buttons[i].setSelected(true);
                        
                    }
                }
            }

            if (keyboard.IsKeyUp(Keys.W) && wKeyDown)
            {
                wKeyDown = false;

                System.Diagnostics.Debug.WriteLine(keyboard.IsKeyUp(Keys.W));
            }
            if (keyboard.IsKeyDown(Keys.S) && !sKeyDown)
            {
                int temp = 0;
                sKeyDown = true;
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].getSelected())
                    {
                        buttons[i].setSelected(false);
                        int position = Math.Abs((i + 1) % buttons.Count);
                        temp = buttons[position].getID();
                    }
                }

                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].getID() == temp)
                    {
                        buttons[i].setSelected(true);

                    }
                }
            }

            if (keyboard.IsKeyUp(Keys.S) && sKeyDown)
            {
                sKeyDown = false;
            }

            base.Update(keyboard);

        }

        public string selected()
        {
            string output = "";
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].getSelected())
                {
                    output = buttons[i].getText();
                }
            }
            return output;
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
            //switch (id)
            //{
            //    case 1:
            //        ButtonEvent = ButtonEvents.Close;
            //        break;
            //    case 2:
            //        this.ButtonEvent = ButtonEvents.Save;
            //        break;
            //    case 3:
            //        this.ButtonEvent = ButtonEvents.Quit;
            //        break;
            //}
        }
    }
}
