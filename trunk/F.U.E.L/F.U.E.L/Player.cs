using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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

        protected SoundEffect soundEffectTowerPlaced;
        protected SoundEffect soundEffectWeaponSwitch;

        public static int credit = 500;
        public static int respawnCost = 500;
        
        public bool placingTower = false, switching = false;
        public int attackerNum=0;

        private PlayerIndex playerIndex;
        public int playerID;
        public int playerClass;//for skill icons

        Model[] towerModel = new Model[1];
        Model[] bulletModel = new Model[1];
        Texture2D texture;

        // Debugging
        //string waypointText = "";
        //System.IO.StreamWriter file = new System.IO.StreamWriter("Waypoints3.txt");
        //bool lDown = false;
        //bool wDown = false, sDown = false, dDown = false, aDown = false;
        //List<Tower> testList = new List<Tower>();
        
        public Player(Game game, Model[] modelComponents,
            SpawnPoint spawnPoint, Class c, PlayerIndex pIndex, Texture2D texture
            )
            : base(game, modelComponents, spawnPoint.position, 10, 10, 8, spawnPoint, new Weapon[4], new FloatRectangle(spawnPoint.position.X, spawnPoint.position.Z, width, depth), true)
        {
            soundEffectTowerPlaced = game.Content.Load<SoundEffect>(@"Sounds/towerplaced");
            soundEffectWeaponSwitch = game.Content.Load<SoundEffect>(@"Sounds/weaponswitch");

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

            Model[] bulletModelComponent = new Model[1];
            bulletModelComponent[0] = modelComponents[2];

            Model[] fireBulletModelComponent = new Model[1];
            fireBulletModelComponent[0] = modelComponents[3];

            Model[] poisonBulletModelComponent = new Model[1];
            poisonBulletModelComponent[0] = modelComponents[4];

            Model[] bigBulletModelComponent = new Model[1];
            bigBulletModelComponent[0] = modelComponents[5];

            Model[] mineBulletModelComponent = new Model[1];
            mineBulletModelComponent[0] = modelComponents[6];

            Model[] grenadeBulletModelComponent = new Model[2];
            grenadeBulletModelComponent[0] = modelComponents[6];
            grenadeBulletModelComponent[1] = modelComponents[3];

            switch (c)
            {
                case Class.Alchemist:
                    weapons[0] = new AssaultRifle(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[1] = new Heal(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[2] = new Renew(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[3] = new PoisonRing(game, poisonBulletModelComponent, new Vector3(0, 0, 0));
                    topHP = 350;
                    hp = topHP;
                    topSP = 250;
                    sp = topSP;
                    speed = 0.08f;
                    playerClass = 1;
                    break;
                case Class.Gunner:
                    weapons[0] = new MiniGun(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[1] = new FlameThrower(game, fireBulletModelComponent, new Vector3(0, 0, 0), this);
                    weapons[2] = new Mines(game, mineBulletModelComponent, new Vector3(0, 0, 0));
                    weapons[3] = new RocketLauncher(game, bigBulletModelComponent, new Vector3(0, 0, 0));
                    topHP = 500;
                    hp = topHP;
                    topSP = 200;
                    sp = topSP;
                    speed = 0.08f;
                    playerClass = 2;
                    break;
                case Class.Sniper:
                    weapons[0] = new Sniper(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[1] = new HeadShot(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[2] = new PiercingShot(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[3] = new AirStrike(game, fireBulletModelComponent, new Vector3(0, 0, 0));
                    topHP = 400;
                    hp = topHP;
                    topSP = 200;
                    sp = topSP;
                    speed = 0.08f;
                    playerClass = 3;
                    break;
                case Class.Tank:
                    weapons[0] = new Shotgun(game, bulletModelComponent, new Vector3(0, 0, 0));
                    weapons[1] = new Grenade(game, grenadeBulletModelComponent, new Vector3(0, 0, 0));
                    weapons[2] = new SpeedBoost(game, bulletModelComponent, new Vector3(0, 0, 0), this);
                    weapons[3] = new GroundPound(game, bigBulletModelComponent, new Vector3(0, 0, 0));
                    topHP = 650;
                    hp = topHP;
                    topSP = 150;
                    sp = topSP;
                    speed = 0.08f;
                    playerClass = 4;
                    break;
            }
            
            selectedWeapon = 1;

            towerModel[0] = modelComponents[1];
            bulletModel[0] = bulletModelComponent[0];

            this.texture = texture;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget, List<Waypoint> waypointList)
        {
            
            #region Keyboard Controls
            //Hack to get it working on a computer
            KeyboardControls(gameTime, colliders, cameraTarget);
            #endregion

            #region Gamepad Support
            //GamePadControls(gameTime, colliders, cameraTarget);
            #endregion
            
            foreach (Weapon w in weapons)
            {
                w.Update(gameTime, colliders, cameraTarget);
            }

            base.Update(gameTime, colliders, cameraTarget, waypointList);

            if (this.position.X > cameraTarget.X + 9)
            {
                position -= speed * Vector3.UnitX;
            }
            else if (this.position.X < cameraTarget.X - 9) //left
            {
                position += speed * Vector3.UnitX;
            }
            if (this.position.Z > cameraTarget.Z + 6)
            {
                position -= speed * Vector3.UnitZ;
            }
            else if (this.position.Z < cameraTarget.Z - 6) //up
            {
                position += speed * Vector3.UnitZ;
            }
        }

        private void KeyboardControls(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget) 
        {
            KeyboardState k = Keyboard.GetState();
            if (playerIndex == PlayerIndex.One)
            {
                if (this.isAlive)
                {
                    if (k.IsKeyDown(Keys.O))
                        Game1.endGameSwarm = true;
                    /*
                    if (k.IsKeyDown(Keys.L) && !lDown)
                    {
                        lDown = true;
                        waypointText = "(" + ((int)this.position.X).ToString() + "," + ((int)this.position.Y).ToString() + "," + ((int)this.position.Z).ToString() + ")";
                        file.WriteLine(waypointText);
                        testList.Add(new Tower(this.game, this.towerModel, 10, 10, this.position, spawnPoint, this.weapons));
                    }
                    else if (k.IsKeyUp(Keys.L))
                    {
                        lDown = false;
                    }

                    if (k.IsKeyDown(Keys.F12))
                    {
                        file.Close();
                    }
                    */

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
                        if (k.IsKeyDown(Keys.W))// && !wDown)
                        {
                            velocity += new Vector3(0, 0, -1);
                            //position += new Vector3(0, 0, -1);
                            //wDown = true;
                        }
                        //else if (k.IsKeyUp(Keys.W))
                        //    wDown = false;

                        if (k.IsKeyDown(Keys.S))// && !sDown)
                        {
                            velocity += new Vector3(0, 0, 1);
                            //position += new Vector3(0, 0, 1);
                            //sDown = true;
                        }
                        //else if (k.IsKeyUp(Keys.S))
                        //    sDown = false;

                        if (k.IsKeyDown(Keys.A))// && !aDown)
                        {
                            velocity += new Vector3(-1, 0, 0);
                            //position += new Vector3(-1, 0, 0);
                            //aDown = true;
                        }
                        //else if (k.IsKeyUp(Keys.A))
                        //    aDown = false;

                        if (k.IsKeyDown(Keys.D))// && !dDown)
                        {
                            velocity += new Vector3(1, 0, 0);
                            //position += new Vector3(1, 0, 0);
                            //dDown = true;
                        }
                        //else if (k.IsKeyUp(Keys.D))
                        //    dDown = false;
                    }

                    if (k.IsKeyDown(Keys.R) && this.getUsableBuilding() is Generator)
                    {
                        Generator g = (Generator)this.getUsableBuilding();
                        g.use();
                    }
                    else if (k.IsKeyDown(Keys.Space))
                    {
                        weapons[selectedWeapon].shoot(position, lookDirection, false, gameTime, cameraTarget);
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
                        checkBox.isAlive = true;
                    }
                    else if (k.IsKeyUp(Keys.T) && placingTower)
                    {
                        if (!checkBoxCollision && credit >= Tower.towerCost)
                        {
                            credit -= Tower.towerCost;
                            Weapon[] towerWeapon = new Weapon[1];
                            towerWeapon[0] = new AssaultRifle(game, bulletModel, new Vector3(0, 0, 0));
                            game.Components.Add(new Tower(game, towerModel, 200, 0, position + lookDirection, spawnPoint, towerWeapon));
                        }
                        placingTower = false;
                        checkBoxCollision = false;
                        checkBox.isAlive = false;
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
        }

        private void GamePadControls(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget) 
        {
            GamePadState gp = GamePad.GetState(playerIndex);
            if (this.isAlive)
            {
                if (!(gp.ThumbSticks.Right.X == 0 && gp.ThumbSticks.Right.Y == 0))
                {
                    lookDirection = new Vector3(gp.ThumbSticks.Right.X, 0, -gp.ThumbSticks.Right.Y);
                    lookDirection.Normalize();
                }

                if (gp.IsButtonDown(Buttons.X))
                {
                    placingTower = true;
                    checkBox.isAlive = true;
                }
                else if (gp.IsButtonUp(Buttons.X) && placingTower)
                {
                    if (!checkBoxCollision && credit >= Tower.towerCost)
                    {
                        credit -= Tower.towerCost;
                        Weapon[] towerWeapon = new Weapon[1];
                        towerWeapon[0] = new AssaultRifle(game, bulletModel, new Vector3(0, 0, 0));
                        game.Components.Add(new Tower(game, towerModel, 200, 0, position + lookDirection, spawnPoint, towerWeapon));
                    }
                    placingTower = false;
                    checkBoxCollision = false;
                    checkBox.isAlive = false;
                }

                if (gp.IsButtonDown(Buttons.LeftShoulder) && !switching)
                {
                    switching = true;
                    --selectedWeapon;
                    if (selectedWeapon == 0)
                    {
                        selectedWeapon = 3;// specials are 1-3
                    }
                    playSoundWeaponSwitch(position, cameraTarget);
                }
                else if (gp.IsButtonDown(Buttons.RightShoulder) && !switching)
                {
                    switching = true;
                    ++selectedWeapon;
                    if (selectedWeapon == 4)
                    {
                        selectedWeapon = 1;// specials are 1-3
                    }
                    playSoundWeaponSwitch(position, cameraTarget);
                }
                else if (switching && gp.IsButtonUp(Buttons.LeftShoulder) && gp.IsButtonUp(Buttons.RightShoulder))
                { switching = false; }

                if (gp.Triggers.Left > 0 && sp>=weapons[selectedWeapon].spCost) 
                {
                    if (weapons[selectedWeapon].interval > weapons[selectedWeapon].fireDelay)//no spCost if can't shoot
                    {sp -= weapons[selectedWeapon].spCost;}
                    weapons[selectedWeapon].shoot(position, lookDirection, false, gameTime, cameraTarget);
                    
                }
                if (gp.Triggers.Right > 0)
                {
                    weapons[0].shoot(position, lookDirection, false, gameTime, cameraTarget);
                }
                else if (gp.IsButtonDown(Buttons.A))//cannot repair if shooting
                {
                    Building b = getUsableBuilding();
                    if (b != null) b.use();
                }

                velocity = new Vector3(gp.ThumbSticks.Left.X, 0, -gp.ThumbSticks.Left.Y);
            }
            else
            {
                if (gp.IsButtonDown(Buttons.Y) && credit >= respawnCost &&
                    this.spawnPoint.position.X < cameraTarget.X + 9 &&
                    this.spawnPoint.position.X > cameraTarget.X - 9 &&
                    this.spawnPoint.position.Z < cameraTarget.Z + 6 &&
                    this.spawnPoint.position.Z > cameraTarget.Z - 6)
                {
                    credit -= respawnCost;
                    respawnCost *= 2;
                    this.isAlive = true;
                    this.hp = this.topHP;
                    this.sp = this.topSP;
                    this.position = this.spawnPoint.position;
                    this.attackerNum = 0;
                }
            }
        }

        public void playSoundTowerPlaced(Vector3 position, Vector3 cameraTarget)
        {
            float dist = (cameraTarget - position).LengthSquared();
            float vol = dist / 300;
            float scaledVol = (vol >= 1 ? 0 : (1 - vol));
            soundEffectTowerPlaced.Play(scaledVol, 0.0f, 0.0f);
        }

        public void playSoundWeaponSwitch(Vector3 position, Vector3 cameraTarget)
        {
            float dist = (cameraTarget - position).LengthSquared();
            float vol = dist / 300;
            float scaledVol = (vol >= 1 ? 0 : (1 - vol));
            soundEffectWeaponSwitch.Play(scaledVol, 0.0f, 0.0f);
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
                Rectangle srcRect, destRect, spRect;

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
                spriteBatch.Draw(healthTexture, destRect, Rectangle.Empty, healthColor,0f,Vector2.Zero,SpriteEffects.None,0.3f);
                
                float spPercentage = (float)sp / (float)topSP;
                spRect = new Rectangle((int)Math.Floor((float)(253 + (playerID * 200)) / (float)1000 * width), (height - (int)(width / 1000f * 200f) + (int)Math.Floor((176f / 200f) * (width / 1000f * 200f))), (int)(spPercentage * healthBarWidth), healthBarHeight);
                spriteBatch.Draw(healthTexture, spRect, Rectangle.Empty, Color.Blue, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            }
        }

        public override void Draw(Camera camera)
        {
            if (placingTower)
            {
                checkBox.Draw(camera);
            }

            if (this.isAlive)
            {
                Matrix[] transforms = new Matrix[modelComponents[0].Bones.Count];
                modelComponents[0].CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in modelComponents[0].Meshes)
                {
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.TextureEnabled = true;
                        be.Texture = texture;
                        be.SpecularPower = 10f;
                        be.Projection = camera.projection;
                        be.View = camera.view;
                        be.World = world * mesh.ParentBone.Transform;
                    }
                    mesh.Draw();
                }
            }

            /* REMOVE AFTER DEBUGGING */
            //foreach (Tower t in testList)
            //{
            //    t.Draw(camera);
            //}
        }
    }
}
