using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace F.U.E.L
{
    class Player : Character
    {
        const float height = .5f;
        const float width = .5f;
        const float depth = .5f;
        const float useRange = 1f;

        bool placedTower = false;
        FloatRectangle checkBox;

        public Player(Game game, Model[] modelComponents,
            int topHP, int topSP, float speed, SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, spawnPoint.position, topHP, topSP, speed, spawnPoint, weapons, new FloatRectangle(spawnPoint.position.X, spawnPoint.position.Z, width, depth), true)
        {
            selectedWeapon = 1;
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            #region Keyboard Controls
            //Hack to get it working on a computer
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Up) || k.IsKeyDown(Keys.Down) || k.IsKeyDown(Keys.Left) || k.IsKeyDown(Keys.Right))
            {
                lookDirection = new Vector3(0, 0, 0);
                if (k.IsKeyDown(Keys.Up))
                    lookDirection += new Vector3(0, 0, -1);

                else if (k.IsKeyDown(Keys.Down))
                    lookDirection += new Vector3(0, 0, 1);

                if (k.IsKeyDown(Keys.Left))
                    lookDirection += new Vector3(-1, 0, 0);

                else if (k.IsKeyDown(Keys.Right))
                    lookDirection += new Vector3(1, 0, 0);
            }

            velocity = new Vector3(0, 0, 0);
            if (k.IsKeyDown(Keys.W) || k.IsKeyDown(Keys.S) || k.IsKeyDown(Keys.A) || k.IsKeyDown(Keys.D))
            {
                if (k.IsKeyDown(Keys.W))
                    velocity += new Vector3(0, 0, -1);

                else if (k.IsKeyDown(Keys.S))
                    velocity += new Vector3(0, 0, 1);

                if (k.IsKeyDown(Keys.A))
                    velocity += new Vector3(-1, 0, 0);

                else if (k.IsKeyDown(Keys.D))
                    velocity += new Vector3(1, 0, 0);
            }

            if (k.IsKeyDown(Keys.Space))
                weapons[selectedWeapon].shoot(position, lookDirection, false);

            if (k.IsKeyDown(Keys.D1))
                selectedWeapon = 0;

            if (k.IsKeyDown(Keys.D2))
                selectedWeapon = 1;

            if (k.IsKeyDown(Keys.D3))
                selectedWeapon = 2;

            if (k.IsKeyDown(Keys.D4))
                selectedWeapon = 3;

            //Doesn't need cooldown, fixed the tower spamming to a single button press
            if(k.IsKeyDown(Keys.T))
            {
                if (!placedTower)
                {
                    placedTower = true;
                    checkBox = new FloatRectangle((position + lookDirection).X, (position + lookDirection).Z, 1, 1);//width and height of Tower is .5, 1 to make sure there are place 0.5 place from other objects
                    foreach (Object o in colliders)
                    {
                        if (checkBox.FloatIntersects(o.bounds))
                        {
                            placedTower = false;
                            break;
                        }
                    } 
                }
            }
            if (k.IsKeyUp(Keys.T) && placedTower)
            {
                Weapon[] w = new Weapon[1];
                w[0] = new Pistol(game, modelComponents, new Vector3(0, 0, 0));
                game.Components.Add(new Tower(game, modelComponents, 100, 0, position + lookDirection, spawnPoint, w));
                placedTower = false;
                checkBox=null;
            }

            #endregion

            //Gamepad Support
            GamePadState gp = GamePad.GetState(PlayerIndex.One);
            if (!(gp.ThumbSticks.Right.X == 0 && gp.ThumbSticks.Right.Y == 0)) lookDirection = new Vector3(gp.ThumbSticks.Right.X, 0, -gp.ThumbSticks.Right.Y);

            if (gp.IsButtonDown(Buttons.X))
                selectedWeapon = 1;
            if (gp.IsButtonDown(Buttons.Y))
                selectedWeapon = 2;
            if (gp.IsButtonDown(Buttons.B))
                selectedWeapon = 3;

            if (gp.IsButtonDown(Buttons.A))
            {
                Building b = getUsableBuilding();
                if (b != null) b.use();
            }
            
            if (gp.IsButtonDown(Buttons.LeftShoulder)) { }
                //Recover EP
            if (gp.IsButtonDown(Buttons.RightShoulder)) { }
                //Recover HP

            if (gp.Triggers.Left > 0) weapons[selectedWeapon].shoot(position, lookDirection, false);
            if (gp.Triggers.Right > 0) weapons[0].shoot(position,lookDirection, false);

            //velocity = new Vector3(gp.ThumbSticks.Left.X, 0, -gp.ThumbSticks.Left.Y);

            base.Update(gameTime, colliders);
        }

        protected Building getUsableBuilding()
        {
            List<Building> buildings = new List<Building>();
            Building target = null;

            foreach (GameComponent gc in game.Components)
            {
                if (gc is Map)
                {
                    Map m = (Map)gc;
                    buildings = m.buildings;
                }
            }

            float distance = float.PositiveInfinity;
            foreach (Building b in buildings)
            {
                if ((b.position - this.position).Length() < distance)
                {
                    distance = (b.position - this.position).Length();
                    target = b;
                }
            }

            if (distance > useRange) target = null;

            return target;
        }

    }
}
