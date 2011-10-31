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
        protected int sp;
        protected Vector2 spawnPoint;
        protected Weapon[] weapons;
        protected int[] attributes;

        protected float lookAngle;
        protected float speed;

        public Player() { }
        public Player(int sp, Vector2 spawnPoint, Weapon[] weapons, int[] attributes, float speed,
            int hp, Vector2 lookDirection,
            Model[] components, Vector3 position, Vector2 velocity) 
        {
            this.sp = sp;
            this.spawnPoint = spawnPoint;
            this.weapons = weapons;
            this.attributes = attributes;
            this.speed = speed;
            base.hp = hp;
            base.lookDirection = lookDirection;
            base.components = components;
            base.position = position;
            base.velocity = velocity;
        }

        public int getSp() { return sp; }
        public Vector2 getSpawnPoint() { return spawnPoint; }
        public Weapon[] getWeapons() { return weapons; }
        public int[] getAttributes() { return attributes; }

        public void setSp(int sp) { this.sp = sp; }
        public void setSpawnPoint(Vector2 spawnPoint) { this.spawnPoint = spawnPoint; }
        public void setWeapons(Weapon[] weapons) { this.weapons = weapons; }
        public void setAttributes(int[] attributes) { this.attributes = attributes; }

        public void playerUpdate(GamePadThumbSticks thumbSticks) 
        {
            lookDirection = thumbSticks.Right;
            velocity = thumbSticks.Left;
            if (lookDirection.X > 0)
            {
                lookAngle = (float)Math.Atan(lookDirection.Y / lookDirection.X);
            }
            else 
            {
                lookAngle = (float)Math.Atan(lookDirection.Y / lookDirection.X) + MathHelper.ToRadians(180);
            }
            position.X += velocity.X;
            position.Y += velocity.Y;
        }
    }
}
