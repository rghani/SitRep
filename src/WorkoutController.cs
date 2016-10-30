using System;
using Microsoft.Kinect.VisualGestureBuilder;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class WorkoutController
    {
        private DiscreteGestureResult workoutDiscrete;
        private ContinuousGestureResult workoutContinuous;
        private string gestureName;

        public WorkoutController()
        {
        }

        public void getResults(DiscreteGestureResult passedWorkoutDiscrete, ContinuousGestureResult passedWorkoutContinuous, string passedGestureName)
        {
            this.workoutDiscrete = passedWorkoutDiscrete;
            this.workoutContinuous = passedWorkoutContinuous;
            this.gestureName = passedGestureName;
        }

        public void checkStatus()
        {
            if(this.workoutDiscrete != null && this.workoutContinuous != null)
            {
                //This double checks whether the gesture is occuring, if its true, it fires an event
                if (this.workoutDiscrete.Detected && this.workoutContinuous.Progress >= 0)
                {
                    WorkoutControllerEventArgs args = new WorkoutControllerEventArgs();
                    args.DiscreteResult = this.workoutDiscrete;
                    args.ContinuousResult = this.workoutContinuous;
                    args.gestureName = this.gestureName;

                    InitiateWorkoutEvent(args);
                }
            }
        }

        protected virtual void InitiateWorkoutEvent(WorkoutControllerEventArgs e)
        {
                OnWorkoutDetected?.Invoke(this, e);
        }
        public event EventHandler<WorkoutControllerEventArgs> OnWorkoutDetected;

    }
    public class WorkoutControllerEventArgs : EventArgs
    {
        public DiscreteGestureResult DiscreteResult { get; set; }
        public ContinuousGestureResult ContinuousResult { get; set; }
        public string gestureName { get; set; }
    }
}



