using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Enemy : Player
    {
        private Character target;
        private float atkRange;
        private int atk;//damage deal
        private float atkTimer;
        private float atkInterval;

        public Enemy() { }
        public Enemy(int atk, float atkRange, float atkInterval,
            int sp, Vector2 spawnPoint, Weapon[] weapons, int[] attributes, float speed,
            int hp, Vector2 lookDirection,
            Model[] components, Vector3 position, Vector2 velocity) 
        {
            this.atk = atk;
            this.atkRange = atkRange;
            this.atkInterval = atkInterval;
            atkTimer = 0;

            base.sp = sp;
            base.spawnPoint = spawnPoint;
            base.weapons = weapons;
            base.attributes = attributes;
            base.speed = speed;
            base.hp = hp;
            base.lookDirection = lookDirection;
            base.components = components;
            base.position = position;
            base.velocity = velocity;
        }

        private void chooseTarget(List<Generator> generators, List<Tower> towers, Player player) 
        {
            float distance = 9999;
            foreach (Generator i in generators)
            {
                if ((i.getPosition() - this.position).Length() < distance)
                {
                    distance = (i.getPosition() - this.position).Length();
                    target = i;
                }
            }
            foreach (Tower i in towers)
            {
                if ((i.getPosition() - this.position).Length() < distance)
                {
                    distance = (i.getPosition() - this.position).Length();
                    target = i;
                }
            }
            if ((player.getPosition() - this.position).Length() < distance)
            {
                distance = (player.getPosition() - this.position).Length();
                target = player;
            }
        }
        private void move(Vector2 distanceToTarget) 
        {
            velocity = distanceToTarget / distanceToTarget.Length() * speed;
            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        public void enemyUpdate(GameTime gameTime, List<Generator> generators, List<Tower> towers, Player player) {
            if (target == null)
            {//no target yet, chooseTarget
                chooseTarget(generators, towers, player);   
            }//end choose target
            else//have target 
            {
                Vector2 to_target = new Vector2(target.getPosition().X - this.position.X, target.getPosition().Y - this.position.Y);
                //velocity, position, atkRange, atkTimer, atk
                if (to_target.Length() > atkRange)//target out of atkRange, move
                {
                    move(to_target);
                }
                else//target in atkRange, atk
                {
                    atkTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (atkTimer > atkInterval)
                    {
                        target.setHp(target.getHp() - atk);
                        atkTimer = 0;
                    }
                }
                //lookDirection, lookAngle
                lookDirection = to_target / to_target.Length();
                if (lookDirection.X > 0)
                {
                    lookAngle = (float)Math.Asin(lookDirection.Y / lookDirection.Length());
                }
                else
                {
                    lookAngle = (float)Math.Asin(lookDirection.Y / lookDirection.Length()) + MathHelper.ToRadians(180);
                }
            }
        }
    }
}
