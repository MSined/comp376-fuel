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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace F.U.E.L
{
    class MenuManager
    {
        public enum MenuEvents
        {
            None,
            Exit
        }public MenuEvents MenuEvent { get; set; }  //Events to feed back to Game1.cs

        Dictionary<string, Menu> Menus;     //A container for all our menus
        Menu activeMenu;                    //Which menu is currently active
        Stack<Menu> previousMenus;          //An ordered queue of previous menus.
        MouseState mouse;
        KeyboardState keyboard;

        public Menu ActiveMenu
        {
            get { return activeMenu; }
        }

        public MenuManager(MouseState mouse, ref KeyboardState keyboard)
        {
            Menus = new Dictionary<string, Menu>();
            previousMenus = new Stack<Menu>();
            this.mouse = mouse;
            this.keyboard = keyboard;
        }

        public void Update(KeyboardState k, GamePadState g, int playerIndex)
        {
            if (activeMenu != null)
                activeMenu.Update(k, g, playerIndex);

            GetButtonEvent();
        }

        public void Draw(SpriteBatch spriteBatch, int height, int width)
        {
            if (activeMenu != null)
                activeMenu.Draw(spriteBatch, height, width);
        }

        /// <summary>
        /// This method adds a new menu to storage
        /// </summary>
        public void AddMenu(string name, Menu menu)
        {
            Menus.Add(name, menu);
        }

        /// <summary>
        /// Puts any active menu item storage
        /// and then opens the new menu.
        /// </summary>
        /// <param name="name">Name of menu</param>
        public void Show(string name)
        {
            if (Menus.ContainsKey(name))
            {
                if (activeMenu == null)
                {
                    //First menu open, so we play bgm.
                    Menus[name].PlayBGM(true);

                    //Sets the active menu based on name
                    //then performs menus open method.
                    activeMenu = Menus[name];
                    activeMenu.Open(true);
                }
                else
                {
                    //Perform closing actions of menu then
                    //puts current menu in stack of hidden.
                    activeMenu.Close(false);
                    previousMenus.Push(activeMenu);

                    //Sets the active menu based on name
                    //then performs menus open method.
                    activeMenu = Menus[name];
                    activeMenu.Open(true);
                }
            }
            else
                return;
        }

        /// <summary>
        /// Closes the current menu then re-opens
        /// last menu, if any.
        /// </summary>
        public void Close()
        {
            activeMenu.Close(true);
            if (previousMenus.Count() > 0)
            {
                activeMenu = previousMenus.Pop();
            }
            else
            {
                activeMenu.StopBGM();
                activeMenu = null;
            }
        }

        /// <summary>
        /// Closes all menus, clears the storage
        /// then will be returned to game.
        /// </summary>
        public void Exit()
        {
            activeMenu.StopBGM();
            previousMenus.Clear();
            activeMenu.Close(true);
            activeMenu = null;
        }

        public void GetButtonEvent()
        {
            if (activeMenu != null)
            {
                switch (activeMenu.ButtonEvent)
                {
                    case Menu.ButtonEvents.None:
                        break;
                    case Menu.ButtonEvents.Close:
                        activeMenu.ButtonEvent = Menu.ButtonEvents.None;
                        Close();
                        break;
                    case Menu.ButtonEvents.Quit:
                        activeMenu.ButtonEvent = Menu.ButtonEvents.None;
                        MenuEvent = MenuEvents.Exit;
                        break;
                    case Menu.ButtonEvents.Save:
                        activeMenu.ButtonEvent = Menu.ButtonEvents.None;
                        Show("Save Menu");
                        break;
                    case Menu.ButtonEvents.Go_Deeper:
                        activeMenu.ButtonEvent = Menu.ButtonEvents.None;
                        Show("Deep Menu");
                        break;
                }
            }
        }
    }
}
