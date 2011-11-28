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
        public enum Class
        {
            Alchemist,
            Gunner,
            Sniper,
            Tank
        };

        const float height = .5f;
        const float width = .5f;
        const float depth = .5f;
        const float useRange = 1f;

        public static int credit = 99990;

        public bool placingTower = false, checkBoxCollision = false;
        public BuildBox checkBox;
        public int attackerNum=0;

        private PlayerIndex playerIndex;
        private int playerID;

        public int respawnCost = 500;

        public Player(Game game, Model[] modelComponents,
            SpawnPoint spawnPoint, Class c, PlayerIndex pIndex
            )
            : base(game, modelComponents, spawnPoint.position, 10, 10, 10, spawnPoint, new Weapon[4], new FloatRectangle(spawnPoint.position.X, spawnPoint.position.Z, width, depth), true)
        {

            playerIndex = pIndex;
            switch (pIndex)
            {
                case PlayerIndex.One:
                    playerID = 0;
                    break;
                case PlayerIndex.Two:
                    playerID = 1;
                    break;
                case PlayerIndex.Three:
                    playerID = 2;
                    break;
                case PlayerIndex.Four:
                    playerID = 3;
                    break;
            }

            switch (c)
            {
                case Class.Alchemist:
                    weapons[0] = new Pistol(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[1] = new Heal(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[2] = new Renew(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[3] = new PoisonRing(game, modelComponents, new Vector3(0, 0, 0));
                    topHP = 350;
                    hp = topHP;
                    topSP = 250;
                    sp = topSP;
                    speed = 0.08f;
                    break;
                case Class.Gunner:
                    weapons[0] = new MiniGun(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[1] = new FlameThrower(game, modelComponents, new Vector3(0, 0, 0), this);
                    weapons[2] = new Stimpack(game, modelComponents, new Vector3(0, 0, 0), this);
                    weapons[3] = new RocketLauncher(game, modelComponents, new Vector3(0, 0, 0));
                    topHP = 500;
                    hp = topHP;
                    topSP = 200;
                    sp = topSP;
                    speed = 0.08f;
                    break;
                case Class.Sniper:
                    weapons[0] = new Sniper(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[1] = new HeadShot(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[2] = new PiercingShot(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[3] = new AirStrike(game, modelComponents, new Vector3(0, 0, 0));
                    topHP = 400;
                    hp = topHP;
                    topSP = 200;
                    sp = topSP;
                    speed = 0.08f;
                    break;
                case Class.Tank:
                    weapons[0] = new Pistol(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[1] = new Pistol(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[2] = new Pistol(game, modelComponents, new Vector3(0, 0, 0));
                    weapons[3] = new Pistol(game, modelComponents, new Vector3(0, 0, 0));
                    topHP = 600;
                    hp = topHP;
                    topSP = 100;
                    sp = topSP;
                    speed = 0.08f;
                    break;
            }
            

            selectedWeapon = 1;
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            #region Keyboard Controls
            //Hack to get it working on a computer
            KeyboardState k = Keyboard.GetState();
            if (playerIndex == PlayerIndex.One)
            {
                if (this.isAlive)
                {
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

                    if (k.IsKeyDown(Keys.R) && this.getUsableBuilding() is Generator)
                    {
                        Generator g = (Generator)this.getUsableBuilding();
                        g.use();
                    }
                    else if (k.IsKeyDown(Keys.Space))
                    {
                        weapons[selectedWeapon].shoot(position, lookDirection, false);
                    }

                    if (k.IsKeyDown(Keys.D1))
                        selectedWeapon = 0;

                    if (k.IsKeyDown(Keys.D2))
                        selectedWeapon = 1;

                    if (k.IsKeyDown(Keys.D3))
                        selectedWeapon = 2;

                    if (k.IsKeyDown(Keys.D4))
                        selectedWeapon = 3;

                    //Doesn't need cooldown, fixed the tower spamming to a single button press
                    if (k.IsKeyDown(Keys.T))
                    {
                        placingTower = true;
                        checkBox.Update(gameTime);
                        foreach (Object o in colliders)
                        {
                            if (checkBox.bounds.FloatIntersects(o.bounds))
                            {
                                checkBoxCollision = true;
                                break;
                            }
                            checkBoxCollision = false;
                        }
                    }
                    if (k.IsKeyUp(Keys.T) && placingTower)
                    {
                        if (!checkBoxCollision && credit >= Tower.towerCost)
                        {
                            Weapon[] w = new Weapon[1];
                            w[0] = new Shotgun(game, modelComponents, new Vector3(0, 0, 0));
                            credit -= Tower.towerCost;
                            game.Components.Add(new Tower(game, modelComponents, 200, 0, position + lookDirection, spawnPoint, w));
                        }
                        placingTower = false;
                        checkBoxCollision = false;
                    }
                }

                else
                {
                    if (k.IsKeyDown(Keys.Enter) && credit >= respawnCost)
                    {
                        credit -= respawnCost;
                        respawnCost *= 2;
                        this.isAlive = true;
                        this.hp = this.topHP;
                        this.position = this.spawnPoint.position;
                        this.attackerNum = 0;
                    }
                }
            }
            #endregion

            #region Gamepad Support
            GamePadState gp = GamePad.GetState(playerIndex);
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
            if (gp.Triggers.Right > 0) weapons[0].shoot(position, lookDirection, false);

            //velocity = new Vector3(gp.ThumbSticks.Left.X, 0, -gp.ThumbSticks.Left.Y);
            
            #endregion
            
            foreach (Weapon w in weapons)
            {
                w.Update(gameTime, colliders);
            }

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
                    buildings = m.usableBuildings;
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

        public override void drawHealth(Camera camera, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Texture2D healthTexture, int width, int height)
        {
            if (this.isAlive)
            {
                int healthBarWidth = (int)Math.Floor((145f * width / (float)1000));
                int healthBarHeight = (int)Math.Floor((22f * width / (float)1000));
                Rectangle srcRect, destRect;

                Vector3 screenPos = graphicsDevice.Viewport.Project(this.position + new Vector3(0, 0.8f, 0), camera.projection, camera.view, Matrix.Identity);
                /*
                srcRect = new Rectangle(0, 0, 1, 1);
                destRect = new Rectangle((int)Math.Floor((float)203 / (float)1000 * width), (height - (int)(width / 1000f * 200f) + (int)Math.Floor((153f / 200f) * (width / 1000f * 200f))), healthBarWidth, healthBarHeight);
                spriteBatch.Draw(healthTexture, destRect, srcRect, Color.Black);
                */
                float healthPercentage = (float)hp / (float)topHP;

                Color healthColor = new Color(new Vector3(1 - healthPercentage, healthPercentage, 0));

                srcRect = new Rectangle(0, 0, 1, 1);
                destRect = new Rectangle((int)Math.Floor((float)(253+(playerID*200)) / (float)1000 * width), (height - (int)(width / 1000f * 200f) + (int)Math.Floor((153f / 200f) * (width / 1000f * 200f))), (int)(healthPercentage * healthBarWidth), healthBarHeight);
                spriteBatch.Draw(healthTexture, destRect, Rectangle.Empty, healthColor);
            }
        }

        public override void Draw(Camera camera)
        {
            if (placingTower)
            {
                checkBox.Draw(camera);
            }

            base.Draw(camera);
        }
    }
}
