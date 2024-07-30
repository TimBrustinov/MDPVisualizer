using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPVisualizer.QLearning
{
    public enum SquareType
    {
        Empty,
        Wall,
        FirePitHell,
        Goal,
    }
    //for now represents each square in the grid
    public class State
    {
        public Point GridCoordinates;
        public SquareType Type;
        public bool IsTerminal;
        public double Reward;
        //dictionary to map ActionDirecton to GridAction for faster/easier lookup   
        public Dictionary<ActionDirection, GridAction> GridActions = new Dictionary<ActionDirection, GridAction>()
        {
            [ActionDirection.Up] = new(ActionDirection.Up),
            [ActionDirection.Down] = new(ActionDirection.Down),
            [ActionDirection.Left] = new(ActionDirection.Left),
            [ActionDirection.Right] = new(ActionDirection.Right)
        };

        //Values of each action
        public Dictionary<GridAction, double> QValues;
        //Action with the highest value
        public KeyValuePair<GridAction, double> HighestValuedAction => QValues.OrderByDescending(x => x.Value).FirstOrDefault();
        public State(SquareType type, double reward, bool isTerminal, Point gridCoordinates)
        {
            Type = type;
            QValues = new Dictionary<GridAction, double>();
            foreach (var action in GridActions)
            {
                QValues.Add(action.Value, 0);
            }
            Reward = reward;
            IsTerminal = isTerminal;
            GridCoordinates = gridCoordinates;
        }
        //calculates the Q learning formula for the action taken by the environment
        public void UpdateQTable(double learningRate, double decay, double nextStateHighestQValue, double reward, ActionDirection actionTaken)
        {
            var gridAction = GridActions[actionTaken];
            double oldQValue = QValues[gridAction];
            double newQValue = oldQValue + learningRate * (reward + decay * nextStateHighestQValue - oldQValue);
            QValues[gridAction] = newQValue;
        }
    }
}
