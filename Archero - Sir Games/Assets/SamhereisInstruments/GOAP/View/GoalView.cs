using SO.GOAP;
using System;

namespace GOAP.View
{
    [Serializable]
    public class GoalView
    {
        [Serializable]
        public class SubGoalView
        {
            public GOAPString subgoal;
            public int cost;
        }

        public SubGoalView[] subgoal;
        public int priority;
    }
}