using System;
using MDPVisualizer.QLearning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MDPVisualizer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GridSquare[,] grid = new GridSquare[4, 3];
        private Texture2D pixel;
        private Point squareSize = new Point(100, 100);
        private int spacing = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            State[,] states = new State[grid.GetLength(0), grid.GetLength(1)];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new GridSquare(pixel, new Vector2(i, j), new Rectangle(i * (squareSize.X + spacing), j * (squareSize.Y + spacing), squareSize.X, squareSize.Y), Color.White);
                    states[i, j] = new State(SquareType.Empty, new List<GridAction>(), 0, false, new Point(i, j));
                }
            }

            states[1, 1] = new State(SquareType.Wall, new List<GridAction>(), 0, false, new Point(1, 1));
            states[3, 1] = new State(SquareType.FirePitHell, new List<GridAction>(), -1, true, new Point(1, 2));
            states[3, 0] = new State(SquareType.Goal, new List<GridAction>(), 1, true, new Point(1, 3));

            QLearningEnvironment environment = new(states, 0.98);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var item in grid)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
