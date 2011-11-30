using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Building : Object
    {
        public SuperModel model { get; protected set; }
        
        public Building(Game game, SuperModel[] modelComponents, Vector3 position,
            float width, float depth, float angle)
            : base(game, modelComponents, position, new FloatRectangle(position.X, position.Z, width, depth), true)
        {
            model = modelComponents[0];
            world = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(position);
            this.position = position;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget) 
        {
		
		}

        public override void Draw(Camera camera)
        {
            model.Draw(camera, world);
        }

        public virtual void use()
        {

        }
    }
}