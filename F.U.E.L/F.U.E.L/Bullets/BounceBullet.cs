﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class BounceBullet : Bullet
    {
        public BounceBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, float damage)
            : base(game, modelComponents, position, direction, range, damage)
        {
            
        }


    }
}
