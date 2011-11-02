using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    abstract class Object : GameComponent
    {
        public Game1 game { get; private set; }
        protected Matrix world = Matrix.Identity;
        protected Model[] modelComponents { get; private set; }
        public Vector3 position { get; protected set; }

        public Object(Game game, Model[] modelComponents, Vector3 position)
            : base(game)
        {
            this.game = (Game1) game;
            this.modelComponents = modelComponents;
            this.position = position;
            this.world = Matrix.CreateTranslation(position);
        }

        public abstract void Draw(Camera camera);

    }
}
