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
        protected Object target = null;
        const float height = .5f;
        const float width = .5f;
        const float depth = .5f;
        
        public Enemy(Game game, Model[] modelComponents, Vector3 position,
            int topHP, int topSP, float speed, SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, position, topHP, topSP, speed, spawnPoint, weapons, new FloatRectangle(position.X, position.Z, width, depth), true)
        {

        }

        protected virtual void chooseTarget(List<Building> buildings, List<Player> players) 
        {
            float distance = float.PositiveInfinity;
            foreach (Building b in buildings)
            {
                if ((b.position - this.position).Length() < distance)
                {
                    distance = (b.position - this.position).Length();
                    target = b;
                }
            }
            foreach (Player p in players)
            {
                if ((p.position - this.position).Length() < distance)
                {
                    distance = (p.position - this.position).Length();
                    target = p;
                }
            }
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            List<Building> buildings = new List<Building>();
            List<Player> players = new List<Player>();

             foreach (GameComponent gc in game.Components)
             {
                if (gc is Player)
                {
                    players.Add((Player)gc);
                }
                if (gc is Map)
                {
                    Map m = (Map)gc;
                    buildings = m.buildings;
                }
            }
            chooseTarget(buildings, players);

            lookDirection = target.position - this.position;

            float targetDist = (target.position - this.position).Length();
            if (targetDist < weapons[selectedWeapon].range)
            {
                velocity = new Vector3(0, 0, 0);
                weapons[selectedWeapon].shoot(this.position, lookDirection);
            }
			else
			{
				velocity = target.position - this.position;
			}

            CheckCollisions(colliders);

            this.bounds = new FloatRectangle(position.X, position.Z, width, depth);

            base.Update(gameTime, colliders);
        }

        public void CheckCollisions(List<Object> colliders)
        {
            foreach (Object o in colliders)
            {
                if (bounds.FloatIntersects(o.bounds))
                {
                    if (o is Bullet)
                    {
                        o.isAlive = false;
                        this.isAlive = false;
                        continue;
                    }
                    Vector3 moveBack = position - o.position;
                    moveBack.Normalize();
                    velocity += moveBack;
                }
            }
        }
    }
}
