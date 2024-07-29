﻿using Microsoft.Xna.Framework;
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
        public List<GridAction> GridActions;
        public Dictionary<GridAction, double> QValues;
        public GridAction HighestValuedAction => QValues.OrderByDescending(x => x.Value).FirstOrDefault().Key;
        public State(SquareType type, List<GridAction> gridActions, double reward, bool isTerminal, Point gridCoordinates)
        {
            Type = type;
            GridActions = gridActions;
            QValues = new Dictionary<GridAction, double>();
            foreach (var action in GridActions)
            {
                QValues.Add(action, 0);
            }
            Reward = reward;
            IsTerminal = isTerminal;
            GridCoordinates = gridCoordinates;
        }

        public void UpdateQTable(double learningRate, double decay, double nextStateBestMove, double reward, GridAction actionTaken)
        {
            if(GridActions.Contains(actionTaken))
            {
                double oldQValue = QValues[actionTaken];
                double newQValue = oldQValue + learningRate * (reward + decay * nextStateBestMove - oldQValue);
                QValues[actionTaken] = newQValue;
            }
        }
    }
}
