using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class SeekingBullet : Bullet
    {

        Object target = null;
        private float turnAngle = MathHelper.ToRadians(50);
        private float minTravelDist = 2;

        public SeekingBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, direction, range, damage, shotByEnemy)
        {
            speed = 0.05f;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            if (target == null || target.isAlive == false) chooseTarget(colliders);

            if (target != null && distanceTraveled > minTravelDist){
                Vector3 targetDirection = target.position - position;
                targetDirection.Normalize();
                direction.Normalize();
                float angle = (float)Math.Acos(Vector3.Dot(direction, targetDirection));

                if (angle == 0){
                    //do nothing
                }else{
                    Matrix m = Matrix.Identity;
                    if (Math.Abs(angle) < turnAngle){
                        m = Matrix.CreateRotationY(angle);
                    }
                    else if (angle < turnAngle)
                    {
                        m = Matrix.CreateRotationY(turnAngle / 10);
                    }
                    else
                    {
                        m = Matrix.CreateRotationY(-turnAngle / 10);
                    }
                    direction = Vector3.Transform(direction, m);

                }
            }
            
            base.Update(gameTime, colliders, cameraTarget);
        }

        protected virtual void chooseTarget(List<Object> colliders)
        {
            float distance = float.PositiveInfinity;
            foreach (GameComponent gc in game.Components)
            {

                if (gc is Object)
                {
                    Object o = (Object)gc;
                    /*if (o is Player && this.shotByEnemy)
                    {
                        if ((o.position - this.position).Length() < distance)
                        {
                            distance = (o.position - this.position).Length();
                            target = o;
                        }
                        continue;
                    }*/
                    if (o is Enemy && !this.shotByEnemy)
                    {
                        if ((o.position - this.position).Length() < distance)
                        {
                            distance = (o.position - this.position).Length();
                            target = o;
                        }
                        continue;
                    }
                    /*if (o is Tower && this.shotByEnemy)//same as player, but tower
                    {
                        if ((o.position - this.position).Length() < distance)
                        {
                            distance = (o.position - this.position).Length();
                            target = o;
                        }
                        continue;
                    }
                    if (o is Generator && this.shotByEnemy)//same as player, but generator
                    {
                        if ((o.position - this.position).Length() < distance)
                        {
                            distance = (o.position - this.position).Length();
                            target = o;
                        }
                        continue;
                    }*/
                }
            }
        }
    }
}
