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
        const float width = 1.5f;
        const float depth = 1.5f;
        
        public Enemy(Game game, Model[] modelComponents, Vector3 position,
            SpawnPoint spawnPoint, Weapon[] weapons, int topHP = 20, int topSP = 20, float speed = 0.04f
            )
            : base(game, modelComponents, position, topHP, topSP, speed, spawnPoint, weapons, new FloatRectangle(position.X, position.Z, width, depth), true)
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
                        if (g.hp == 0) 
                            continue;
                        distance = (b.position - this.position).Length();
                        target = b;
                    }
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
            foreach (Tower t in towers)
            {
                if ((p.position - this.position).Length() < distance)
                {
                    distance = (t.position - this.position).Length();
                    target = t;
                }
            }
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            if (target = null || !target.isAlive)//choose target if it doesn't have one or the last one is dead
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
                        buildings = m.buildings;
                    }
                    if (gc is Tower)
                    {
                        towera.Add((Tower)gc);
                    }
                }
                chooseTarget(buildings, players, towers);
            }
            else
            {
                lookDirection = target.position - this.position;

                float targetDist = (target.position - this.position).Length();
                if (targetDist < weapons[selectedWeapon].range)
                {
                    velocity = new Vector3(0, 0, 0);
                    weapons[selectedWeapon].shoot(this.position, lookDirection, true);
                }
                else
                {
                    velocity = target.position - this.position;
                }
            }
            base.Update(gameTime, colliders);
        }

    }
}
