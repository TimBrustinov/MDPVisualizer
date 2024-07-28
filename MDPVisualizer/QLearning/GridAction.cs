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
        public double IntendedDirectionProbability => OutcomeProbabilities[IntendedDirection];

        public Dictionary<ActionDirection, double> OutcomeProbabilities = new()
        {
            [ActionDirection.Left] = 0,
            [ActionDirection.Right] = 0,
            [ActionDirection.Up] = 0,
            [ActionDirection.Down] = 0,
        };

        public GridAction(ActionDirection intendedDirection)
        {
            IntendedDirection = intendedDirection;
            OutcomeProbabilities = new Dictionary<ActionDirection, double>();
            SetProbabilities();
        }

        private void SetProbabilities()
        {
            OutcomeProbabilities[IntendedDirection] = 0.8;
            switch (IntendedDirection)
            {
                case ActionDirection.Up:
                case ActionDirection.Down:
                    OutcomeProbabilities[ActionDirection.Left] = 0.1;
                    OutcomeProbabilities[ActionDirection.Right] = 0.1;
                    break;
                case ActionDirection.Left:
                case ActionDirection.Right:
                    OutcomeProbabilities[ActionDirection.Up] = 0.1;
                    OutcomeProbabilities[ActionDirection.Down] = 0.1;
                    break;
            }
        }
    }

}
