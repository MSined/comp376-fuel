using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Bullet : Object
    {
        public Bullet(Game game, Model[] modelComponents, Vector3 position)
            : base(game, modelComponents, position)
        {

        }

    }
}
