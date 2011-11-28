﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class HeadShot : Weapon
    {
        private const float RANGE = 10;
        private const int DAMAGE = 100;
        private const int FIRERATE = (int)(6f * 10000000);

        private SoundEffect soundEffect;

        public HeadShot(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            //soundEffect = game.Content.Load<SoundEffect>(@"Sounds/pistol");

        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                game.Components.Add(new Bullet(game, this.bulletModelComponents, position, direction, range, damage, shotByEnemy));
                lastShot = nowTick;
                //soundEffect.Play();
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}