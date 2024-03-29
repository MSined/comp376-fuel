﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace F.U.E.L
{
    class Character : Object
    {
        public int topHP;
        public int hp;// { get; protected set; }
        public int topSP;
        public int sp;// { get; protected set; }

        private float spRecoverTimer = 0f;
        private float spRecoverInterval = 1000f;
        private int spRecoverRate = 5;

        public SpawnPoint spawnPoint { get; private set; }
        public Weapon[] weapons { get; private set; }
        public int selectedWeapon { get; protected set; }
        protected int[] attributes { get; private set; }

        public Vector3 lookDirection = new Vector3(1, 0, 0);
        public Vector3 velocity = new Vector3(0,0,0);
        public float speed;

        public Boolean poisoned = false, checkBoxCollision = false;
        public int burningStacks = 0;
        public BuildBox checkBox;
        public bool wasUpdated = false;

        // Characters initial position is defined by the spawnpoint ther are associated with
        public Character(Game game, Model[] modelComponents, Vector3 position,
            int topHP, int topSP, float speed, SpawnPoint spawnPoint, Weapon[] weapons, FloatRectangle bounds, bool isAlive)
            : base(game, modelComponents, position, bounds, isAlive)
        {
            this.topHP = topHP;
            this.hp = topHP;

            this.topSP = topSP;
            this.sp = topSP;

            this.speed = speed;

            this.spawnPoint = spawnPoint;
            this.weapons = weapons;
            this.selectedWeapon = 0;
        }

        public virtual void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget, List<Waypoint> waypointList)
        {
            lookDirection.Normalize();

            float angle = (float) Math.Asin(lookDirection.X) + MathHelper.ToRadians(180);
            if (lookDirection.Z > 0)
            {
                angle = MathHelper.ToRadians(180) - angle;
            }

            if (this is Tower) angle += MathHelper.ToRadians(90);

            world = Matrix.CreateRotationY(-angle) * Matrix.CreateTranslation(position);

            // Don't perform this block of code if object is a tower
            // towers do not need a velocity, or check for collisions, or check if dead, or check for sp recovery
            if (!(this is Tower))
            {
                if (!(velocity.X == 0 && velocity.Y == 0 && velocity.Z == 0))
                {
                    velocity.Normalize();
                    position += speed * velocity;
                    this.bounds = new FloatRectangle(position.X, position.Z, this.bounds.Width, this.bounds.Height);
                }
                //check collisions after moved
                CheckCollisions(colliders);

                checkIfDead(cameraTarget);

                if (this is Enemy)
                    checkBox.Update(gameTime, colliders, cameraTarget);

                spRecoverTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (sp < topSP && spRecoverTimer > spRecoverInterval)
                {
                    sp += spRecoverRate;
                    spRecoverTimer = 0;
                }
            }

            //base.Update(gameTime, colliders, cameraTarget, waypointList);
        }

        public void checkIfDead(Vector3 cameraTarget)
        {
            if (hp <= 0)
            {
                // If current character is the player and it died
                // Return it to the spawnpoint
                if (this is Player)
                {
                    Player p = (Player)this;
                    this.isAlive = false;
                    this.velocity = Vector3.Zero;
                }
                else if (this is Enemy)//if enemy got killed, the target's attackerNum -1
                {
                    Enemy e = (Enemy)this;
                    if (e.target is Player)
                    {
                        Player p = (Player)e.target;
                        --p.attackerNum;
                    }
                    else if (e.target is Tower)
                    {
                        Tower t = (Tower)e.target;
                        --t.attackerNum;
                    }
                    isAlive = false;
                    Player.credit += 100;//cash
                }
                else if (this is Tower)
                {
                    isAlive = false;
                    Tower t = (Tower)this;
                    t.playSound(position, cameraTarget);
                    --Tower.numTowers;
                }
                // Otherwise kill it!
                else
                    isAlive = false;
            }
        }

        public override void Draw(Camera camera)
        {
            // Required to stop drawing players that are dead and should not be drawn
            if (this.isAlive)
            {
                Matrix[] transforms = new Matrix[modelComponents[0].Bones.Count];
                modelComponents[0].CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in modelComponents[0].Meshes)
                {
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.SpecularPower = 10f;
                        be.Projection = camera.projection;
                        be.View = camera.view;
                        be.World = world * mesh.ParentBone.Transform;
                    }
                    mesh.Draw();
                }
            }
        }

        public override void drawHealth(Camera camera, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Texture2D healthTexture)
        {
            if (this.isAlive)
            {
                int healthBarWidth = 20;
                int healthBarHeight = 5;
                Rectangle srcRect, destRect;

                Vector3 screenPos = graphicsDevice.Viewport.Project(this.position + new Vector3(0, 0.8f, 0), camera.projection, camera.view, Matrix.Identity);

                srcRect = new Rectangle(0, 0, 1, 1);
                destRect = new Rectangle((int)screenPos.X - healthBarWidth / 2, (int)screenPos.Y, healthBarWidth, healthBarHeight);
                spriteBatch.Draw(healthTexture, destRect, srcRect, Color.LightGray, 0f, Vector2.Zero, SpriteEffects.None, 0.81f);

                float healthPercentage = (float)hp / (float)topHP;

                Color healthColor = new Color(new Vector3(1 - healthPercentage, healthPercentage, 0));

                srcRect = new Rectangle(0, 0, 1, 1);
                destRect = new Rectangle((int)screenPos.X - healthBarWidth / 2, (int)screenPos.Y, (int)(healthPercentage * healthBarWidth), healthBarHeight);
                spriteBatch.Draw(healthTexture, destRect, srcRect, healthColor, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            }
        }

        //collision vs buildings/ all bullet collisions are in Bullet
        public void CheckCollisions(List<Object> colliders)
        {
            foreach (Object o in colliders)
            {
                if (bounds.FloatIntersects(o.bounds))
                {
                    if (o is Building || o is Tower || o is Enemy || o is Generator || o is Player)
                    {
                        //neutralize the Z movement if going in a collision by moving up/down
                        if (bounds.CenterX > o.bounds.Left && bounds.CenterX < o.bounds.Right)
                        {
                            position -= speed * new Vector3 (0,0,velocity.Z);
                        }
                        //neutralize the X movement if going in a collision by moving left/right
                        if (bounds.CenterY < o.bounds.Top && bounds.CenterY > o.bounds.Bottom)
                        {
                            position -= speed * new Vector3(velocity.X, 0, 0);
                        }
                        
                        //update bounds again to make sure Character does not get stuck
                        this.bounds = new FloatRectangle(position.X, position.Z, this.bounds.Width, this.bounds.Height);
                        if (bounds.FloatIntersects(o.bounds)) {//push against the building
                            Vector3 moveBack = position - o.position;
                            moveBack.Normalize();
                            position += moveBack * speed;
                        }
                        else if (this is Tower) {//tower do not move after moved to empty space
                            this.speed = 0;
                        }
                    }
                }
            }
        }
    }
}
