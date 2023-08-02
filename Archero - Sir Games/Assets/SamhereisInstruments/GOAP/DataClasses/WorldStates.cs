using SO.GOAP;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    [System.Serializable]
    public class GoapState
    {
        public GOAPString key;
        public int value;
    }

    public class GoapStates
    {
        public Dictionary<GOAPString, int> goapStates = new Dictionary<GOAPString, int>();

        public bool HasState(GOAPString key)
        {
            return goapStates.ContainsKey(key);
        }

        private void AddState(GOAPString key, int value)
        {
            goapStates.Add(key, value);
        }

        public void ModifyState(GOAPString key, int value)
        {
            if (HasState(key))
            {
                goapStates[key] += value;

                if (goapStates[key] <= 0)
                {
                    RemoveState(key);
                }
            }
            else
            {
                AddState(key, value);
            }
        }

        public void RemoveState(GOAPString key)
        {
            if (HasState(key))
            {
                goapStates.Remove(key);
            }
        }

        public void SetState(GOAPString key, int value)
        {
            if (HasState(key))
            {
                goapStates[key] = value;
            }
            else
            {
                AddState(key, value);
            }
        }

        public Dictionary<GOAPString, int> GetStates()
        {
            return goapStates;
        }
    }
}