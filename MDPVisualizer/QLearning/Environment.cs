using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPVisualizer.QLearning
{
    public class QLearningEnvironment
    {

        //Represents the environment the agent is in
        public State[,] StateGrid;
        public State CurrentState;
        private Random random;
        private double decay;
        public QLearningEnvironment(State[,] stateGrid, double decay) 
        { 
            random = new Random();
            StateGrid = stateGrid;
            CurrentState = StateGrid[0, 0];
            this.decay = decay;
        }    
        public void TakeStep(double learningRate)
        {
            var bestStepOutcome = CurrentState.HighestValuedAction.GetOutcomeDirection().Item2;
            var nextStep = new Point(bestStepOutcome.X + CurrentState.GridCoordinates.X, bestStepOutcome.Y + CurrentState.GridCoordinates.Y);
            if(nextStep.X > 0 && nextStep.X < StateGrid.GetLength(0) && nextStep.Y > 0 && nextStep.Y < StateGrid.GetLength(1))
            {
                if(StateGrid[nextStep.X, nextStep.Y].IsTerminal)
                {
                    CurrentState.UpdateQTable(learningRate, decay, 0, StateGrid[nextStep.X, nextStep.Y].Reward, CurrentState.HighestValuedAction);
                    CurrentState = StateGrid[nextStep.X, nextStep.Y];
                }
                else
                {
                    var nextStateBestMove = StateGrid[nextStep.X, nextStep.Y].HighestValuedAction.IntendedDirectionProbability;
                    CurrentState.UpdateQTable(learningRate, decay, nextStateBestMove, StateGrid[nextStep.X, nextStep.Y].Reward, CurrentState.HighestValuedAction);
                    CurrentState = StateGrid[nextStep.X, nextStep.Y];
                }
            }
        }

    }
}
