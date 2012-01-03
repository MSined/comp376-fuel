using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Waypoint : Object
    {
        public struct Edge { public float length; public Waypoint connectedTo; }

        int temp = 0;
        static int IDCtr = 0;
        public int ID;
        public Vector3 position;
        public List<Edge> connectedEdges = new List<Edge>();

        public Waypoint(Game game, Vector3 position)
            : base(game, null, position, null, true)
        {
            this.ID = IDCtr++;
            this.position = position;
        }

        public override void Draw(Camera camera)
        {
            throw new NotImplementedException();
        }
    }
}
