using SO.GOAP;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    public class GPlanner
    {
        public Queue<GAction> Plan(List<GAction> actions, Dictionary<GOAPString, int> goal, GoapStates beliefStates)
        {
            List<GAction> usableActions = new List<GAction>();

            //of all the actions available find the ones that can be achieved.
            foreach (GAction a in actions)
            {
                if (a.IsAchievable())
                {
                    usableActions.Add(a);
                }
            }

            //create the first node in the graph
            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0.0f, GWorld.worldStates.GetStates(), beliefStates.GetStates(), null);

            //pass the first node through to start branching out the graph of plans from
            bool success = BuildGraph(start, leaves, usableActions, goal);

            //if a plan wasn't found
            if (!success)
            {
                // Debug.Log("NO PLAN");
                return null;
            }

            //of all the plans found, find the one that's cheapest to execute
            //and use that
            Node cheapest = null;
            foreach (Node leaf in leaves)
            {
                if (cheapest == null)
                {
                    cheapest = leaf;
                }
                else if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }

            List<GAction> result = new List<GAction>();
            Node n = cheapest;

            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action);
                }

                n = n.parent;
            }

            //make a queue out of the actions represented by the nodes in the plan
            //for the agent to work its way through
            Queue<GAction> queue = new Queue<GAction>();

            foreach (GAction a in result)
            {
                queue.Enqueue(a);
            }

            // Debug.Log("The Plan is: ");
            foreach (GAction a in queue)
            {
                // Debug.Log("Q: " + a.actionName);
            }

            return queue;
        }

        private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<GOAPString, int> goal)
        {
            bool foundPath = false;

            //with all the useable actions
            foreach (GAction action in usableActions)
            {
                //check their preconditions
                if (action.IsAhievableGiven(parent.state))
                {
                    //get the state of the world if the parent node were to be executed
                    Dictionary<GOAPString, int> currentState = new Dictionary<GOAPString, int>(parent.state);

                    //add the effects of this node to the nodes states to reflect what
                    //the world would look like if this node's action were executed
                    foreach (KeyValuePair<GOAPString, int> eff in action.baseSettings.afterEffects)
                    {
                        if (!currentState.ContainsKey(eff.Key))
                        {
                            currentState.Add(eff.Key, eff.Value);
                        }
                    }

                    //create the next node in the branch and set this current node as the parent
                    Node node = new Node(parent, parent.cost + action.baseSettings.cost, currentState, action);

                    //if the current state of the world after doing this node's action is the goal
                    //this plan will achieve that goal and will become the agent's plan
                    if (GoalAchieved(goal, currentState))
                    {
                        leaves.Add(node);
                        foundPath = true;
                    }
                    else
                    {
                        //if no goal has been found branch out to add other actions to the plan
                        List<GAction> subset = ActionSubset(usableActions, action);
                        bool found = BuildGraph(node, leaves, subset, goal);

                        if (found)
                        {
                            foundPath = true;
                        }
                    }
                }
            }
            return foundPath;
        }

        private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
        {
            List<GAction> subset = new List<GAction>();

            foreach (GAction a in actions)
            {
                if (!a.Equals(removeMe))
                {
                    subset.Add(a);
                }
            }

            return subset;
        }

        private bool GoalAchieved(Dictionary<GOAPString, int> goal, Dictionary<GOAPString, int> state)
        {
            foreach (KeyValuePair<GOAPString, int> g in goal)
            {
                if (!state.ContainsKey(g.Key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}