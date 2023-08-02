using SO.GOAP;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    public class SubGoals
    {
        public Dictionary<GOAPString, int> subGoals;
        public bool remove;

        public SubGoals(Dictionary<GOAPString, int> goal, bool r)
        {
            subGoals = goal;
            remove = r;
        }
    }
}