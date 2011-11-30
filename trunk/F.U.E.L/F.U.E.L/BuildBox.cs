using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class BuildBox : Object
    {
        Player boundPlayer;

        public BuildBox(Game game, SuperModel[] modelComponents, Vector3 position, FloatRectangle bounds, Player boundPlayer)
            : base(game, modelComponents, position, bounds, false)
        {
            this.boundPlayer = boundPlayer;
        }

        public override void Update(GameTime gameTime)
        {
            // Component-wise sum of two vectors to create the new position
            position = boundPlayer.position + boundPlayer.lookDirection;

            world = Matrix.CreateTranslation(position);

            // Update the bounds of the BuildBox
            bounds = new FloatRectangle(position.X, position.Z, 1, 1);

            base.Update(gameTime);
        }

        public override void Draw(Camera camera)
        {
            modelComponents[0].Draw(camera, world);
        }
    }
}
