using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class BurningBullet : Bullet
    {

        Character target = null;
        private const int HITTIME = 5 * 10000000;
        private const int HITRATE = (int)(1 / 2.0 * 10000000);
        private const int MAXSTACK = 10;
        private long currentHitTime = 0;
        private long lastHit = 0;

        public BurningBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, direction, range, damage, shotByEnemy)
        {
            
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            if (target == null)
            {
                position += Vector3.Multiply(direction, speed);
                world = Matrix.CreateTranslation(position);
                distanceTraveled += Vector3.Multiply(direction, speed).Length();
                if (distanceTraveled > range)
                {
                    this.isAlive = false;
                }

                CheckCollisions(colliders);

                this.bounds = new FloatRectangle(this.position.X, this.position.Z, width, height);
            }
            else
            {
                long nowTick = DateTime.Now.Ticks;

                if (currentHitTime > 0 && lastHit + HITRATE < nowTick)
                {
                    target.hp -= damage;
                    currentHitTime -= HITRATE;
                    if (currentHitTime <= 0)
                    {
                        this.isAlive = false;
                        --target.burningStacks;
                    }
                    lastHit = nowTick;
                }
            }
        }

        public override void CheckCollisions(List<Object> colliders)
        {
            foreach (Object o in colliders)
            {
                if (bounds.FloatIntersects(o.bounds))
                {
                    /*if (o is Player && this.shotByEnemy)
                    {
                        Character c = (Character)o;
                        if (c.burningStacks < MAXSTACK)
                        {
                            target = c;
                            ++target.burningStacks;
                            currentHitTime = HITTIME;
                            continue;
                        }
                    }*/
                    if (o is Enemy && !this.shotByEnemy)
                    {
                        Character c = (Character)o;
                        if (c.burningStacks < MAXSTACK)
                        {
                            target = c;
                            ++target.burningStacks;
                            currentHitTime = HITTIME;
                            continue;
                        }
                    }
                    /*if (o is Tower && this.shotByEnemy)//same as player, but tower
                    {
                        Character c = (Character)o;
                        if (c.burningStacks < MAXSTACK)
                        {
                            target = c;
                            ++target.burningStacks;
                            currentHitTime = HITTIME;
                            continue;
                        }
                    }*/
                    if (o is Building)// && bounds.FloatIntersects(o.bounds))
                    {
                        this.isAlive = false;
                    }
                    /*if (o is Generator && this.shotByEnemy)//same as player, but generator
                    {
                        this.isAlive = false;
                        continue;
                    }*/

                }
            }
        }

        public override void Draw(Camera camera)
        {
            if (target == null)
            {
                Matrix[] transforms = new Matrix[modelComponents[0].Bones.Count];
                modelComponents[0].CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in modelComponents[0].Meshes)
                {
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.Projection = camera.projection;
                        be.View = camera.view;
                        be.World = world * mesh.ParentBone.Transform;
                    }

                    mesh.Draw();
                }
            }
        }

    }
}
