﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Tower : Character
    {
        protected Object target = null;
        const float height = .5f;
        const float width = .5f;
        const float depth = .5f;

        public Tower(Game game, Model[] modelComponents,
            int topHP, int topSP, Vector3 position, SpawnPoint anySpawnPoint, Weapon[] weapons)
            : base(game, modelComponents, position, topHP, topSP, 0, anySpawnPoint, weapons, new FloatRectangle(position.X, position.Z, width, depth), true)
        {

        }

        private void chooseTarget(List<Enemy> enemies)
        {
            float distance = float.PositiveInfinity;
            foreach (Enemy e in enemies)
            {
                if ((e.position - this.position).Length() < distance)
                {
                    distance = (e.position - this.position).Length();
                    target = e;
                }
            }
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            if (target == null || !target.isAlive)//choose target if it doesn't have one or the last one is dead
            {
                List<Enemy> enemies = new List<Enemy>();
                foreach (GameComponent gc in game.Components)
                {
                    if (gc is Enemy)
                    {
                        enemies.Add((Enemy)gc);
                    }
                }
                chooseTarget(enemies);
            }
            else
            {
                float targetDist = (target.position - this.position).Length();
                if (targetDist < weapons[selectedWeapon].range)
                {
                    lookDirection = target.position - this.position;
                    weapons[selectedWeapon].shoot(this.position, lookDirection, false);
                }
            }
            base.Update(gameTime, colliders);
        }
    }
}