using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDPVisualizer.QLearning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MDPVisualizer
{
    public class GridSquare
    {
        public Rectangle BoundingBox { get; private set; }
        public Vector2 Coordinates;
        public Color Color;
        public ActionDirection ArrowDirection;
        public bool IsCurrentState;
        private Texture2D texture;
        private Texture2D arrowTexture;
        

        public GridSquare(Texture2D texture, Texture2D arrowTexture, Vector2 coordinates, Rectangle boundingBox, Color color)
        {
            this.texture = texture;
            this.arrowTexture = arrowTexture;
            this.Coordinates = coordinates;
            this.BoundingBox = boundingBox;
            this.Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(IsCurrentState)
            {
                spriteBatch.Draw(texture, BoundingBox, Color.Green);
            }
            else
            {
                spriteBatch.Draw(texture, BoundingBox, Color);
            }
        }

        public void DrawWithArrow(SpriteBatch spriteBatch)
        {
            // Draw the square
            Draw(spriteBatch);

            // Calculate the center and origin for the arrow
            Vector2 arrowOrigin = new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2);
            Vector2 arrowPosition = new Vector2(BoundingBox.Center.X, BoundingBox.Center.Y);

            // Determine rotation based on the arrow direction
            float rotation = 0;
            switch (ArrowDirection)
            {
                case ActionDirection.Right:
                    rotation = MathHelper.ToRadians(0);
                    break;
                case ActionDirection.Left:
                    rotation = MathHelper.ToRadians(180);
                    break;
                case ActionDirection.Up:
                    rotation = MathHelper.ToRadians(270);
                    break;
                case ActionDirection.Down:
                    rotation = MathHelper.ToRadians(90);
                    break;
            }

            // Calculate scaling to fit and provide padding
            float scaleX = (float)BoundingBox.Width / arrowTexture.Width;
            float scaleY = (float)BoundingBox.Height / arrowTexture.Height;
            float scale = Math.Min(scaleX, scaleY) * 0.8f;  // 80% of the bounding box size

            // Draw the arrow
            spriteBatch.Draw(arrowTexture, arrowPosition, null, Color.White, rotation, arrowOrigin, scale, SpriteEffects.None, 0f);
        }
    }
}
