using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPVisualizer.QLearning
{
    public enum ActionDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public class GridAction
    {
        public ActionDirection IntendedDirection { get; private set; }
        public double IntendedDirectionProbability => outcomeProbabilities[IntendedDirection];

        private Dictionary<ActionDirection, double> outcomeProbabilities = new()
        {
            [ActionDirection.Left] = 0,
            [ActionDirection.Right] = 0,
            [ActionDirection.Up] = 0,
            [ActionDirection.Down] = 0,
        };

        private Dictionary<ActionDirection, Point> directionToMovement = new()
        {
            [ActionDirection.Left] = new (-1, 0),
            [ActionDirection.Right] = new (1, 0),
            [ActionDirection.Up] = new (0, -1),
            [ActionDirection.Down] = new (0, 1),
        };

        public GridAction(ActionDirection intendedDirection)
        {
            IntendedDirection = intendedDirection;
            outcomeProbabilities = new Dictionary<ActionDirection, double>();
            SetProbabilities();
        }
        public (ActionDirection, Point) GetOutcomeDirection()
        {
            double randomValue = new Random().NextDouble();
            double cumulativeProbability = 0;
            foreach (var outcome in outcomeProbabilities)
            {
                cumulativeProbability += outcome.Value;
                if (randomValue <= cumulativeProbability)
                {
                    var direction = directionToMovement[outcome.Key];
                    return (outcome.Key, direction);
                }
            }
            var intendedOutcome = outcomeProbabilities.OrderByDescending(x => x.Value).FirstOrDefault().Key;
            return (intendedOutcome, directionToMovement[intendedOutcome]);
        }
        private void SetProbabilities()
        {
            outcomeProbabilities[IntendedDirection] = 0.8;
            switch (IntendedDirection)
            {
                case ActionDirection.Up:
                case ActionDirection.Down:
                    outcomeProbabilities[ActionDirection.Left] = 0.1;
                    outcomeProbabilities[ActionDirection.Right] = 0.1;
                    break;
                case ActionDirection.Left:
                case ActionDirection.Right:
                    outcomeProbabilities[ActionDirection.Up] = 0.1;
                    outcomeProbabilities[ActionDirection.Down] = 0.1;
                    break;
            }
        }
    }

}
