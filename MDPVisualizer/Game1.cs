using System;
using MDPVisualizer.QLearning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;

namespace MDPVisualizer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private QLearningEnvironment learningEnvironment;
        private double learingRate = 1;

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
            Texture2D arrowTexture = Content.Load<Texture2D>("computer-icons-arrow-symbol-arrow");

            State[,] states = new State[grid.GetLength(0), grid.GetLength(1)];
            for (int i = 0; i < states.GetLength(0); i++)
            {
                for (int j = 0; j < states.GetLength(1); j++)
                {
                    states[i, j] = new State(SquareType.Empty, 0, false, new Point(i, j));
                }
            }

            states[1, 1] = new State(SquareType.Wall, 0, false, new Point(1, 1));
            states[3, 1] = new State(SquareType.FirePitHell, -10000, true, new Point(3, 1));
            states[3, 0] = new State(SquareType.Goal, 100, true, new Point(3, 0));
            
            learningEnvironment = new(states, 0.98);

            //ugly visual grid initialization
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (states[i, j].Type == SquareType.FirePitHell)
                    {
                        grid[i, j] = new GridSquare(pixel, arrowTexture, new Vector2(i, j), new Rectangle(i * (squareSize.X + spacing), j * (squareSize.Y + spacing), squareSize.X, squareSize.Y), Color.Red);
                    }
                    else if (states[i,j].Type == SquareType.Goal)
                    {
                        grid[i, j] = new GridSquare(pixel, arrowTexture, new Vector2(i, j), new Rectangle(i * (squareSize.X + spacing), j * (squareSize.Y + spacing), squareSize.X, squareSize.Y), Color.Yellow);
                    }
                    else if (states[i, j].Type == SquareType.Wall)
                    {
                        grid[i, j] = new GridSquare(pixel, arrowTexture, new Vector2(i, j), new Rectangle(i * (squareSize.X + spacing), j * (squareSize.Y + spacing), squareSize.X, squareSize.Y), Color.Gray);
                    }
                    else
                    {
                        grid[i, j] = new GridSquare(pixel, arrowTexture, new Vector2(i, j), new Rectangle(i * (squareSize.X + spacing), j * (squareSize.Y + spacing), squareSize.X, squareSize.Y), Color.White);
                    }
                }
            }

            
            
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

            learningEnvironment.TakeStep(learingRate);

            var stateGrid = learningEnvironment.StateGrid;
            for (int i = 0; i < stateGrid.GetLength(0); i++)
            {
                for(int j = 0; j < stateGrid.GetLength(1); j++)
                {
                    grid[i, j].ArrowDirection = stateGrid[i, j].HighestValuedAction.Key.IntendedDirection;
                    grid[i, j].IsCurrentState = false;
                }
            }

            grid[learningEnvironment.CurrentState.GridCoordinates.X, learningEnvironment.CurrentState.GridCoordinates.Y].IsCurrentState = true;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var item in grid)
            {
                item.DrawWithArrow(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
