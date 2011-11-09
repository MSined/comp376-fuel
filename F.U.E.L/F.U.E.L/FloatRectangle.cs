using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F.U.E.L
{
    class FloatRectangle
    {
        // Left X-value
        public float Left;
        // Bottom Y-value
        public float Bottom;
        // Top Y-value
        public float Top;
        // Right X-value
        public float Right;
        // Self-explanatory
        public float CenterX;
        // Self-explanatory
        public float CenterY;
        // Width of rectangle
        public float Width;
        // Height of rectangle
        public float Height;

        public FloatRectangle(float centerX, float centerY, float width, float height)
        {
            Left = centerX-width/2;
            Bottom = centerY-height/2;
            Top = centerY + height/2;
            Right = centerX + width/2;
            CenterX = centerX;
            CenterY = centerY;
            Width = width;
            Height = height;
        }

        public bool FloatIntersects(FloatRectangle other)
        {
            if (this.Right >= other.Left &&
               this.Left <= other.Right &&
               this.Top >= other.Bottom &&
               this.Bottom <= other.Top)
                return true;
            return false;
        }

        public bool FloatIntersectsExtended(FloatRectangle other)
        {
            if (this.Right + 0.1f > other.Left - 0.1f &&
               this.Left - 0.1f < other.Right + 0.1f &&
               this.Top - 0.1f > other.Bottom + 0.1f &&
               this.Bottom + 0.1f < other.Top - 0.1f)
                return true;
            return false;
        }
    }
}
