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
        public List<Button> buttons;
        public bool upButtonDown1 = false, upButtonDown2 = false, upButtonDown3 = false, upButtonDown4 = false, currentUpButtonDown = false;
        public bool downButtonDown1 = false, downButtonDown2 = false, downButtonDown3 = false, downButtonDown4 = false, currentDownButtonDown = false;
        public bool player1Chosen = false, player2Chosen = false, player3Chosen = false, player4Chosen = false;
        public bool allPlayersChose = false;

        public bool singlePlayer = false;
        public bool enterUp = false;

        int buttonLowerLimit = 0, buttonUpperLimit = 4;

        public MainMenu(string title)
            : base(title)
        {
            buttons = new List<Button>();
        }

        public void LoadButtons(ContentManager content, int[] id, List<Rectangle> bounds, List<string> text)
        {
            for (int i = 0; i < id.Count(); i++)
            {
                this.buttons.Add(new Button(id[i], bounds[i], text[i]));
                if (i == 0)
                    buttons[i].setSelected(true);
                buttons[i].Load(content);
            }
        }

        public override void Update(KeyboardState keyboard, GamePadState gamepad, int playerIndex)
        {
            if (this.title.Equals("Character Menu"))
            {
                buttonLowerLimit = playerIndex * 5 - 5;
                buttonUpperLimit = buttonLowerLimit + 5;
            }
            else
            {
                buttonLowerLimit = 0;
                buttonUpperLimit = buttons.Count;
            }

            #region Single Player Keyboard Controls
            if (singlePlayer)
            {
                if(keyboard.IsKeyDown(Keys.W) && !upButtonDown1)
                {
                    int temp = 0;
                    upButtonDown1 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((((i - 1) % buttonUpperLimit) + buttonUpperLimit) % buttonUpperLimit);
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if (keyboard.IsKeyUp(Keys.W) && upButtonDown1)
                {
                    upButtonDown1 = false;
                }

                if (keyboard.IsKeyDown(Keys.S) && !downButtonDown1)
                {
                    int temp = 0;
                    downButtonDown1 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((i + 1) % buttonUpperLimit);
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if (keyboard.IsKeyUp(Keys.S) && downButtonDown1)
                {
                    downButtonDown1 = false;
                }

                if (keyboard.IsKeyUp(Keys.Enter))
                    enterUp = true;

                if (keyboard.IsKeyDown(Keys.Enter) && this.title.Equals("Character Menu") && enterUp)
                {
                    player1Chosen = true;
                    player2Chosen = true;
                    player3Chosen = true;
                    player4Chosen = true;
                    allPlayersChose = true;
                }
            }
            #endregion

            #region Gamepad 1 Support
            if (!singlePlayer && playerIndex == 1 && !player1Chosen)
            {
                if ((gamepad.IsButtonDown(Buttons.DPadUp) && !upButtonDown1))
                {
                    int temp = 0;
                    upButtonDown1 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((((i - 1) % buttonUpperLimit) + buttonUpperLimit) % buttonUpperLimit);
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadUp) && upButtonDown1))
                {
                    upButtonDown1 = false;
                }

                if ((gamepad.IsButtonDown(Buttons.DPadDown) && !downButtonDown1))
                {
                    int temp = 0;
                    downButtonDown1 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((i + 1) % buttonUpperLimit);
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadDown) && downButtonDown1))
                {
                    downButtonDown1 = false;
                }

                if (gamepad.IsButtonDown(Buttons.Start) && this.title.Equals("Character Menu"))
                {
                    player1Chosen = true;
                    Game1.StartButtonDown1 = true;
                }
            }
            #endregion

            #region Gamepad 2 Support
            if (!singlePlayer && playerIndex == 2 && !player2Chosen)
            {
                if ((gamepad.IsButtonDown(Buttons.DPadUp) && !upButtonDown2))
                {
                    int temp = 5;
                    upButtonDown2 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((((i - 1) % buttonUpperLimit) + buttonUpperLimit) % buttonUpperLimit);
                            if (position == 4)
                                position = 9;
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadUp) && upButtonDown2))
                {
                    upButtonDown2 = false;
                }

                if ((gamepad.IsButtonDown(Buttons.DPadDown) && !downButtonDown2))
                {
                    int temp = 5;
                    downButtonDown2 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((i + 1) % buttonUpperLimit);
                            if (position == 0)
                                position = 5;
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadDown) && downButtonDown2))
                {
                    downButtonDown2 = false;
                }

                if (gamepad.IsButtonDown(Buttons.Start) && this.title.Equals("Character Menu"))
                {
                    player2Chosen = true;
                    Game1.StartButtonDown2 = true;
                }
            }
            #endregion

            #region Gamepad 3 Support
            if (!singlePlayer && playerIndex == 3 && !player3Chosen)
            {
                if ((gamepad.IsButtonDown(Buttons.DPadUp) && !upButtonDown3))
                {
                    int temp = 10;
                    upButtonDown3 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((((i - 1) % buttonUpperLimit) + buttonUpperLimit) % buttonUpperLimit);
                            if (position == 9)
                                position = 14;
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadUp) && upButtonDown3))
                {
                    upButtonDown3 = false;
                }

                if ((gamepad.IsButtonDown(Buttons.DPadDown) && !downButtonDown3))
                {
                    int temp = 10;
                    downButtonDown3 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((i + 1) % buttonUpperLimit);
                            if (position == 0)
                                position = 10;
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadDown) && downButtonDown3))
                {
                    downButtonDown3 = false;
                }

                if (gamepad.IsButtonDown(Buttons.Start) && this.title.Equals("Character Menu"))
                {
                    player3Chosen = true;
                    Game1.StartButtonDown3 = true;
                }
            }
            #endregion

            #region Gamepad 4 Support
            if (!singlePlayer && playerIndex == 4 && !player4Chosen)
            {
                if ((gamepad.IsButtonDown(Buttons.DPadUp) && !upButtonDown4))
                {
                    int temp = 15;
                    upButtonDown4 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((((i - 1) % buttonUpperLimit) + buttonUpperLimit) % buttonUpperLimit);
                            if (position == 14)
                                position = 19;
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadUp) && upButtonDown4))
                {
                    upButtonDown4 = false;
                }

                if ((gamepad.IsButtonDown(Buttons.DPadDown) && !downButtonDown4))
                {
                    int temp = 15;
                    downButtonDown4 = true;
                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getSelected())
                        {
                            buttons[i].setSelected(false);
                            int position = Math.Abs((i + 1) % buttonUpperLimit);
                            if (position == 0)
                                position = 15;
                            temp = buttons[position].getID();
                        }
                    }

                    for (int i = buttonLowerLimit; i < buttonUpperLimit; ++i)
                    {
                        if (buttons[i].getID() == temp)
                        {
                            buttons[i].setSelected(true);
                        }
                    }
                }

                if ((gamepad.IsButtonUp(Buttons.DPadDown) && downButtonDown4))
                {
                    downButtonDown4 = false;
                }

                if (gamepad.IsButtonDown(Buttons.Start) && this.title.Equals("Character Menu"))
                {
                    player4Chosen = true;
                    Game1.StartButtonDown4 = true;
                }
            }
            #endregion

            if (this.title.Equals("Character Menu") && player1Chosen && player2Chosen && player3Chosen && player4Chosen)
                allPlayersChose = true;

            base.Update(keyboard, gamepad, playerIndex);
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
