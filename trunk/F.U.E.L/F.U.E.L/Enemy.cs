using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Enemy : Character
    {
        private Object target;
        
        public Enemy(Game game, Model[] modelComponents, Vector3 position,
            int topHP, int topSP, float speed, SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, position, topHP, topSP, speed, spawnPoint, weapons)
        {

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
            if ((player.position - this.position).Length() < distance)
            {
                distance = (player.position - this.position).Length();
                target = player;
            }
        }
        private void move(Vector2 distanceToTarget) 
        {
            Vector2 a = distanceToTarget / distanceToTarget.Length() * speed;
            velocity = new Vector3(a.X, 0, a.Y);
            position += velocity;
        }

        
        public void enemyUpdate(GameTime gameTime, List<Generator> generators, List<Tower> towers, Player player) {
            if (target == null)
            {//no target yet, chooseTarget
                chooseTarget(generators, towers, player);   
            }//end choose target
                /*
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
                 * */
        }
    }
}
