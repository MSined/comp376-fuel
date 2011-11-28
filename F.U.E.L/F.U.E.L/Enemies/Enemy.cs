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
        public Object target = null;
        protected int teamSize = 3;//max number of attackers on one target
        // Removed height as it was not and will not be used
        const float width = 1.5f;
        const float depth = 1.5f;
        // Added mini BB sizes to test with smaller models
        const float miniDepth = .7f;
        const float miniWidth = .7f;
        const float attackDistanceLimit = 25;
        // Used for offsetting the spawn position of the enemy so that we can rarely (never?)
        // Have two towers that overlap exactly, causing the game to act unexpectedly
        static Random rand = new Random();

        // Removed position from this constructor as it will be taken from the spawnpoint
        public Enemy(Game game, Model[] modelComponents,
            SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, new Vector3(spawnPoint.position.X +(float)rand.NextDouble()/2, 
                                                      spawnPoint.position.Y, 
                                                      spawnPoint.position.Z + (float)rand.NextDouble()/2), 
                   200, 20, 0.03f, spawnPoint, weapons, new FloatRectangle(spawnPoint.position.X, spawnPoint.position.Z, miniWidth, miniDepth), true)
        {
            
        }

        protected virtual void chooseTarget(List<Building> buildings, List<Player> players, List<Tower> towers) 
        {
            float distance = float.PositiveInfinity;
            foreach (Building b in buildings)
            {
                if ((b.position - this.position).Length() < distance)
                {
                    if (b is Generator)
                    {
                        Generator g = (Generator)b;
                        if (g.hp <= 50 && !g.functional) 
                            continue;
                        distance = (b.position - this.position).Length();
                        target = b;
                    }
                }
            }
            foreach (Player p in players)
            {
                if (p.attackerNum < teamSize && (p.position - this.position).Length() < distance)
                {
                    distance = (p.position - this.position).Length();
                    target = p;
                }
            }
            foreach (Tower t in towers)
            {
                if (t.attackerNum < teamSize && (t.position - this.position).Length() < distance)
                {
                    distance = (t.position - this.position).Length();
                    target = t;
                }
            }
            if (distance > attackDistanceLimit)
            {
                target = null;
            }
            else if (target is Player) { 
                Player p = (Player)target;
                ++p.attackerNum;
            }
            else if (target is Tower)
            {
                Tower t = (Tower)target;
                ++t.attackerNum;
            }
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            if (target != null && target is Generator) 
            {
                Generator g = (Generator)target;
                if (!g.functional) 
                {
                    target=null;
                    velocity = Vector3.Zero;
                }
            }
            else if (target != null && target is Player) 
            {
                if ((target.position - this.position).Length() > attackDistanceLimit) 
                {
                    Player p = (Player)target;
                    --p.attackerNum;
                    target = null;
                    velocity = Vector3.Zero;
                }
            }

            if (target == null || !target.isAlive)
            {
                List<Building> buildings = new List<Building>();
                List<Player> players = new List<Player>();
                List<Tower> towers = new List<Tower>();

                foreach (GameComponent gc in game.Components)
                {
                    if (gc is Player)
                    {
                        players.Add((Player)gc);
                    }
                    if (gc is Map)
                    {
                        Map m = (Map)gc;
                        buildings = m.usableBuildings;
                    }
                    if (gc is Tower)
                    {
                        towers.Add((Tower)gc);
                    }
                }

                chooseTarget(buildings, players, towers);
            }
            if (target != null)//start attacking the target the Update it chooses it
            {
                float targetDist = (target.position - this.position).Length();
                if (targetDist < weapons[selectedWeapon].range)
                {
                    velocity = Vector3.Zero;
                    weapons[selectedWeapon].shoot(this.position, lookDirection, true, cameraTarget);
                    lookDirection = target.position - this.position;
                }
                else
                {
                    collisionAvoidance(colliders, 12);
                    lookDirection = velocity;
                    if (velocity.Length() == 0)
                    {
                        velocity = target.position - this.position;
                        lookDirection = velocity;
                    }
                }
            }
            
            //bounds are updated in Character
            //this.bounds = new FloatRectangle(position.X, position.Z, width, depth);

            base.Update(gameTime, colliders, cameraTarget);
        }

        private void collisionAvoidance(List<Object> colliders, float angle)
        {
            if (!needSteer(colliders, target.position - this.position))//nothing block the way to target
            {
                velocity = target.position - this.position;
            }
            else if (!needSteer(colliders, velocity)) {}//current direction is good, keep going
            else//need to change direction
            {
                Vector3 right = Vector3.Zero;
                Vector3 left = Vector3.Zero;
                
                right.X = (velocity.X * (float)Math.Cos(MathHelper.ToRadians(angle))) - (velocity.Z * (float)Math.Sin(MathHelper.ToRadians(angle)));
                right.Z = (velocity.X * (float)Math.Sin(MathHelper.ToRadians(angle))) + (velocity.Z * (float)Math.Cos(MathHelper.ToRadians(angle)));
                
                left.X = (velocity.X * (float)Math.Cos(MathHelper.ToRadians(-angle))) - (velocity.Z * (float)Math.Sin(MathHelper.ToRadians(-angle)));
                left.Z = (velocity.X * (float)Math.Sin(MathHelper.ToRadians(-angle))) + (velocity.Z * (float)Math.Cos(MathHelper.ToRadians(-angle)));
                
                if ((this.position + right - target.position).Length() > (this.position + left - target.position).Length())
                {
                    velocity = right;
                }
                else { velocity = left; }
            }
        }
        private bool needSteer(List<Object> colliders, Vector3 direction)
        {
            Vector3 step = direction;
            step.Normalize();
            FloatRectangle next;
            foreach (Object o in colliders) 
            {
                if (!(o is Bullet))
                {
                    next = new FloatRectangle(position.X+step.X, position.Z+step.Z, this.bounds.Width, this.bounds.Height);
                    if (next.FloatIntersects(o.bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
