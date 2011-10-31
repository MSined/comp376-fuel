﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Object : GameComponent
    {
        protected Model[] modelComponents { get; private set; }
        public Vector3 position { get; protected set; }

        public Object(Game game, Model[] modelComponents, Vector3 position)
            : base(game)
        {
            this.modelComponents = modelComponents;
            this.position = position;
        }
        
    }
}
