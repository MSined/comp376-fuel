﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Generator : Building
    {
        public Generator(Game game, Model[] modelComponents, Vector3 position,
            float angle)
            : base(game, modelComponents, position, angle)
        {

        }

    }
}
