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
        public Vector3 position;
        static int objectIDCounter = 0;
        public int objectID = objectIDCounter++;
        public int[] cellIDs = { -1, -1, -1, -1 };

        public FloatRectangle bounds { get; protected set; }
        public bool isAlive;

        public Object(Game game, Model[] modelComponents, Vector3 position, FloatRectangle bounds, bool isAlive)
            : base(game)
        {
            this.game = (Game1) game;
            this.modelComponents = modelComponents;
            this.position = position;
            this.world = Matrix.CreateTranslation(position);
            this.bounds = bounds;
            this.isAlive = isAlive;
        }

        public abstract void Draw(Camera camera);

        public virtual void Update(GameTime gameTime, List<Object> colliders)
        {

        }
    }
}
