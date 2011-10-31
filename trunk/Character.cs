using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Character : Objects
    {

        protected int hp;
        protected Vector2 lookDirection;

        public Character() { }
        public Character(int hp, Vector2 lookDirection,
            Model[] components, Vector3 position, Vector3 velocity)
        {
            this.hp = hp;
            this.lookDirection = lookDirection;
            setComponents(components);
            setPosition(position);
            setVelocity(velocity);
        }

        public int getHp() { return hp; }
        public Vector2 getLookDirection(){ return lookDirection; }

        public void setHp(int hp) { this.hp = hp; }
        public void setLookDirection(Vector2 lookDirection) { this.lookDirection = lookDirection; }
    }
}
