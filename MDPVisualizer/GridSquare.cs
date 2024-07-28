using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MDPVisualizer
{
    public class GridSquare
    {
        public Rectangle BoundingBox { get; private set; }
        public Vector2 Coordinates;
        public Color Color;
        private Texture2D texture;

        public GridSquare(Texture2D texture, Vector2 coordinates, Rectangle boundingBox, Color color)
        {
            this.texture = texture;
            this.Coordinates = coordinates;
            this.BoundingBox = boundingBox;
            this.Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, BoundingBox, Color);
        }
    }
}
