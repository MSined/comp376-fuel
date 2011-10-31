using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Objects
    {
        protected Model[] components;
        protected Vector3 position;
        protected Vector3 velocity;

        public Objects()
        {
        }

        public Model[] getComponents() { return components; }
        public Vector3 getPosition() { return position; }
        public Vector3 getVelocity() { return velocity; }

        public void setComponents(Model[] components){ this.components = components; }
        public void setPosition(Vector3 position){ this.position = position; }
        public void setVelocity(Vector3 velocity) { this.velocity = velocity; }
    }
}
