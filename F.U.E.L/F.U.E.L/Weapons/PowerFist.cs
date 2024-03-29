﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class PowerFist : Weapon
    {
        //range = 1.75/ enemy size is 1.5
        //range changed to 1/ enemy size is .7
        private const float RANGE = 1f;
        private const int DAMAGE = 15;
        private const int FIREDELAY = 1 * 1000;
        private const int SPCOST = 10;

        public PowerFist(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {
            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/alienattack");
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                game.Components.Add(new Bullet(game, this.bulletModelComponents, position, direction, range, damage, shotByEnemy));
                interval = 0;
                playSound(position, cameraTarget);
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
