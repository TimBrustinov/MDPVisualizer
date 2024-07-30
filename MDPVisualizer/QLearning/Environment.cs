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
        public double epsilon = 0.8;
        public double epsilonDecay = 0.999;
        private double epsilonMin = 0.1;
        public QLearningEnvironment(State[,] stateGrid, double decay) 
        { 
            random = new Random();
            StateGrid = stateGrid;
            CurrentState = StateGrid[0, 0];
            this.decay = decay;
        }    
        public void TakeStep(double learningRate)
        {
            //resets the Current state to a random square on the grid
            if(CurrentState.IsTerminal)
            {
                Console.WriteLine($"CurrentState reached {CurrentState.Type}");
                ResetCurrentState();
                Console.WriteLine($"CurrentState moved to grid position {CurrentState.GridCoordinates}");
            }

            ActionDirection actionTaken;
            //based on the epsilon value, chooses to either explore a random direction or the direction with the highest Q value
            if(random.NextDouble() < epsilon)
            {
                var randomAction = CurrentState.GridActions.Keys.ToList()[random.Next(CurrentState.GridActions.Count)];
                actionTaken = randomAction;
            }
            else
            {
                actionTaken = CurrentState.HighestValuedAction.Key.IntendedDirection;
            }

            PerformAction(learningRate, actionTaken);
        }

        private void PerformAction(double learningRate, ActionDirection actionTaken) 
        {
            //gets the direction(80% chance its the actual direction, 20% chance its a perpendiculare direction) from the actionTaken 
            var actionTakenOutcome = CurrentState.GridActions[actionTaken].GetOutcomeDirection();
            //calculates the grid position of the next step
            var nextStep = new Point(actionTakenOutcome.direction.X + CurrentState.GridCoordinates.X, actionTakenOutcome.direction.Y + CurrentState.GridCoordinates.Y);

            
            // check if the next step is out of bounds or is a wall
            if (nextStep.X >= 0 && nextStep.X < StateGrid.GetLength(0) && nextStep.Y >= 0 && nextStep.Y < StateGrid.GetLength(1) && StateGrid[nextStep.X, nextStep.Y].Type != SquareType.Wall)
            {
                //if the next state is an end state (the goal or the fire pit) then calculate the Q value of the move taken at the CurrentState without using the next states highest Q value
                if (StateGrid[nextStep.X, nextStep.Y].IsTerminal)
                {
                    CurrentState.UpdateQTable(learningRate, decay, 0, StateGrid[nextStep.X, nextStep.Y].Reward, actionTakenOutcome.actionTaken);
                    CurrentState = StateGrid[nextStep.X, nextStep.Y];
                }
                else
                {
                    var nextStateHighestQValue = StateGrid[nextStep.X, nextStep.Y].HighestValuedAction.Value;
                    //calculates the Q value for the current state based on the next state chosen by nextStep
                    CurrentState.UpdateQTable(learningRate, decay, nextStateHighestQValue, StateGrid[nextStep.X, nextStep.Y].Reward, actionTakenOutcome.actionTaken);
                    CurrentState = StateGrid[nextStep.X, nextStep.Y];
                }
            }
            else
            {
                //if it hits a wall or goes out of bounds(aka hitting a wall) then apply a penalty for taking that action
                CurrentState.UpdateQTable(learningRate, decay, 0, -1, actionTakenOutcome.actionTaken);
            }

            //decays the epsilon so that as the algo continues, the environment will start exploring less and choosing best moves more
            if(epsilon > epsilonMin)
            {
                epsilon *= epsilonDecay;
            }

        }
        private void ResetCurrentState()
        {
            do
            {
                CurrentState = StateGrid[random.Next(0, StateGrid.GetLength(0) - 1), random.Next(0, StateGrid.GetLength(1) - 1)];
            } while(CurrentState.IsTerminal || CurrentState.Type == SquareType.Wall);
        }
    }
}
