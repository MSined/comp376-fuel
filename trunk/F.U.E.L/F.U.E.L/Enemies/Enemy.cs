﻿using System;
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
        // Removed height as it was not and will not be used
        const float width = 1.5f;
        const float depth = 1.5f;
        // Added mini BB sizes to test with smaller models
        const float miniDepth = .7f;
        const float miniWidth = .7f;
        const float attackDistanceLimit = 50;
        // Used for offsetting the spawn position of the enemy so that we can rarely (never?)
        // Have two towers that overlap exactly, causing the game to act unexpectedly
        static Random rand = new Random();
        
        // Removed position from this constructor as it will be taken from the spawnpoint
        public Enemy(Game game, Model[] modelComponents,
            SpawnPoint spawnPoint, Weapon[] weapons, int topHP = 200, int topSP = 20, float speed = 0.1f
            )
            : base(game, modelComponents, new Vector3(spawnPoint.position.X + (float)rand.NextDouble(), 
                                                      spawnPoint.position.Y, 
                                                      spawnPoint.position.Z + (float)rand.NextDouble()), 
                   topHP, topSP, speed, spawnPoint, weapons, new FloatRectangle(spawnPoint.position.X, spawnPoint.position.Z, miniWidth, miniDepth), true)
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
                if ((t.position - this.position).Length() < distance)
                {
                    distance = (t.position - this.position).Length();
                    target = t;
                }
            }
            if (distance > attackDistanceLimit)
                target = null;
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            if (this.hp <= 0)
            {
                this.isAlive = false;
                return;
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
                        buildings = m.buildings;
                    }
                    if (gc is Tower)
                    {
                        towers.Add((Tower)gc);
                    }
                }

                chooseTarget(buildings, players, towers);
            }
            else//if no target in sight
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
            //bounds are updated in Character
            //this.bounds = new FloatRectangle(position.X, position.Z, width, depth);

            base.Update(gameTime, colliders);
        }

        //collisions in Character and Bullet
    }
}
