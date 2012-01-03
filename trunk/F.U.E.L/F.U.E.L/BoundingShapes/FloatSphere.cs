using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class FloatSphere
    {
        public Vector3 position;
        public float radius;

        public FloatSphere(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public bool FloatIntersects(FloatSphere other)
        {
            // If this is somehow contained within other
            if ((this.position - other.position).LengthSquared() < (this.radius + other.radius) * (this.radius + other.radius))
            {
                return true;
            }


            // No collision
            return false;
        }
    }
}
